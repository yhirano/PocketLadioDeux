using System;

using System.Collections.Generic;
using System.Text;

namespace PocketLadioDeux.ShoutCastHeadlinePlugin
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
        /// 検索単語
        /// </summary>
        private string searchWord = string.Empty;

        /// <summary>
        /// 検索単語を取得・設定する
        /// </summary>
        public string SearchWord
        {
            get { return searchWord; }
            set { searchWord = value; }
        }

        /// <summary>
        /// ヘッドラインの表示方法
        /// </summary>
        private string displayFormat = "[[TITLE]]";

        /// <summary>
        /// ヘッドラインの表示方法を取得・設定する
        /// </summary>
        public string DisplayFormat
        {
            get { return displayFormat; }
            set { displayFormat = value; }
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
        /// ビットレート（～以下）フィルターを使用するか
        /// </summary>
        private bool isFilteringBelowBitrate = false;

        /// <summary>
        /// ビットレート（～以下）フィルターを使用するかを取得・設定する
        /// </summary>
        public bool IsFilteringBelowBitrate
        {
            get { return isFilteringBelowBitrate; }
            set
            {
                if (isFilteringBelowBitrate != value)
                {
                    isFilteringBelowBitrate = value;
                    OnFilterChanged();
                }
            }
        }

        /// <summary>
        /// ビットレート（～以下）フィルター
        /// </summary>
        private int filteringBelowBitrate = 320;

        /// <summary>
        /// ビットレート（～以下）フィルターを取得・設定する
        /// </summary>
        public int FilteringBelowBitrate
        {
            get
            {
                if (filteringBelowBitrate >= 0)
                {
                    return filteringBelowBitrate;
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                if (value >= 0 && filteringBelowBitrate != value)
                {
                    filteringBelowBitrate = value;
                    OnFilterChanged();
                }
                else { ; }
            }
        }

        /// <summary>
        /// ビットレート（～以上）フィルターを使用するか
        /// </summary>
        private bool isFilteringAboveBitrate = false;

        /// <summary>
        /// ビットレート（～以上）フィルターを使用するかを取得・設定する
        /// </summary>
        public bool IsFilteringAboveBitrate
        {
            get { return isFilteringAboveBitrate; }
            set
            {
                if (isFilteringAboveBitrate != value)
                {
                    isFilteringAboveBitrate = value;
                    OnFilterChanged();
                }
            }
        }

        /// <summary>
        /// ビットレート（～以上）フィルター
        /// </summary>
        private int filteringAboveBitrate = 0;

        /// <summary>
        /// ビットレート（～以上）フィルターを取得・設定する
        /// </summary>
        public int FilteringAboveBitrate
        {
            get
            {
                if (filteringAboveBitrate >= 0)
                {
                    return filteringAboveBitrate;
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                if (value >= 0 && filteringAboveBitrate != value)
                {
                    filteringAboveBitrate = value;
                    OnFilterChanged();
                }
                else { ; }
            }
        }

        /// <summary>
        /// ソート種類の列挙
        /// </summary>
        public enum SortKinds
        {
            None, Title, Listener, Bitrate
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
