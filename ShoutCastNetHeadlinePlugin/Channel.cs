using System;

using System.Collections.Generic;
using System.Text;
using PocketLadioDeux.HeadlinePluginInterface;

namespace PocketLadioDeux.ShoutCastNetHeadlinePlugin
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
                    string result = dislpayFormat.Replace("[[RANK]]", Rank)
                        .Replace("[[TITLE]]", Title)
                        .Replace("[[PLAYING]]", Playing)
                        .Replace("[[LISTENER]]", ((Listener != Channel.UNKNOWN_LISTENER_NUM) ? Listener.ToString() : "na"))
                        .Replace("[[LISTENERTOTAL]]", ((ListenerTotal != Channel.UNKNOWN_LISTENER_NUM) ? ListenerTotal.ToString() : "na"))
                        .Replace("[[CATEGORY]]", Category)
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
        private string dislpayFormat = string.Empty;

        /// <summary>
        /// 表示フォーマットを設定する
        /// </summary>
        internal string DislpayFormat
        {
            set { dislpayFormat = value; }
        }

        /// <summary>
        /// 番組の放送URLを取得・設定する
        /// </summary>
        public Uri PlayUrl
        {
            get
            {
                try
                {
                    return new Uri(Headline.SHOUTCAST_NET_URL + path);
                }
                catch (UriFormatException)
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// 再生URLへのパス
        /// </summary>
        private string path = string.Empty;

        /// <summary>
        /// 再生URLへのパス
        /// </summary>
        internal string Path
        {
            set { path = value; }
        }

        /// <summary>
        /// ランク
        /// </summary>
        private string rank = string.Empty;

        /// <summary>
        /// ランク
        /// </summary>
        internal string Rank
        {
            get { return rank; }
            set { rank = value; }
        }

        /// <summary>
        /// タイトル
        /// </summary>
        private string title = string.Empty;

        /// <summary>
        /// タイトル
        /// </summary>
        internal string Title
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
        internal string Playing
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
        internal int Listener
        {
            get { return listener; }
            set { listener = value; }
        }

        /// <summary>
        /// リスナ数が不明
        /// </summary>
        internal const int UNKNOWN_LISTENER_NUM = -1;

        /// <summary>
        /// 述べリスナ数
        /// </summary>
        private int listenerTotal = UNKNOWN_LISTENER_NUM;

        /// <summary>
        /// 述べリスナ数
        /// </summary>
        internal int ListenerTotal
        {
            get { return listenerTotal; }
            set { listenerTotal = value; }
        }

        /// <summary>
        /// カテゴリ
        /// </summary>
        private string category = string.Empty;

        /// <summary>
        /// カテゴリ
        /// </summary>
        internal string Category
        {
            get { return category; }
            set { category = value; }
        }

        /// <summary>
        /// ビットレート
        /// </summary>
        private int bitRate = UNKNOWN_BITRATE;

        /// <summary>
        /// ビットレート
        /// </summary>
        internal int Bitrate
        {
            get { return bitRate; }
            set { bitRate = value; }
        }

        /// <summary>
        /// ビットレートが不明
        /// </summary>
        internal const int UNKNOWN_BITRATE = -1;

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
                    return new string[] { Title, Category, PlayUrl.ToString() };
                }
                else
                {
                    return new string[] { Title, Category };
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
