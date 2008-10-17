using System;

using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Xml.Serialization;
using MiscPocketCompactLibrary2.Net;
using PocketLadioDeux.HeadlinePluginInterface;

namespace PocketLadioDeux.ShoutCastHeadlinePlugin
{
    public sealed class Headline : HeadlineBase, IComparer<IChannel>
    {
        /// <summary>
        /// SHOUTcastのURL
        /// </summary>
        internal const string SHOUTCAST_URL = "http://classic.shoutcast.com/";

        /// <summary>
        /// 設定
        /// </summary>
        private UserSetting _setting;

        /// <summary>
        /// 設定を取得・設定する
        /// </summary>
        public UserSetting Setting
        {
            get { return _setting; }
            internal set
            {
                _setting = value;
                // フィルタ条件が変わった場合、フィルタのキャッシュを削除する
                _setting.FilterChangedEventHandler += delegate
                {
                    channelsMatchesToFilterCache = null;
                    channelsUnmatchesToFilterCache = null;
                };
            }
        }

        /// <summary>
        /// ヘッドライン名を取得・設定する
        /// </summary>
        public override string Name
        {
            get { return Setting.Name; }
            set { Setting.Name = value; }
        }

        public override string Kind
        {
            get { return "SHOUTcast"; }
        }

        /// <summary>
        /// 番組のリスト
        /// </summary>
        private List<Channel> channels = new List<Channel>();

        protected override IChannel[] Channels
        {
            get { return channels.ToArray(); }
        }

        /// <summary>
        /// フィルターにマッチした番組のキャッシュ
        /// </summary>
        private Channel[] channelsMatchesToFilterCache;

        protected override IChannel[] ChannelsMatchesToFilter
        {
            get
            {
                // キャッシュが空の場合、フィルタにマッチする番組とマッチしない番組を検索する
                if (channelsMatchesToFilterCache == null)
                {
                    List<Channel> matched = new List<Channel>();
                    List<Channel> unmatched = new List<Channel>();

                    foreach (Channel channel in Channels)
                    {
                        if (IsMatchToFilter(channel) == true)
                        {
                            matched.Add(channel);
                        }
                        else
                        {
                            unmatched.Add(channel);
                        }
                    }

                    // ソート
                    if (Setting.SortKind != UserSetting.SortKinds.None)
                    {
                        matched.Sort(Compare);
                        unmatched.Sort(Compare);
                    }

                    channelsMatchesToFilterCache = matched.ToArray();
                    channelsUnmatchesToFilterCache = unmatched.ToArray();
                }

                return channelsMatchesToFilterCache;
            }
        }

        /// <summary>
        /// フィルターにマッチしない番組のキャッシュ
        /// </summary>
        private Channel[] channelsUnmatchesToFilterCache;

        protected override IChannel[] ChannelsUnmatchesToFilter
        {
            get
            {
                // キャッシュが空の場合、フィルタにマッチする番組とマッチしない番組を検索する
                if (channelsUnmatchesToFilterCache == null)
                {
                    List<Channel> matched = new List<Channel>();
                    List<Channel> unmatched = new List<Channel>();

                    foreach (Channel channel in Channels)
                    {
                        if (IsMatchToFilter(channel) == true)
                        {
                            matched.Add(channel);
                        }
                        else
                        {
                            unmatched.Add(channel);
                        }
                    }

                    // ソート
                    if (Setting.SortKind != UserSetting.SortKinds.None)
                    {
                        matched.Sort(Compare);
                        unmatched.Sort(Compare);
                    }

                    channelsMatchesToFilterCache = matched.ToArray();
                    channelsUnmatchesToFilterCache = unmatched.ToArray();
                }

                return channelsUnmatchesToFilterCache;
            }
        }

