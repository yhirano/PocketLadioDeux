using System;
using PocketLadioDeux.HeadlinePluginInterface;

namespace PocketLadioDeux
{
    /// <summary>
    /// ヘッドラインイベントのEventArgs
    /// </summary>
    public class HeadlineEventArgs : EventArgs
    {
        /// <summary>
        /// キャンセルされたヘッドライン
        /// </summary>
        HeadlineBase headline;

        /// <summary>
        /// キャンセルされたヘッドラインを取得する
        /// </summary>
        public HeadlineBase Headline
        {
            get { return headline; }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="headline">ヘッドライン</param>
        public HeadlineEventArgs(HeadlineBase headline)
        {
            this.headline = headline;
        }
    }
}
