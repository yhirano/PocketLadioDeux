using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Xml;
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
        internal const string SHOUTCAST_URL = "http://yp.shoutcast.com/sbin/newxml.phtml";

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

        #region 解析用正規表現

        /// <summary>
        /// Listener解析用正規表現
        /// </summary>
        private readonly static Regex listenerRegex = new Regex(@"\[Listeners:(\s*)(\d+)\]", RegexOptions.None);

        /// <summary>
        /// Bitrate解析用正規表現
        /// </summary>
        private readonly static Regex bitRateRegex = 
            new Regex(@"\[Bitrate:(\s*)(\d+)\]", RegexOptions.None);

        #endregion

        /// <summary>
        /// ヘッドラインのフォーマット種類
        /// </summary>
        private enum FetchHeadlineFormats
        {
            StationList,
            Rss,
            None
        }

        /// <summary>
        /// ヘッドラインのフォーマット種類
        /// </summary>
        private FetchHeadlineFormats fetchHeadlineFormatStatus = FetchHeadlineFormats.None;

        protected override void FetchHeadline()
        {
            channels.Clear();
            // フィルターのクリア
            channelsMatchesToFilterCache = null;
            channelsUnmatchesToFilterCache = null;
            fetchHeadlineFormatStatus = FetchHeadlineFormats.None;

            // ヘッドラインが取得できない場合は取得を3回繰り返す。
            // RSSタイプのヘッドラインが取得できないことがある。
            for (int i = 0; (i < 3) && (fetchHeadlineFormatStatus != FetchHeadlineFormats.Rss); ++i)
            {
                fetchHeadline();
            }
        }

        /// <summary>
        /// ヘッドラインを取得する
        /// </summary>
        private void fetchHeadline()
        {
            fetchHeadlineFormatStatus = FetchHeadlineFormats.None;

            Stream input = null;
            XmlReader reader = null;

            try
            {
                // 番組
                Channel channel = null;

                // Itemタグの中にいるか
                bool inItenFlag = false;
                // Enclosureの一時リスト
                List<Enclosure> list = new List<Enclosure>();

                // URLを生成
                string seachWord;
                if (Setting.SearchWord != string.Empty)
                {
                    seachWord = "&search=" + Uri.EscapeUriString(Setting.SearchWord.Replace(' ', '+').Replace("　", "+"));
                }
                // 検索単語が空の場合はサーバからヘッドラインが返ってこないので、"Top 40"ジャンルのヘッドラインを取得してごまかす
                else
                {
                    seachWord = "&genre=" + Uri.EscapeUriString("Top 40");
                }
                string limitStr = (Setting.PerView.ToString() != string.Empty) ? ("&limit=" + this.Setting.PerView) : string.Empty;
                Uri uri = new Uri(SHOUTCAST_URL + "?rss=1" + seachWord + limitStr);

                input = connectionSetting.CreateStream(uri);
                reader = new XmlTextReader(input);

                while (reader.Read())
                {
                    if ((fetchCancel == true) || (fetchHeadlineFormatStatus == FetchHeadlineFormats.StationList))
                    {
                        return;
                    }

                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        if ((fetchHeadlineFormatStatus == FetchHeadlineFormats.None) && (reader.LocalName == "rss"))
                        {
                            fetchHeadlineFormatStatus = FetchHeadlineFormats.Rss;
                        }
                        else if ((fetchHeadlineFormatStatus == FetchHeadlineFormats.None) && (reader.LocalName == "stationlist"))
                        {
                            fetchHeadlineFormatStatus = FetchHeadlineFormats.StationList;
                        }
                        else if (reader.LocalName == "item")
                        {
                            inItenFlag = true;
                            channel = new Channel();
                        } // End of item
                        // itemタグの中にいる場合
                        else if (inItenFlag)
                        {
                            if (reader.LocalName == "title")
                            {
                                channel.Title = reader.ReadString();
                            } // End of title
                            else if (reader.LocalName == "description")
                            {
                                channel.Description = reader.ReadString();
                                Match match = listenerRegex.Match(channel.Description);
                                if (match.Success == true)
                                {
                                    try
                                    {
                                        channel.Listener = int.Parse(match.Groups[2].Value);
                                    }
                                    catch (ArgumentException) { ; }
                                    catch (FormatException) { ; }
                                    catch (OverflowException) { ; }
                                }
                                Match match2 = bitRateRegex.Match(channel.Description);
                                if (match2.Success == true)
                                {
                                    try
                                    {
                                        channel.Bitrate = int.Parse(match2.Groups[2].Value);
                                    }
                                    catch (ArgumentException) { ; }
                                    catch (FormatException) { ; }
                                    catch (OverflowException) { ; }
                                }
                            } // End of description
                            else if (reader.LocalName == "category")
                            {
                                if (channel.Category == string.Empty)
                                {
                                    channel.Category = reader.ReadString();
                                }
                                else
                                {
                                    channel.Category = channel.Category + "," + reader.ReadString();
                                }
                            } // End of category
                            else if (reader.LocalName == "enclosure")
                            {
                                Uri url = null;
                                string length = string.Empty;
                                string type = string.Empty;
                                try
                                {
                                    if (reader.MoveToFirstAttribute())
                                    {
                                        url = new Uri(reader.GetAttribute("url"));
                                        length = reader.GetAttribute("length");
                                        type = reader.GetAttribute("type");
                                    }
                                    if (length == null)
                                    {
                                        length = string.Empty;
                                    }
                                    if (type == null)
                                    {
                                        type = string.Empty;
                                    }

                                    // Enclosureタグの数だけ、 Enclosure一時リストにEnclosureの内容を追加していく
                                    Enclosure item = new Enclosure(url, length, type);
                                    if (item.IsPodcast())
                                    {
                                        list.Add(item);
                                    }
                                }
                                catch (UriFormatException) { ; }
                            } // End of enclosure
                        }
                    }
                    else if ((reader.NodeType == XmlNodeType.EndElement) && (reader.LocalName == "item"))
                    {
                        inItenFlag = false;
                        if (channel != null)
                        {
                            channel.DislpayFormat = this.Setting.DisplayFormat;

                            // Enclosureの要素の数だけ、Channelの複製を作る
                            if (list.Count != 0)
                            {
                                foreach (Enclosure enclosure in list)
                                {
                                    Channel channel2 = (Channel)channel.Clone();
                                    channel2.PlayUrl = enclosure.Url;
                                    channel2.Length = enclosure.Length;
                                    channel2.Type = enclosure.Type;
                                    channels.Add(channel2);
                                    OnChannelAdded(channel2);
                                }
                            }
                        }

                        // Enclosure一時リストをクリア
                        list.Clear();

                        channel = null;
                    }
                } // End of itemタグの中にいる場合
            }
            finally
            {
                channelsMatchesToFilterCache = null;
                channelsUnmatchesToFilterCache = null;
                if (reader != null)
                {
                    reader.Close();
                }
                if (input != null)
                {
                    input.Close();
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

        /// <summary>
        /// RSSのEnclosure要素
        /// </summary>
        private class Enclosure
        {
            Uri url;

            public Uri Url
            {
                get { return url; }
            }

            string length;

            public string Length
            {
                get { return length; }
            }

            string type;

            public string Type
            {
                get { return type; }
            }

            public Enclosure(Uri url, string length, string type)
            {
                this.url = url;
                this.length = length;
                this.type = type;
            }

            /// <summary>
            /// 再生可能と思われるPodcastのタイプ
            /// </summary>
            private readonly string[] podcastTypes = new string[]
            {
                "audio/mpeg",
                "audio/mp3",
                "audio/mpg",
                "audio/x-mpeg",
                "audio/mpeg3",
                "audio/x-mpeg3",
                "audio/ogg",
                "application/ogg",
                "application/x-ogg",
                "audio/x-ms-wma",
                "video/mp4",
                "video/x-m4v",
                "video/x-ms-wmv",
                "application/x-ms-wmv",
                "application/x-mplayer2",
                "video/x-ms-asf",
                "video/x-ms-wm",
                "video/x-ms-asf-plugin",
                "video/mpeg",
                "video/mpg",
                "video/x-mpeg",
                "video/avi",
                "video/msvideo",
                "video/x-msvideo",
                "application/octet-stream",
                "application/x-drm-v2",
                "audio/wav",
                "audio/x-wav"
            };

            /// <summary>
            /// このEnclosure要素は再生可能なPodcastかを判断する
            /// </summary>
            /// <returns>このEnclosure要素は再生可能なPodcastが再生可能な場合はtrue、それ以外はfalse</returns>
            internal bool IsPodcast()
            {
                if (type == null || type == string.Empty)
                {
                    return false;
                }

                return (Array.IndexOf<string>(podcastTypes, type) != -1);
            }
        }
    }
}
