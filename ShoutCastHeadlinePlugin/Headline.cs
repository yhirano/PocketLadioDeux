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
        private const string SHOUTCAST_URL = "http://www.shoutcast.com/directory/search_results.jsp";

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
        /// 空白。
        /// </summary>
        private readonly static Regex emptyRegex =
            new Regex(@"^\s+$", RegexOptions.None);

        /// <summary>
        /// HTML解析用正規表現。
        /// 放送局。
        /// </summary>
        private readonly static Regex stationRegex =
            new Regex(@"<div\s+[^>]*id=""\d+""[^>]*>$", RegexOptions.None);

        /// <summary>
        /// HTML解析用正規表現。
        /// Path解析用。
        /// </summary>
        private readonly static Regex pathRegex =
            new Regex(@"<a\s+[^>]*href=""(.*tunein-station\.pls[^""]*)""[^>]*>", RegexOptions.None);

        /// <summary>
        /// HTML解析用正規表現。
        /// Title解析用1。
        /// </summary>
        private readonly static Regex titleRegex1 =
            new Regex(@"Station:</span>", RegexOptions.None);

        /// <summary>
        /// HTML解析用正規表現。
        /// Title解析用2。
        /// </summary>
        private readonly static Regex titleRegex2 =
            new Regex(@"<a\s+[^>]*href=""([^""]*)""[^>]*>", RegexOptions.None);

        /// <summary>
        /// HTML解析用正規表現。
        /// Title解析用3。
        /// </summary>
        private readonly static Regex titleRegex3 =
            new Regex(@"^\s*(.+?)$", RegexOptions.None);

        /// <summary>
        /// HTML解析用正規表現。
        /// Playing解析用1。
        /// </summary>
        private readonly static Regex playingNowRegex1 =
            new Regex(@"Now Playing:</span>", RegexOptions.None);

        /// <summary>
        /// HTML解析用正規表現。
        /// Playing解析用2。
        /// </summary>
        private readonly static Regex playingNowRegex2 =
            new Regex(@"^\s*(.+?)</span>", RegexOptions.None);

        /// <summary>
        /// HTML解析用正規表現。
        /// Genre解析用1。
        /// </summary>
        private readonly static Regex genreRegex1 =
            new Regex(@"Genre:</span>", RegexOptions.None);

        /// <summary>
        /// HTML解析用正規表現。
        /// Genre解析用2。
        /// </summary>
        private readonly static Regex genreRegex2 =
            new Regex(@"(.+?)</span>", RegexOptions.None);

        /// <summary>
        /// HTML解析用正規表現。
        /// Listener解析用1。
        /// </summary>
        private readonly static Regex listenerRegex1 =
            new Regex(@"Listeners:</span>", RegexOptions.None);

        /// <summary>
        /// HTML解析用正規表現。
        /// Listener解析用2。
        /// </summary>
        private readonly static Regex listenerRegex2 =
            new Regex(@"([\d\,]+)</span>", RegexOptions.None);

        /// <summary>
        /// HTML解析用正規表現。
        /// BitRate解析用1。
        /// </summary>
        private readonly static Regex bitRateRegex1 =
            new Regex(@"Bitrate:</span>", RegexOptions.None);

        /// <summary>
        /// HTML解析用正規表現。
        /// BitRate解析用2。
        /// </summary>
        private readonly static Regex bitRateRegex2 =
            new Regex(@"(\d+)(\s*)kbps</span>", RegexOptions.None);

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
                string searchWord = ((Setting.SearchWord.Length != 0) ? "&s=" + Setting.SearchWord : string.Empty);
                // 半角スペースと全角スペースを+に置き換える SHOUTcast上のURLでAND検索のスペースが+に置き換えられるため
                searchWord = searchWord.Replace(' ', '+').Replace("　", "+");
                Uri url = new Uri(Headline.SHOUTCAST_URL + "?" + searchWord);

                st = connectionSetting.CreateStream(url);
                sr = new StreamReader(st, Encoding.GetEncoding("Windows-1252"));

                Channel channel = null;
                string text = sr.ReadToEnd();

                // タグの後に改行を入れる（Willcom高速化サービス対応のため）
                text = text.Replace(">", ">\n");

                string[] lines = text.Split('\n', '\r');

                for (int i = 0; i < lines.Length; ++i)
                {
                    if (fetchCancel == true)
                    {
                        return;
                    }

                    // 空行の場合
                    Match emptyMath = emptyRegex.Match(lines[i]);
                    if (lines[i] == string.Empty || emptyMath.Success)
                    {
                        continue;
                    }

                    Match stationMatch = stationRegex.Match(lines[i]);

                    if (stationMatch.Success)
                    {
                        for (++i; i < lines.Length; ++i)
                        {
                            /*** playlist.plsを検索 ***/
                            Match pathMatch = pathRegex.Match(lines[i]);

                            // tunein-station.plsが見つかった場合
                            if (pathMatch.Success)
                            {
                                channel = new Channel();

                                channel.PlayUrl = new Uri(pathMatch.Groups[1].Value);

                                /*** Titleを検索 ***/
                                Match titleMatch;

                                // Titleが見つからない場合は行を読み飛ばして検索する
                                for (++i; i < lines.Length; ++i)
                                {
                                    titleMatch = titleRegex1.Match(lines[i]);

                                    // Titleが見つかった場合
                                    if (titleMatch.Success)
                                    {
                                        for (++i; i < lines.Length; ++i)
                                        {
                                            titleMatch = titleRegex2.Match(lines[i]);
                                            if (titleMatch.Success)
                                            {
                                                channel.WebSiteUrl = new Uri(titleMatch.Groups[1].Value);
                                                break;
                                            }
                                        }

                                        for (++i; i < lines.Length; ++i)
                                        {
                                            titleMatch = titleRegex3.Match(lines[i]);
                                            if (titleMatch.Success)
                                            {
                                                channel.Title = titleMatch.Groups[1].Value;
                                                break;
                                            }
                                        }
                                        break;
                                    }
                                }

                                /*** Playingを検索 ***/
                                Match playingMatch;

                                // Playingが見つからない場合は行を読み飛ばして検索する
                                for (++i; i < lines.Length; ++i)
                                {
                                    playingMatch = playingNowRegex1.Match(lines[i]);

                                    // Playingが見つかった場合
                                    if (playingMatch.Success)
                                    {
                                        for (++i; i < lines.Length; ++i)
                                        {
                                            playingMatch = playingNowRegex2.Match(lines[i]);
                                            if (playingMatch.Success)
                                            {
                                                channel.Playing = playingMatch.Groups[1].Value;
                                                break;
                                            }
                                        }
                                        break;
                                    }
                                }

                                /*** Genreを検索 ***/
                                Match genreMatch;

                                // Genreが見つからない場合は行を読み飛ばして検索する
                                for (++i; i < lines.Length; ++i)
                                {
                                    genreMatch = genreRegex1.Match(lines[i]);

                                    // Genreが見つかった場合
                                    if (genreMatch.Success)
                                    {
                                        for (++i; i < lines.Length; ++i)
                                        {
                                            genreMatch = genreRegex2.Match(lines[i]);
                                            if (genreMatch.Success)
                                            {
                                                channel.Genre = genreMatch.Groups[1].Value;
                                                break;
                                            }
                                        }
                                        break;
                                    }
                                }

                                /*** Listenerを検索 ***/
                                Match listenerMatch;

                                // Listenerが見つからない場合は行を読み飛ばして検索する
                                for (++i; i < lines.Length; ++i)
                                {
                                    listenerMatch = listenerRegex1.Match(lines[i]);

                                    // Listenerが見つかった場合
                                    if (listenerMatch.Success)
                                    {
                                        for (++i; i < lines.Length; ++i)
                                        {
                                            listenerMatch = listenerRegex2.Match(lines[i]);
                                            if (listenerMatch.Success)
                                            {
                                                try
                                                {
                                                    channel.Listener = int.Parse(listenerMatch.Groups[1].Value.Replace(",", string.Empty));
                                                }
                                                catch (ArgumentException) { ; }
                                                catch (FormatException) { ; }
                                                catch (OverflowException) { ; }
                                                break;
                                            }
                                        }
                                        break;
                                    }
                                }

                                /*** Bitrateを検索 ***/
                                Match bitRateMatch;

                                // Bitrateが見つからない場合は行を読み飛ばして検索する
                                for (++i; i < lines.Length; ++i)
                                {
                                    bitRateMatch = bitRateRegex1.Match(lines[i]);

                                    // Bitrateが見つかった場合
                                    if (bitRateMatch.Success)
                                    {
                                        for (++i; i < lines.Length; ++i)
                                        {
                                            bitRateMatch = bitRateRegex2.Match(lines[i]);
                                            if (bitRateMatch.Success)
                                            {
                                                try
                                                {
                                                    channel.Bitrate = int.Parse(bitRateMatch.Groups[1].Value);
                                                }
                                                catch (ArgumentException) { ; }
                                                catch (FormatException) { ; }
                                                catch (OverflowException) { ; }
                                                break;
                                            }
                                        }
                                        break;
                                    }
                                }

                                if (channel != null)
                                {
                                    channel.DislpayFormat = Setting.DisplayFormat;
                                    channels.Add(channel);
                                    OnChannelAdded(channel);
                                    channel = null;
                                }
                                break;
                            }
                        }
                    }
                }

                OnChannelFetched();
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