        public override int Compare(IChannel _x, IChannel _y)
        {
            if (_x == null)
            {
                if (_y == null) { return 0; }
                else { return -1; }
            }

            Channel x = _x as Channel;
            Channel y = _y as Channel;
            if (x == null || y == null)
            {
                throw new ArgumentException();
            }

            switch (Setting.SortKind)
            {
                case UserSetting.SortKinds.Title:
                    return x.Title.CompareTo(y.Title)
                        // 逆順の場合はCompareToの結果を反転させる
                        * ((Setting.SortScending == UserSetting.SortScendings.Descending) ? -1 : 1);
                case UserSetting.SortKinds.Listener:
                    return x.Listener.CompareTo(y.Listener)
                        // 逆順の場合はCompareToの結果を反転させる
                        * ((Setting.SortScending == UserSetting.SortScendings.Descending) ? -1 : 1);
                case UserSetting.SortKinds.Bitrate:
                    return x.Bitrate.CompareTo(y.Bitrate)
                        // 逆順の場合はCompareToの結果を反転させる
                        * ((Setting.SortScending == UserSetting.SortScendings.Descending) ? -1 : 1);
                case UserSetting.SortKinds.None:
                default:
                    return 1;
            }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Headline()
        {
            Setting = new UserSetting();
        }

        #region HTML解析用正規表現

        /// <summary>
        /// HTML解析用正規表現。
        /// Path解析用。
        /// </summary>
        private readonly static Regex pathRegex =
            new Regex(@"<a\s+[^>]*href=""(.*playlist\.pls[^""]*)""[^>]*>", RegexOptions.None);

        /// <summary>
        /// HTML解析用正規表現。
        /// Rank解析用。
        /// </summary>
        private readonly static Regex rankRegex =
            new Regex(@"(\d+)</b>", RegexOptions.None);

        /// <summary>
        /// HTML解析用正規表現。
        /// Category解析用。
        /// </summary>
        private readonly static Regex categoryRegex =
            new Regex(@"^.*(\[.+?\])", RegexOptions.None);

        /// <summary>
        /// HTML解析用正規表現。
        /// ClusterUrl解析用。
        /// </summary>
        private readonly static Regex clusterUrlRegex =
            new Regex(@"<a\s+[^>]*href=""([^""]*)""[^>]*>", RegexOptions.None);

        /// <summary>
        /// HTML解析用正規表現。
        /// Title解析用。
        /// Willcom高速化サービス用。
        /// </summary>
        private readonly static Regex titleRegex =
            new Regex(@"(.+?)</a>", RegexOptions.None);

        /// <summary>
        /// HTML解析用正規表現。
        /// Listener解析用。
        /// </summary>
        private readonly static Regex listenerRegex =
            new Regex(@"(\d+)/(\d+)</font>", RegexOptions.None);

        /// <summary>
        /// HTML解析用正規表現。
        /// Playing解析用1。
        /// </summary>
        private readonly static Regex playingNowRegex =
            new Regex(@"Now Playing:</font>(.*)", RegexOptions.None);

        /// <summary>
        /// HTML解析用正規表現。
        /// Playing解析用2。
        /// </summary>
        private readonly static Regex playingRegex =
            new Regex(@"\s*(.+?)</font.*$", RegexOptions.None);

        /// <summary>
        /// HTML解析用正規表現。
        /// BitRate解析用。
        /// Willcom高速化サービス用。
        /// </summary>
        private readonly static Regex bitRateRegex =
            new Regex(@"(\d+)</font>", RegexOptions.None);

        /// <summary>
        /// HTML解析用正規表現。
        /// Rankらしき行の解析用。
        /// </summary>
        private readonly static Regex maybeRankLineRegex =
            new Regex(@"^.*</b>", RegexOptions.None);

        #endregion

        protected override void FetchHeadline(HttpConnection connectionSetting)
        {
            channels.Clear();
            // フィルターのクリア
            channelsMatchesToFilterCache = null;
            channelsUnmatchesToFilterCache = null;

            Stream st = null;
            StreamReader sr = null;
            try
            {
                string searchWord = ((Setting.SearchWord != string.Empty) ? "&s=" + Setting.SearchWord : string.Empty);
                // 半角スペースと全角スペースを+に置き換える SHOUTcast上のURLでAND検索のスペースが+に置き換えられるため
                searchWord = searchWord.Replace(' ', '+').Replace("　", "+");

                string perView = ((Setting.PerView.ToString() != string.Empty) ? "&numresult=" + Setting.PerView : string.Empty);
                Uri url = new Uri(Headline.SHOUTCAST_URL + "/?" + searchWord + perView);

                st = connectionSetting.CreateStream(url);
                sr = new StreamReader(st, Encoding.GetEncoding("Windows-1252"));

                Channel channel = null;
                string text = sr.ReadToEnd();

                // タグの後に改行を入れる（Willcom高速化サービス対応のため）
                text = text.Replace(">", ">\n");

                string[] lines = text.Split('\n', '\r');

                #region HTML解析

                // 順位らしき行
                string maybeRankLine = string.Empty;

                // 1～200行目まではHTMLを解析しない
                int analyzeHtmlFirstTo = 200;
                // 終端から250行前～終端まではHTMLを解析しない
                int analyzeHtmlLast = lines.Length - 250;

                // HTML解析
                for (int lineNumber = analyzeHtmlFirstTo; lineNumber < analyzeHtmlLast && lineNumber < lines.Length; ++lineNumber)
                {
                    if (fetchCancel == true)
                    {
                        return;
                    }

                    /*** playlist.plsを検索 ***/
                    Match pathMatch = pathRegex.Match(lines[lineNumber]);

                    // playlist.plsが見つかった場合
                    if (pathMatch.Success)
                    {
                        channel = new Channel();

                        channel.Path = pathMatch.Groups[1].Value;

                        /*** Rankを検索 ***/
                        Match rankMatch = rankRegex.Match(maybeRankLine);

                        // Rankが見つかった場合
                        if (rankMatch.Success)
                        {
                            channel.Rank = rankMatch.Groups[1].Value;
                        }

                        /*** Categoryを検索 ***/
                        Match categoryMatch;

                        // Categoryが見つからない場合は行を読み飛ばして検索する
                        for (++lineNumber; lineNumber < analyzeHtmlLast; ++lineNumber)
                        {
                            categoryMatch = categoryRegex.Match(lines[lineNumber]);

                            // Categoryが見つかった場合
                            if (categoryMatch.Success)
                            {
                                channel.Category = categoryMatch.Groups[1].Value;
                                break;
                            }
                        }

                        /*** ClusterUrlを検索 ***/
                        Match clusterUrlMatch;

                        // ClusterUrlが見つからない場合は行を読み飛ばして検索する
                        for (; lineNumber < analyzeHtmlLast; ++lineNumber)
                        {
                            clusterUrlMatch = clusterUrlRegex.Match(lines[lineNumber]);

                            // Categoryが見つかった場合
                            if (clusterUrlMatch.Success)
                            {
                                try
                                {
                                    channel.WebSiteUrl = new Uri(clusterUrlMatch.Groups[1].Value);
                                }
                                catch (UriFormatException)
                                {
                                    channel.WebSiteUrl = null;
                                }
                                break;
                            }
                        }

                        /*** Titleを検索 ***/
                        Match titleMatch;

                        // Titleが見つからない場合は行を読み飛ばして検索する
                        for (; lineNumber < analyzeHtmlLast; ++lineNumber)
                        {
                            titleMatch = titleRegex.Match(lines[lineNumber]);

                            // Titleが見つかった場合
                            if (titleMatch.Success)
                            {
                                channel.Title = titleMatch.Groups[1].Value;
                                break;
                            }
                        }

                        /*** Listenerを検索 ***/
                        Match listenerMatch = listenerRegex.Match(lines[lineNumber]);
                        for (; lineNumber < analyzeHtmlLast; ++lineNumber)
                        {
                            listenerMatch = listenerRegex.Match(lines[lineNumber]);

                            if (listenerMatch.Success)
                            {
                                break;
                            }

                            // Now Playing:は存在しない場合があるのでリスナー数検出の中でチェックを行う
                            Match playingNowMatch = playingNowRegex.Match(lines[lineNumber]);
                            if (playingNowMatch.Success)
                            {
                                Match playingMatch = playingRegex.Match(playingNowMatch.Groups[1].Value);
                                if (playingMatch.Success)
                                {
                                    channel.Playing = playingMatch.Groups[1].Value;
                                }
                            }

                        }
                        try
                        {
                            channel.Listener = int.Parse(listenerMatch.Groups[1].Value);
                            channel.ListenerTotal = int.Parse(listenerMatch.Groups[2].Value);
                        }
                        catch (ArgumentException) { ; }
                        catch (FormatException) { ; }
                        catch (OverflowException) { ; }

                        /*** Bitrateを検索 ***/
                        Match bitrateMatch;

                        // Bitrateが見つからない場合は行を読み飛ばして検索する
                        for (++lineNumber; lineNumber < analyzeHtmlLast; ++lineNumber)
                        {
                            bitrateMatch = bitRateRegex.Match(lines[lineNumber]);

                            // Bitrateが見つかった場合
                            if (bitrateMatch.Success)
                            {
                                try
                                {
                                    channel.Bitrate = int.Parse(bitrateMatch.Groups[1].Value);
                                    break;
                                }
                                catch (ArgumentException) { ; }
                                catch (FormatException) { ; }
                                catch (OverflowException) { ; }
                            }
                        }

                        if (channel != null)
                        {
                            channel.DislpayFormat = Setting.DisplayFormat;
                            channels.Add(channel);
                            OnChannelAdded(channel);
                            channel = null;
                        }
                    }

                    /*** Rankらしき行を保存する ***/
                    Match maybeRankLineMatch = maybeRankLineRegex.Match(lines[lineNumber]);

                    if (maybeRankLineMatch.Success)
                    {
                        maybeRankLine = lines[lineNumber];

                    }
                }

                OnChannelFetched();

                #endregion
            }
            finally
            {
                // フィルターのクリア
                /* 
                 * 本メソッドの先頭でもフィルターキャッシュをクリアしているが、本メソッドの実行中にフィルターを
                 * 使用した場合に、フィルターキャッシュの整合性がとれなくなるため、本メソッドの終了時に
                 * フィルターのキャッシュを削除してしまう。
                 */
                channelsMatchesToFilterCache = null;
                channelsUnmatchesToFilterCache = null;

                if (sr != null)
                {
                    sr.Close();
                }
                if (st != null)
                {
                    st.Close();
                }
            }
        }

        public override bool IsMatchToFilter(IChannel channel)
        {
            if (channel == null)
            {
                throw new ArgumentNullException();
            }

            // SHOUTcast以外の番組はフィルタリングできない
            Channel _channel = channel as Channel;
            if (_channel == null)
            {
                throw new ArgumentException();
            }

            #region 単語フィルター処理

            // 一致単語フィルター・除外フィルターが存在する場合
            if (Setting.FilterMatchWords.Length > 0 && Setting.FilterExcludeWords.Length > 0)
            {
                if (IsMatchFilterMatchWords(channel) == false || IsMatchFilterExcludeWords(channel) == true)
                {
                    return false;
                }
            }
            // 一致単語フィルターのみが存在する場合
            else if (Setting.FilterMatchWords.Length > 0 && Setting.FilterExcludeWords.Length <= 0)
            {
                if (IsMatchFilterMatchWords(channel) == false)
                {
                    return false;
                }
            }
            // 除外フィルターのみが存在する場合
            else if (Setting.FilterMatchWords.Length <= 0 && Setting.FilterExcludeWords.Length > 0)
            {
                if (IsMatchFilterExcludeWords(channel) == true)
                {
                    return false;
                }

            }
            // 単語フィルターが存在しない場合
            else
            {
                ;
            }

            #endregion

            #region 最低ビットレートフィルター処理

            // 最低ビットレートフィルターが存在する場合
            if (Setting.IsFilteringAboveBitrate == true)
            {
                if (0 < _channel.Bitrate && _channel.Bitrate < Setting.FilteringAboveBitrate)
                {
                    return false;
                }
            }

            #endregion

            #region 最大ビットレートフィルター処理

            // 最大ビットレートフィルターが存在する場合
            if (Setting.IsFilteringBelowBitrate == true)
            {
                if (_channel.Bitrate > Setting.FilteringBelowBitrate)
                {
                    return false;
                }
            }

            #endregion

            return true;
        }

        /// <summary>
        /// 番組が一致単語フィルターに合致するかを調べる
        /// </summary>
        /// <param name="channel">番組</param>
        /// <returns>番組が一致単語フィルターに合致したらtrue、それ以外はfalse</returns>
        private bool IsMatchFilterMatchWords(IChannel channel)
        {
            foreach (string filter in Setting.FilterMatchWords)
            {
                foreach (string filted in channel.FilteredWords)
                {
                    if (filted.ToLower(System.Globalization.CultureInfo.InvariantCulture).IndexOf(filter.ToLower(System.Globalization.CultureInfo.InvariantCulture)) != -1)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 番組が除外単語フィルターに合致するかを調べる
        /// </summary>
        /// <param name="channel">番組</param>
        /// <returns>番組が除外単語フィルターに合致したらtrue、それ以外はfalse</returns>
        private bool IsMatchFilterExcludeWords(IChannel channel)
        {
            foreach (string filter in Setting.FilterExcludeWords)
            {
                foreach (string filted in channel.FilteredWords)
                {
                    if (filted.ToLower(System.Globalization.CultureInfo.InvariantCulture).IndexOf(filter.ToLower(System.Globalization.CultureInfo.InvariantCulture)) != -1)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public override void ShowSettingForm()
        {
            SettingForm settingForm = new SettingForm(this);
            settingForm.ShowDialog();
            settingForm.Dispose();
        }

        public override void ShowSettingFormForAddFilter(string filterWord)
        {
            // TODO 未実装
            throw new NotImplementedException();
        }

        public override void Save(Stream stream)
        {
            XmlSerializer sr = new XmlSerializer(typeof(UserSetting));
            sr.Serialize(stream, Setting);
        }

        public override void Load(Stream stream)
        {
            XmlSerializer sr = new XmlSerializer(typeof(UserSetting));
            UserSetting setting = sr.Deserialize(stream) as UserSetting;
            if (setting != null)
            {
                Setting = setting;
            }
        }
    }
}
