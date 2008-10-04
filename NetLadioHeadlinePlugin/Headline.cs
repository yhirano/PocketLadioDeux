﻿using System;

using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Xml.Serialization;
using MiscPocketCompactLibrary2.Net;
using PocketLadioDeux.HeadlinePluginInterface;

namespace PocketLadioDeux.NetLadioHeadlinePlugin
{
    public sealed class Headline : HeadlineBase
    {
        /// <summary>
        /// ねとらじのヘッドラインのURL DAT v2
        /// </summary>
        private const string NETLADIO_HEADLINE_DAT_V2_URL = "http://yp.ladio.livedoor.jp/stats/list.v2.dat";

        /// <summary>
        /// 設定
        /// </summary>
        private UserSetting setting = new UserSetting();

        /// <summary>
        /// 設定を取得・設定する
        /// </summary>
        internal UserSetting Setting
        {
            get { return setting; }
            set { setting = value; }
        }

        /// <summary>
        /// ヘッドライン名を取得・設定する
        /// </summary>
        public override string Name
        {
            get { return setting.Name; }
            set { setting.Name = value; }
        }

        public override string Kind
        {
            get { return "ねとらじ"; }
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

                    #region ソート

                    switch (setting.SortKind)
                    {
                        case UserSetting.SortKinds.Nam:
                            matched.Sort(
                                delegate(Channel x, Channel y)
                                {
                                    if (x == null)
                                    {
                                        if (y == null) { return 0; }
                                        else { return -1; }
                                    }
                                    else
                                    {
                                        return x.Nam.CompareTo(y.Nam);
                                    }
                                });
                            unmatched.Sort(
                                delegate(Channel x, Channel y)
                                {
                                    if (x == null)
                                    {
                                        if (y == null) { return 0; }
                                        else { return -1; }
                                    }
                                    else
                                    {
                                        return x.Nam.CompareTo(y.Nam);
                                    }
                                });
                            break;
                        case UserSetting.SortKinds.Tims:
                            matched.Sort(
                                delegate(Channel x, Channel y)
                                {
                                    if (x == null)
                                    {
                                        if (y == null) { return 0; }
                                        else { return -1; }
                                    }
                                    else
                                    {
                                        return x.Tims.CompareTo(y.Tims);
                                    }
                                });
                            unmatched.Sort(
                                delegate(Channel x, Channel y)
                                {
                                    if (x == null)
                                    {
                                        if (y == null) { return 0; }
                                        else { return -1; }
                                    }
                                    else
                                    {
                                        return x.Tims.CompareTo(y.Tims);
                                    }
                                });
                            break;
                        case UserSetting.SortKinds.Cln:
                            matched.Sort(
                                delegate(Channel x, Channel y)
                                {
                                    if (x == null)
                                    {
                                        if (y == null) { return 0; }
                                        else { return -1; }
                                    }
                                    else
                                    {
                                        return x.Cln.CompareTo(y.Cln);
                                    }
                                });
                            unmatched.Sort(
                                delegate(Channel x, Channel y)
                                {
                                    if (x == null)
                                    {
                                        if (y == null) { return 0; }
                                        else { return -1; }
                                    }
                                    else
                                    {
                                        return x.Cln.CompareTo(y.Cln);
                                    }
                                });
                            break;
                        case UserSetting.SortKinds.Clns:
                            matched.Sort(
                                delegate(Channel x, Channel y)
                                {
                                    if (x == null)
                                    {
                                        if (y == null) { return 0; }
                                        else { return -1; }
                                    }
                                    else
                                    {
                                        return x.Clns.CompareTo(y.Clns);
                                    }
                                });
                            unmatched.Sort(
                                delegate(Channel x, Channel y)
                                {
                                    if (x == null)
                                    {
                                        if (y == null) { return 0; }
                                        else { return -1; }
                                    }
                                    else
                                    {
                                        return x.Clns.CompareTo(y.Clns);
                                    }
                                });
                            break;
                        case UserSetting.SortKinds.Bit:
                            matched.Sort(
                                delegate(Channel x, Channel y)
                                {
                                    if (x == null)
                                    {
                                        if (y == null) { return 0; }
                                        else { return -1; }
                                    }
                                    else
                                    {
                                        return x.Bit.CompareTo(y.Bit);
                                    }
                                });
                            unmatched.Sort(
                                delegate(Channel x, Channel y)
                                {
                                    if (x == null)
                                    {
                                        if (y == null) { return 0; }
                                        else { return -1; }
                                    }
                                    else
                                    {
                                        return x.Bit.CompareTo(y.Bit);
                                    }
                                });
                            break;
                        case UserSetting.SortKinds.None:
                        default:
                            break;
                    }

                    // 降順の場合
                    if (setting.SortKind != UserSetting.SortKinds.None && setting.SortScending == UserSetting.SortScendings.Descending)
                    {
                        matched.Reverse();
                        unmatched.Reverse();
                    }

                    #endregion // ソート

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

                    #region ソート

                    switch (setting.SortKind)
                    {
                        case UserSetting.SortKinds.Nam:
                            matched.Sort(
                                delegate(Channel x, Channel y)
                                {
                                    if (x == null)
                                    {
                                        if (y == null) { return 0; }
                                        else { return -1; }
                                    }
                                    else
                                    {
                                        return x.Nam.CompareTo(y.Nam);
                                    }
                                });
                            unmatched.Sort(
                                delegate(Channel x, Channel y)
                                {
                                    if (x == null)
                                    {
                                        if (y == null) { return 0; }
                                        else { return -1; }
                                    }
                                    else
                                    {
                                        return x.Nam.CompareTo(y.Nam);
                                    }
                                });
                            break;
                        case UserSetting.SortKinds.Tims:
                            matched.Sort(
                                delegate(Channel x, Channel y)
                                {
                                    if (x == null)
                                    {
                                        if (y == null) { return 0; }
                                        else { return -1; }
                                    }
                                    else
                                    {
                                        return x.Tims.CompareTo(y.Tims);
                                    }
                                });
                            unmatched.Sort(
                                delegate(Channel x, Channel y)
                                {
                                    if (x == null)
                                    {
                                        if (y == null) { return 0; }
                                        else { return -1; }
                                    }
                                    else
                                    {
                                        return x.Tims.CompareTo(y.Tims);
                                    }
                                });
                            break;
                        case UserSetting.SortKinds.Cln:
                            matched.Sort(
                                delegate(Channel x, Channel y)
                                {
                                    if (x == null)
                                    {
                                        if (y == null) { return 0; }
                                        else { return -1; }
                                    }
                                    else
                                    {
                                        return x.Cln.CompareTo(y.Cln);
                                    }
                                });
                            unmatched.Sort(
                                delegate(Channel x, Channel y)
                                {
                                    if (x == null)
                                    {
                                        if (y == null) { return 0; }
                                        else { return -1; }
                                    }
                                    else
                                    {
                                        return x.Cln.CompareTo(y.Cln);
                                    }
                                });
                            break;
                        case UserSetting.SortKinds.Clns:
                            matched.Sort(
                                delegate(Channel x, Channel y)
                                {
                                    if (x == null)
                                    {
                                        if (y == null) { return 0; }
                                        else { return -1; }
                                    }
                                    else
                                    {
                                        return x.Clns.CompareTo(y.Clns);
                                    }
                                });
                            unmatched.Sort(
                                delegate(Channel x, Channel y)
                                {
                                    if (x == null)
                                    {
                                        if (y == null) { return 0; }
                                        else { return -1; }
                                    }
                                    else
                                    {
                                        return x.Clns.CompareTo(y.Clns);
                                    }
                                });
                            break;
                        case UserSetting.SortKinds.Bit:
                            matched.Sort(
                                delegate(Channel x, Channel y)
                                {
                                    if (x == null)
                                    {
                                        if (y == null) { return 0; }
                                        else { return -1; }
                                    }
                                    else
                                    {
                                        return x.Bit.CompareTo(y.Bit);
                                    }
                                });
                            unmatched.Sort(
                                delegate(Channel x, Channel y)
                                {
                                    if (x == null)
                                    {
                                        if (y == null) { return 0; }
                                        else { return -1; }
                                    }
                                    else
                                    {
                                        return x.Bit.CompareTo(y.Bit);
                                    }
                                });
                            break;
                        case UserSetting.SortKinds.None:
                        default:
                            break;
                    }

                    // 降順の場合
                    if (setting.SortKind != UserSetting.SortKinds.None && setting.SortScending == UserSetting.SortScendings.Descending)
                    {
                        matched.Reverse();
                        unmatched.Reverse();
                    }

                    #endregion // ソート

                    channelsMatchesToFilterCache = matched.ToArray();
                    channelsUnmatchesToFilterCache = unmatched.ToArray();
                }

