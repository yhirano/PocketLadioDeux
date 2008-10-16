using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Resources;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;
using System.Windows.Forms;
using MiscPocketCompactLibrary2.Net;
using PocketLadioDeux.HeadlinePluginInterface;

namespace PocketLadioDeux.PodcastHeadlinePlugin
{
    public sealed class Headline : HeadlineBase, IComparer<IChannel>
    {
        /// <summary>
        /// メッセージ表示用のリソース
        /// </summary>
        private readonly ResourceManager messagesResource = new ResourceManager("PocketLadioDeux.PodcastHeadlinePlugin.MessagesResource", Assembly.GetExecutingAssembly());


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
            get { return "Podcast"; }
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
                case UserSetting.SortKinds.Category:
                    return x.Category.CompareTo(y.Category)
                        // 逆順の場合はCompareToの結果を反転させる
                        * ((Setting.SortScending == UserSetting.SortScendings.Descending) ? -1 : 1);
                case UserSetting.SortKinds.Author:
                    return x.Author.CompareTo(y.Author)
                        // 逆順の場合はCompareToの結果を反転させる
                        * ((Setting.SortScending == UserSetting.SortScendings.Descending) ? -1 : 1);
                case UserSetting.SortKinds.Length:
                    return x.Length.CompareTo(y.Length)
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

        public override void CreatedHeadlineByManual()
        {
            base.CreatedHeadlineByManual();

            // RSS URLを入力するように促す
            MessageBox.Show(messagesResource.GetString("PleaseInputRssUrl"), messagesResource.GetString("Infomation"), MessageBoxButtons.OK,
                MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
            SettingForm settingForm = new SettingForm(this);
            settingForm.ShowDialogAndFocusRssUrl();
            settingForm.Dispose();
        }

        protected override void FetchHeadline(HttpConnection connectionSetting)
        {
            channels.Clear();
            // フィルターのクリア
            channelsMatchesToFilterCache = null;
            channelsUnmatchesToFilterCache = null;

            Stream st = null;
            XmlReader reader = null;
            try
            {
                // 番組
                Channel channel = null;
                // itemタグの中にいるか
                bool inItemFlag = false;
                // Enclosureの一時リスト
                List<Enclosure> enclosuresTemp = new List<Enclosure>();

                st = connectionSetting.CreateStream(Setting.RssUrl);
                reader = new XmlTextReader(st);

                while (reader.Read())
                {
                    if (fetchCancel == true)
                    {
                        return;
                    }

                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        if (reader.LocalName == "item")
                        {
                            inItemFlag = true;
                            channel = new Channel();
                        } // End of item
                        // itemタグの中にいる場合
                        else if (inItemFlag == true)
                        {
                            if (reader.LocalName == "title")
                            {
                                channel.Title = reader.ReadString();
                            } // End of title
                            else if (reader.LocalName == "description")
                            {
                                channel.Description = reader.ReadString();
                            } // End of description
                            else if (reader.LocalName == "link")
                            {
                                try
                                {
                                    channel.WebSiteUrl = new Uri(reader.ReadString());
                                }
                                catch (UriFormatException) { ; }
                            } // End of link
                            else if (reader.LocalName == "pubDate")
                            {
                                string dateString = reader.ReadString();
                                try
                                {
                                    channel.Date = DateTime.ParseExact(dateString, "ddd, d MMM yyyy HH':'mm':'ss zzz",
                                        System.Globalization.DateTimeFormatInfo.InvariantInfo,
                                        System.Globalization.DateTimeStyles.None);
                                }
                                catch (FormatException)
                                {
                                    try
                                    {
                                        channel.Date = DateTime.Parse(dateString,
                                            System.Globalization.DateTimeFormatInfo.InvariantInfo,
                                            System.Globalization.DateTimeStyles.None);
                                    }
                                    catch (FormatException)
                                    {
                                        channel.Date = DateTime.Now;
                                    }
                                }
                            } // End of pubDate
                            else if (reader.LocalName == "category")
                            {
                                channel.Category = reader.ReadString();
                            } // End of category
                            else if (reader.LocalName == "author")
                            {
                                channel.Author = reader.ReadString();
                            } // End of author
                            else if (reader.LocalName == "guid")
                            {
                                try
                                {
                                    channel.WebSiteUrl = new Uri(reader.ReadString());
                                }
                                catch (UriFormatException) { ; }
                            } // End of guid
                            else if (reader.LocalName == "enclosure")
                            {
                                Uri enclosureUrl = null;
                                string enclosureLength = string.Empty;
                                string enclosureType = string.Empty;

                                try
                                {
                                    if (reader.MoveToFirstAttribute())
                                    {
                                        enclosureUrl = new Uri(reader.GetAttribute("url"));
                                        enclosureLength = reader.GetAttribute("length");
                                        enclosureType = reader.GetAttribute("type");
                                    }

                                    if (enclosureLength == null)
                                    {
                                        enclosureLength = string.Empty;
                                    }
                                    if (enclosureType == null)
                                    {
                                        enclosureType = string.Empty;
                                    }

                                    // Enclosureタグの数だけ、 Enclosure一時リストにEnclosureの内容を追加していく
                                    Enclosure enclosure = new Enclosure(enclosureUrl, enclosureLength, enclosureType);
                                    if (enclosure.IsPodcast() == true)
                                    {
                                        enclosuresTemp.Add(enclosure);
                                    }
                                }
                                catch (UriFormatException) { ; }
                            } // End of enclosure
                        } // End of itemタグの中にいる場合
                    }
                    else if (reader.NodeType == XmlNodeType.EndElement)
                    {
                        if (reader.LocalName == "item")
                        {
                            inItemFlag = false;
                            if (channel != null)
                            {
                                channel.DislpayFormat = Setting.DisplayFormat;
                                // Enclosureの要素の数だけ、Channelの複製を作る
                                if (enclosuresTemp.Count != 0)
                                {
                                    foreach (Enclosure enclosure in enclosuresTemp)
                                    {
                                        Channel clonedChannel = (Channel)channel.Clone();
                                        clonedChannel.PlayUrl = enclosure.Url;
                                        clonedChannel.Length = enclosure.Length;
                                        clonedChannel.Type = enclosure.Type;
                                        channels.Add(clonedChannel);
                                        OnChannelAdded(channel);
                                    }
                                }
                            }

                            // Enclosure一時リストをクリア
                            enclosuresTemp.Clear();

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

                if (reader != null)
                {
                    reader.Close();
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
            public bool IsPodcast()
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
