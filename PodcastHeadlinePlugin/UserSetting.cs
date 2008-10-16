using System;
using System.Collections.Generic;
using System.Text;

namespace PocketLadioDeux.PodcastHeadlinePlugin
{
    [Serializable()]
    public class UserSetting
    {
        /// <summary>
        /// ヘッドライン名
        /// </summary>
        private string name;

        /// <summary>
        /// ヘッドライン名を取得・設定する
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// ヘッドラインの表示方法
        /// </summary>
        private string displayFormat = "[[TITLE]] - [[DESCRIPTION]]";

        /// <summary>
        /// ヘッドラインの表示方法を取得・設定する
        /// </summary>
        public string DisplayFormat
        {
            get { return displayFormat; }
            set { displayFormat = value; }
        }

        /// <summary>
        /// RSSのURL
        /// </summary>
        private Uri rssUrl;

        /// <summary>
        /// RSSのURLを取得・設定する
        /// </summary>
        public Uri RssUrl
        {
            get { return rssUrl; }
            set { rssUrl = value; }
        }

        /// <summary>
        /// 一致単語フィルター
        /// </summary>
        private string[] filterMatchWords = new string[0];

        /// <summary>
        /// 一致単語フィルターを取得・設定する
        /// </summary>
        public string[] FilterMatchWords
        {
            get { return filterMatchWords; }
            set
            {
                filterMatchWords = value;
                OnFilterChanged();
            }
        }

        /// <summary>
        /// 除外単語フィルター
        /// </summary>
        private string[] filterExcludeWords = new string[0];

        /// <summary>
        /// 除外単語フィルターを取得・設定する
        /// </summary>
        public string[] FilterExcludeWords
        {
            get { return filterExcludeWords; }
            set
            {
                filterExcludeWords = value;
                OnFilterChanged();
            }
        }

        /// <summary>
        /// ソート種類の列挙
        /// </summary>
        public enum SortKinds
        {
            None, Title, Category, Author, Length
        }

        /// <summary>
        /// ソートの種類
        /// </summary>
        private SortKinds sortKind = SortKinds.None;

        /// <summary>
        /// ソートの種類を取得・設定する
        /// </summary>
        public SortKinds SortKind
        {
            get { return sortKind; }
            set
            {
                if (sortKind != value)
                {
                    sortKind = value;
                    OnFilterChanged();
                }
            }
        }

        /// <summary>
        /// ソートの昇順・降順の列挙
        /// </summary>
        public enum SortScendings
        {
            Ascending, Descending
        }

        /// <summary>
        /// ソートの昇順・降順
        /// </summary>
        private SortScendings sortScending = SortScendings.Ascending;

        /// <summary>
        /// ソートの昇順・降順を取得・設定する
        /// </summary>
        public SortScendings SortScending
        {
            get { return sortScending; }
            set
            {
                if (sortScending != value)
                {
                    sortScending = value;
                    OnFilterChanged();
                }
            }
        }

        /// <summary>
        /// フィルタ条件が変更になった場合に発生するイベント
        /// </summary>
        [field: NonSerializedAttribute()]
        internal event EventHandler FilterChangedEventHandler;

        /// <summary>
        /// FilterChangedEventHandlerの実行
        /// </summary>
        private void OnFilterChanged()
        {
            if (FilterChangedEventHandler != null)
            {
                FilterChangedEventHandler(this, EventArgs.Empty);
            }
        }

    }
}