                return channelsUnmatchesToFilterCache;
            }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Headline()
        {
            // フィルタ条件が変わった場合、フィルタのキャッシュを削除する
            setting.FilterChangedEventHandler += delegate
            {
                channelsMatchesToFilterCache = null;
                channelsUnmatchesToFilterCache = null;
            };
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
                            channel.DislpayFormat = setting.DisplayFormat;
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
            if (setting.FilterMatchWords.Length > 0 && setting.FilterExcludeWords.Length > 0)
            {
                if (IsMatchFilterMatchWords(channel) == false || IsMatchFilterExcludeWords(channel) == true)
                {
                    return false;
                }
            }
            // 一致単語フィルターのみが存在する場合
            else if (setting.FilterMatchWords.Length > 0 && setting.FilterExcludeWords.Length <= 0)
            {
                if (IsMatchFilterMatchWords(channel) == false)
                {
                    return false;
                }
            }
            // 除外フィルターのみが存在する場合
            else if (setting.FilterMatchWords.Length <= 0 && setting.FilterExcludeWords.Length > 0)
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
            if (setting.IsFilteringAboveBitrate == true)
            {
                if (0 < _channel.Bit && _channel.Bit < setting.FilteringAboveBitrate)
                {
                    return false;
                }
            }

            #endregion

            #region 最大ビットレートフィルター処理

            // 最大ビットレートフィルターが存在する場合
            if (setting.IsFilteringBelowBitrate == true)
            {
                if (_channel.Bit > setting.FilteringBelowBitrate)
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
            foreach (string filter in setting.FilterMatchWords)
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
            foreach (string filter in setting.FilterExcludeWords)
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
            sr.Serialize(stream, setting);
        }

        public override void Load(Stream stream)
        {
            XmlSerializer sr = new XmlSerializer(typeof(UserSetting));
            UserSetting setting = sr.Deserialize(stream) as UserSetting;
            if (setting != null)
            {
                this.setting = setting;
            }
        }
    }
}
