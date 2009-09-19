using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using MiscPocketCompactLibrary2.Net;
using PocketLadioDeux.HeadlinePluginInterface;

namespace PocketLadioDeux.IcecastHeadlinePlugin
{
    public sealed class Headline : HeadlineBase, IComparer<IChannel>
    {
        /// <summary>
        /// IcecastのURL
        /// </summary>
        private const string ICECAST_URL = "http://dir.xiph.org/yp.xml";

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
            get { return "Icecast"; }
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
                case UserSetting.SortKinds.ServerName:
                    return x.ServerName.CompareTo(y.ServerName)
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

        protected override void FetchHeadline()
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
                // Entryタグの中にいるか
                bool inEntry = false;

                st = connectionSetting.CreateStream(new Uri(Headline.ICECAST_URL));
                reader = new XmlTextReader(st);

                while (reader.Read())
                {
                    if (fetchCancel == true)
                    {
                        return;
                    }

                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        if (reader.LocalName == "entry")
                        {
                            inEntry = true;
                            channel = new Channel();
                        } // End of item
                        // Entryタグの中にいる場合
                        else if (inEntry == true)
                        {
                            if (reader.LocalName == "server_name")
                            {
                                channel.ServerName = reader.ReadString();
                            } // End of server_name
                            else if (reader.LocalName == "listen_url")
                            {
                                try
                                {
                                    channel.ListenUrl = new Uri(reader.ReadString().Trim());
                                }
                                catch (UriFormatException) { ; }
                            } // End of listen_url
                            else if (reader.LocalName == "server_type")
                            {
                                channel.ServerType = reader.ReadString();
                            } // End of server_type
                            else if (reader.LocalName == "bitrate")
                            {
                                try
                                {
                                    channel.Bitrate = int.Parse(reader.ReadString());
                                }
                                catch (ArgumentException) { ; }
                                catch (FormatException) { ; }
                                catch (OverflowException) { ; }
                            } // End of bitrate
                            else if (reader.LocalName == "channels")
                            {
                                channel.Channels = reader.ReadString();
                            } // End of channels
                            else if (reader.LocalName == "samplerate")
                            {
                                try
                                {
                                    channel.SampleRate = int.Parse(reader.ReadString());
                                }
                                catch (ArgumentException) { ; }
                                catch (FormatException) { ; }
                                catch (OverflowException) { ; }
                            } // End of samplerate
                            else if (reader.LocalName == "genre")
                            {
                                channel.Genre = reader.ReadString();
                            } // End of genre
                            else if (reader.LocalName == "current_song")
                            {
                                channel.CurrentSong = reader.ReadString();
                            } // End of current_song

                        } // End of entryタグの中にいる場合
                    }
                    else if (reader.NodeType == XmlNodeType.EndElement)
                    {
                        if (reader.LocalName == "entry")
                        {
                            inEntry = false;
                            if (channel != null)
                            {
                                channel.DislpayFormat = Setting.DisplayFormat;
                                channels.Add(channel);
                                OnChannelAdded(channel);
                                channel = null;
                            }
                        }
                    }
                }
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
