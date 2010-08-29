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
                    string result = dislpayFormat.Replace("[[TITLE]]", this.Title)
                        .Replace("[[DESCRIPTION]]", this.Description)
                        .Replace("[[LISTENER]]", (this.Listener != -1) ? this.Listener.ToString() : "na")
                        .Replace("[[CATEGORY]]", this.Category)
                        .Replace("[[BIT]]", (this.Bitrate != -1) ? this.Bitrate.ToString() : "na");
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
        internal string Title
        {
            get { return title; }
            set { title = value; }
        }

        /// <summary>
        /// 番組の詳細
        /// </summary>
        private string description = string.Empty;

        /// <summary>
        /// 番組の詳細
        /// </summary>
        internal string Description
        {
            get { return description; }
            set { description = (value != null) ? value : string.Empty; }
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
        /// 番組の長さ
        /// </summary>
        private string length = string.Empty;

        /// <summary>
        /// 番組の長さ
        /// </summary>
        internal string Length
        {
            get { return length; }
            set { length = (value != null) ? value : string.Empty; }
        }

        /// <summary>
        /// 番組のタイプ
        /// </summary>
        private string type = string.Empty;

        /// <summary>
        /// 番組のタイプ
        /// </summary>
        internal string Type
        {
            get { return type; }
            set { type = (value != null) ? value : string.Empty; }
        }

        /// <summary>
        /// リスナー数
        /// </summary>
        private int listener = UNKNOWN_LISTENER_NUM;

        /// <summary>
        /// リスナー数を取得・設定する
        /// </summary>
        public int Listener
        {
            get { return listener; }
            set { listener = value; }
        }

        /// <summary>
        /// リスナー数が不明
        /// </summary>
        internal const int UNKNOWN_LISTENER_NUM = -1;

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
                if (this.PlayUrl != null)
                {
                    return new string[] { Title, Description, Category, PlayUrl.ToString() };
                }
                return new string[] { Title, Description, Category };
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

        public object Clone()
        {
            Channel channel = new Channel();
            channel.Title = (string)Title.Clone();
            if (this.PlayUrl != null)
            {
                channel.PlayUrl = new Uri(PlayUrl.ToString());
            }
            else
            {
                channel.PlayUrl = null;
            }
            if (this.WebSiteUrl != null)
            {
                channel.WebSiteUrl = new Uri(WebSiteUrl.ToString());
            }
            else
            {
                channel.WebSiteUrl = null;
            }
            if (this.Description != null)
            {
                channel.Description = (string)Description.Clone();
            }
            if (this.Category != null)
            {
                channel.Category = (string)Category.Clone();
            }
            channel.Listener = Listener;
            channel.Bitrate = Bitrate;
            if (this.Length != null)
            {
                channel.Length = (string)Length.Clone();
            }
            if (this.Type != null)
            {
                channel.Type = (string)Type.Clone();
            }
            return channel;
        }
    }
}
