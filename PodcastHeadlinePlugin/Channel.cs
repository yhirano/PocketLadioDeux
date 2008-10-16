using System;
using System.Collections.Generic;
using System.Text;
using PocketLadioDeux.HeadlinePluginInterface;

namespace PocketLadioDeux.PodcastHeadlinePlugin
{
    public sealed class Channel : IChannel, ICloneable
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
                    string result = dislpayFormat.Replace("[[TITLE]]", Title)
                        .Replace("[[DESCRIPTION]]", Description)
                        .Replace("[[CATEGORY]]", Category)
                        .Replace("[[AUTHOR]]", Author);

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
        internal string DislpayFormat
        {
            set { dislpayFormat = value; }
        }

        /// <summary>
        /// 再生URL
        /// </summary>
        private Uri playUrl;

        public Uri PlayUrl
        {
            get { return playUrl; }
            internal set { playUrl = value; }
        }

        /// <summary>
        /// 番組のサイト
        /// </summary>
        private Uri link;

        public Uri WebSiteUrl
        {
            get { return link; }
            internal set { link = value; }
        }

        /// <summary>
        /// 番組のタイトル
        /// </summary>
        private string title;

        /// <summary>
        /// 番組のタイトル
        /// </summary>
        internal string Title
        {
            get { return title; }
            set { title = value; }
        }

        /// <summary>
        /// 番組の詳細
        /// </summary>
        private string description;

        /// <summary>
        /// 番組の詳細
        /// </summary>
        internal string Description
        {
            get { return description; }
            set { description = value; }
        }

        /// <summary>
        /// 番組の配信日時
        /// </summary>
        private DateTime date = DateTime.Now;

        /// <summary>
        /// 番組の配信日時
        /// </summary>
        internal DateTime Date
        {
            get { return date; }
            set { date = value; }
        }

        /// <summary>
        /// 番組のカテゴリ
        /// </summary>
        private string category = string.Empty;

        /// <summary>
        /// 番組のカテゴリ
        /// </summary>
        internal string Category
        {
            get { return category; }
            set { category = value; }
        }

        /// <summary>
        /// 番組の著者
        /// </summary>
        private string author = string.Empty;

        /// <summary>
        /// 番組の著者
        /// </summary>
        internal string Author
        {
            get { return author; }
            set { author = value; }
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
            set { length = value; }
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
            set { type = value; }
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
                    return new string[] { Title, Description, Author, PlayUrl.ToString() };
                }
                else
                {
                    return new string[] { Title, Description, Author };
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

        public object Clone()
        {
            Channel channel = new Channel();
            channel.Title = (string)(Title.Clone());
            if (PlayUrl != null)
            {
                channel.PlayUrl = new Uri(PlayUrl.ToString());
            }
            else
            {
                channel.PlayUrl = null;
            }
            if (WebSiteUrl != null)
            {
                channel.WebSiteUrl = new Uri(WebSiteUrl.ToString());
            }
            else
            {
                channel.WebSiteUrl = null;
            }
            channel.Description = (string)(channel.Description.Clone());
            channel.Date = Date;
            channel.Category = (string)(Category.Clone());
            channel.Author = (string)(Author.Clone());
            channel.Length = (string)(Length.Clone());
            channel.Type = (string)(Type.Clone());

            return channel;
        }
    }
}
