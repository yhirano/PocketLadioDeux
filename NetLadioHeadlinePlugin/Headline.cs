using System;

using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Resources;
using System.Reflection;
using System.Xml.Serialization;
using OpenNETCF.ComponentModel;
using MiscPocketCompactLibrary2.Net;
using PocketLadioDeux.HeadlinePluginInterface;

namespace PocketLadioDeux.NetLadioHeadlinePlugin
{
    public sealed class Headline : HeadlineBase, IComparer<IChannel>
    {
        /// <summary>
        /// ねとらじのヘッドラインのURL DAT v2
        /// </summary>
        private const string NETLADIO_HEADLINE_DAT_V2_URL = "http://yp.ladio.livedoor.jp/stats/list.v2.dat";

        /// <summary>
        /// メッセージ表示用のリソース
        /// </summary>
        private readonly ResourceManager messagesResource = new ResourceManager("PocketLadioDeux.NetLadioHeadlinePlugin.MessagesResource", Assembly.GetExecutingAssembly());

        /// <summary>
        /// 設定
        /// </summary>
        private UserSetting _setting;

        /// <summary>
        /// 設定を取得・設定する
        /// </summary>
        internal UserSetting Setting
        {
            get { return _setting; }
            set
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
            get { return messagesResource.GetString("NetLadio"); }
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
                case UserSetting.SortKinds.Nam:
                    return x.Nam.CompareTo(y.Nam)
                        // 逆順の場合はCompareToの結果を反転させる
                        * ((Setting.SortScending == UserSetting.SortScendings.Descending) ? -1 : 1);
                case UserSetting.SortKinds.Tims:
                    return x.Tims.CompareTo(y.Tims)
                        // 逆順の場合はCompareToの結果を反転させる
                        * ((Setting.SortScending == UserSetting.SortScendings.Descending) ? -1 : 1);
                case UserSetting.SortKinds.Cln:
                    return x.Cln.CompareTo(y.Cln)
                        // 逆順の場合はCompareToの結果を反転させる
                        * ((Setting.SortScending == UserSetting.SortScendings.Descending) ? -1 : 1);
                case UserSetting.SortKinds.Clns:
                    return x.Clns.CompareTo(y.Clns)
                        // 逆順の場合はCompareToの結果を反転させる
                        * ((Setting.SortScending == UserSetting.SortScendings.Descending) ? -1 : 1);
                case UserSetting.SortKinds.Bit:
                    return x.Bit.CompareTo(y.Bit)
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

        #region dat v2解析用正規表現

        private static readonly Regex urlRegex = new Regex("^URL=(.*)", RegexOptions.None);

        private static readonly Regex gnlRegex = new Regex("^GNL=(.*)", RegexOptions.None);

        private static readonly Regex namRegex = new Regex("^NAM=(.*)", RegexOptions.None);

        private static readonly Regex mntRegex = new Regex("^MNT=(.*)", RegexOptions.None);

        private static readonly Regex timsRegex = new Regex("^TIMS=(.*)", RegexOptions.None);

        private static readonly Regex clnRegex = new Regex(@"^CLN=(\d+)", RegexOptions.None);

        private static readonly Regex clnsRegex = new Regex(@"^CLNS=(\d+)", RegexOptions.None);

        private static readonly Regex maxRegex = new Regex(@"^MNT=(\d+)", RegexOptions.None);

        private static readonly Regex srvRegex = new Regex("^SRV=(.*)", RegexOptions.None);

        private static readonly Regex prtRegex = new Regex("^PRT=(.*)", RegexOptions.None);

        private static readonly Regex bitRegex = new Regex(@"^BIT=(\d+)", RegexOptions.None);

        private static readonly Regex songRegex = new Regex("^SONG=(.*)", RegexOptions.None);

        #endregion // dat v2解析用正規表現

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
                st = connectionSetting.CreateStream(new Uri(NETLADIO_HEADLINE_DAT_V2_URL));
                sr = new StreamReader(st, Encoding.GetEncoding("shift-jis"));

                Channel channel = null;
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    if (fetchCancel == true)
                    {
                        return;
                    }

                    // Url取得
                    Match urlMatch = urlRegex.Match(line);
                    if (urlMatch.Success)
                    {
                        try
                        {
                            if (urlMatch.Groups[1].Value != string.Empty)
                            {
                                if (channel == null)
                                {
                                    channel = new Channel();
                                }
                                channel.WebSiteUrl = new Uri(urlMatch.Groups[1].Value);
                            }
                        }
                        catch (UriFormatException) { ; }

                        continue;
                    }

                    // Gnl取得
                    Match gnlMatch = gnlRegex.Match(line);
                    if (gnlMatch.Success)
                    {
                        if (channel == null)
                        {
                            channel = new Channel();
                        }
                        channel.Gnl = gnlMatch.Groups[1].Value;

                        continue;
                    }

                    Match namMatch = namRegex.Match(line);
                    if (namMatch.Success)
                    {
                        if (channel == null)
                        {
                            channel = new Channel();
                        }
                        channel.Nam = namMatch.Groups[1].Value;

                        continue;
                    }

                    Match mntMatch = mntRegex.Match(line);
                    if (mntMatch.Success)
                    {
                        if (channel == null)
                        {
                            channel = new Channel();
                        }
                        channel.Mnt = mntMatch.Groups[1].Value;

                        continue;
                    }

                    Match timsMatch = timsRegex.Match(line);
                    if (timsMatch.Success)
                    {
                        if (channel == null)
                        {
                            channel = new Channel();
                        }

                        try
                        {
                            channel.Tims = DateTime.ParseExact(timsMatch.Groups[1].Value, "yy'/'MM'/'dd HH':'mm':'ss",
                                System.Globalization.DateTimeFormatInfo.InvariantInfo,
                                System.Globalization.DateTimeStyles.None);
                        }
                        catch (FormatException)
                        {
                            channel.Tims = DateTime.Now;
                        }

                        continue;
                    }

                    // Cln取得
                    Match clnMatch = clnRegex.Match(line);
                    if (clnMatch.Success)
                    {
                        if (channel == null)
                        {
                            channel = new Channel();
                        }

                        try
                        {
                            channel.Cln = int.Parse(clnMatch.Groups[1].Value);
                        }
                        catch (ArgumentException) { ; }
                        catch (FormatException) { ; }
                        catch (OverflowException) { ; }

                        continue;
                    }

                    // Clns取得
                    Match clnsMatch = clnsRegex.Match(line);
                    if (clnsMatch.Success)
                    {
                        if (channel == null)
                        {
                            channel = new Channel();
                        }

                        try
                        {
                            channel.Clns = int.Parse(clnsMatch.Groups[1].Value);
                        }
                        catch (ArgumentException) { ; }
                        catch (FormatException) { ; }
                        catch (OverflowException) { ; }

                        continue;
                    }

                    Match maxMatch = maxRegex.Match(line);
                    if (maxMatch.Success)
                    {
                        if (channel == null)
                        {
                            channel = new Channel();
                        }

                        continue;
                    }

                    Match srvMatch = srvRegex.Match(line);
                    if (srvMatch.Success)
                    {
                        if (channel == null)
                        {
                            channel = new Channel();
                        }
                        channel.Srv = srvMatch.Groups[1].Value;

                        continue;
                    }

                    Match prtMatch = prtRegex.Match(line);
                    if (prtMatch.Success)
                    {
                        if (channel == null)
                        {
                            channel = new Channel();
                        }
                        channel.Prt = prtMatch.Groups[1].Value;

                        continue;
                    }

                    Match bitMatch = bitRegex.Match(line);
                    if (bitMatch.Success)
                    {
                        if (channel == null)
                        {
                            channel = new Channel();
                        }

                        try
                        {
                            channel.Bit = int.Parse(bitMatch.Groups[1].Value);
                        }
                        catch (ArgumentException) { ; }
                        catch (FormatException) { ; }
                        catch (OverflowException) { ; }

                        continue;
                    }

                    Match songMatch = songRegex.Match(line);
                    if (prtMatch.Success)
                    {
                        if (channel == null)
                        {
                            channel = new Channel();
                        }
                        channel.Tit = songMatch.Groups[1].Value;

                        continue;
                    }

                    if (line == string.Empty)
                    {
                        if (channel != null)
                        {
                            channel.DislpayFormat = Setting.DisplayFormat;
                            channels.Add(channel);
                            OnChannelAdded(channel);
                            channel = null;
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

            // ねとらじ以外の番組はフィルタリングできない
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
                if (0 < _channel.Bit && _channel.Bit < Setting.FilteringAboveBitrate)
                {
                    return false;
                }
            }

            #endregion

            #region 最大ビットレートフィルター処理

            // 最大ビットレートフィルターが存在する場合
            if (Setting.IsFilteringBelowBitrate == true)
            {
                if (_channel.Bit > Setting.FilteringBelowBitrate)
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
