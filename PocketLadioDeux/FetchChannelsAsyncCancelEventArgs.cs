using System;
using PocketLadioDeux.HeadlinePluginInterface;

namespace PocketLadioDeux
{
    /// <summary>
    /// ヘッドラインから番組の情報を取得中にキャンセルされたときに発生するイベントのEventArgs
    /// </summary>
    public class FetchChannelsAsyncCancelEventArgs : EventArgs
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
        /// <param name="headline">キャンセルされたヘッドライン</param>
        public FetchChannelsAsyncCancelEventArgs(HeadlineBase headline)
        {
            this.headline = headline;
        }
    }
}
