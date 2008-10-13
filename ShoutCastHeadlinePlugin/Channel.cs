using System;

using System.Collections.Generic;
using System.Text;
using PocketLadioDeux.HeadlinePluginInterface;

namespace PocketLadioDeux.ShoutCastHeadlinePlugin
{
    public sealed class Channel : IChannel
    {
        /// <summary>
        /// DSPツールで指定されるURL
        /// </summary>
        private Uri webSiteUrl;

        /// <summary>
        /// DSPツールで指定されるURLを設定する
        /// </summary>
        public Uri WebSiteUrl
        {
            get { return webSiteUrl; }
            internal set { webSiteUrl = value; }
        }

        /// <summary>
        /// 番組情報の表示文字列
        /// </summary>
        public string Display
        {
            get
            {
                if (dislpayFormat != string.Empty)
                {
                    string result = dislpayFormat.Replace("[[TITLE]]", Title)
                        .Replace("[[PLAYING]]", Playing)
                        .Replace("[[LISTENER]]", ((Listener != Channel.UNKNOWN_LISTENER_NUM) ? Listener.ToString() : "na"))
                        .Replace("[[GENRE]]", Genre)
                        .Replace("[[BIT]]", ((Bitrate != Channel.UNKNOWN_BITRATE) ? Bitrate.ToString() : "na"));

                    return result;
                }
                else
                {
                    return Title;
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
        public string DislpayFormat
        {
            set { dislpayFormat = value; }
        }

        /// <summary>
        /// 番組の放送URL
        /// </summary>
        private Uri playUrl;

        /// <summary>
        /// 番組の放送URLを取得・設定する
        /// </summary>
        public Uri PlayUrl
        {
            get { return playUrl; }
            internal set { playUrl = value; }
        }

        /// <summary>
        /// タイトル
        /// </summary>
        private string title = string.Empty;

        /// <summary>
        /// タイトル
        /// </summary>
        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        /// <summary>
        /// 現在演奏中の曲
        /// </summary>
        private string playing = string.Empty;

        /// <summary>
        /// 現在演奏中の曲
        /// </summary>
        public string Playing
        {
            get { return playing; }
            set { playing = value; }
        }

        /// <summary>
        /// リスナ数
        /// </summary>
        private int listener = UNKNOWN_LISTENER_NUM;

        /// <summary>
        /// リスナ数
        /// </summary>
        public int Listener
        {
            get { return listener; }
            set { listener = value; }
        }

        /// <summary>
        /// リスナ数が不明
        /// </summary>
        public const int UNKNOWN_LISTENER_NUM = -1;

        /// <summary>
        /// ジャンル
        /// </summary>
        private string genre = string.Empty;

        /// <summary>
        /// ジャンル
        /// </summary>
        public string Genre
        {
            get { return genre; }
            set { genre = value; }
        }

        /// <summary>
        /// ビットレート
        /// </summary>
        private int bitRate = UNKNOWN_BITRATE;

        /// <summary>
        /// ビットレート
        /// </summary>
        public int Bitrate
        {
            get { return bitRate; }
            set { bitRate = value; }
        }

        /// <summary>
        /// ビットレートが不明
        /// </summary>
        public const int UNKNOWN_BITRATE = -1;

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
                    return new string[] { Title, Genre, PlayUrl.ToString() };
                }
                else
                {
                    return new string[] { Title, Genre };
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
