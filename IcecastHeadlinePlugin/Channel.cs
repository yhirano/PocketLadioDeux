using System;
using System.Collections.Generic;
using System.Text;
using PocketLadioDeux.HeadlinePluginInterface;

namespace PocketLadioDeux.IcecastHeadlinePlugin
{
    public sealed class Channel : IChannel
    {
        /// <summary>
        /// 番組情報の表示文字列
        /// </summary>
        public string Display
        {
            get
            {
                if (dislpayFormat != string.Empty)
                {
                    string result = dislpayFormat.Replace("[[SERVERNAME]]", ServerName)
                        .Replace("[[SERVERTYPE]]", ServerType)
                        .Replace("[[GENRE]]", Genre)
                        .Replace("[[CURRENTSONG]]", CurrentSong)
                        .Replace("[[BIT]]", ((Bitrate != Channel.UNKNOWN_BITRATE) ? Bitrate.ToString() : "na"));

                    return result;
                }
                else
                {
                    return ServerName;
                }
            }
        }

        /// <summary>
        /// 表示フォーマット
        /// </summary>
        private string dislpayFormat;

        /// <summary>
        /// 表示フォーマットを設定する
        /// </summary>
        internal string DislpayFormat
        {
            set { dislpayFormat = value; }
        }

        public Uri PlayUrl
        {
            get { return listenUrl; }
            internal set { listenUrl = value; }
        }

        public Uri WebSiteUrl
        {
            get { return null; }
        }

        /// <summary>
        /// サーバー名
        /// </summary>
        private string serverName;

        /// <summary>
        /// サーバー名
        /// </summary>
        internal string ServerName
        {
            get { return serverName; }
            set { serverName = value; }
        }

        /// <summary>
        /// Url
        /// </summary>
        private Uri listenUrl;

        /// <summary>
        /// Url
        /// </summary>
        internal Uri ListenUrl
        {
            get { return listenUrl; }
            set { listenUrl = value; }
        }

        /// <summary>
        /// ストリーミングの種類
        /// </summary>
        private string serverType;

        /// <summary>
        /// ストリーミングの種類
        /// </summary>
        internal string ServerType
        {
            get { return serverType; }
            set { serverType = value; }
        }

        /// <summary>
        /// ビットレート
        /// </summary>
        private int bitrate = UNKNOWN_BITRATE;

        /// <summary>
        /// ビットレート
        /// </summary>
        internal int Bitrate
        {
            get { return bitrate; }
            set { bitrate = value; }
        }

        /// <summary>
        /// ビットレートが不明
        /// </summary>
        internal const int UNKNOWN_BITRATE = -1;

        /// <summary>
        /// チャンネル数
        /// </summary>
        private string channels;

        /// <summary>
        /// チャンネル数
        /// </summary>
        internal string Channels
        {
            get { return channels; }
            set { channels = value; }
        }

        /// <summary>
        /// サンプリングレート
        /// </summary>
        private int sampleRate = UNKNOWN_SAMPLE_RATE;

        /// <summary>
        /// サンプリングレート
        /// </summary>
        internal int SampleRate
        {
            get { return sampleRate; }
            set { sampleRate = value; }
        }

        /// <summary>
        /// サンプリングレートが不明
        /// </summary>
        internal const int UNKNOWN_SAMPLE_RATE = -1;

        /// <summary>
        /// ジャンル
        /// </summary>
        private string genre;

        /// <summary>
        /// ジャンル
        /// </summary>
        internal string Genre
        {
            get { return genre; }
            set { genre = value; }
        }

        /// <summary>
        /// 現在の音楽
        /// </summary>
        private string currentSong;

        /// <summary>
        /// 現在の音楽
        /// </summary>
        internal string CurrentSong
        {
            get { return currentSong; }
            set { currentSong = value; }
        }

        /// <summary>
        /// フィルタリング対象のワードを取得する。
        /// 返されたワードに従い、フィルタリングを行う。
        /// </summary>
        public string[] FilteredWords
        {
            get
            {
                if (PlayUrl != null)
                {
                    return new string[] { ServerName, Genre, CurrentSong, PlayUrl.ToString() };
                }
                else
                {
                    return new string[] { ServerName, Genre, CurrentSong };
                }
            }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Channel()
        {
        }

        /// <summary>
        /// 番組の詳細フォームを表示する
        /// </summary>
        public void ShowPropertyForm()
        {
            ChannelPropertyForm channelPropertyForm = new ChannelPropertyForm(this);
            channelPropertyForm.ShowDialog();
            channelPropertyForm.Dispose();
        }
    }
}
