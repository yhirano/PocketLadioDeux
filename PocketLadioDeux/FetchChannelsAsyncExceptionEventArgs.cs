using System;
using PocketLadioDeux.HeadlinePluginInterface;

namespace PocketLadioDeux
{
    /// <summary>
    /// ヘッドラインから番組の情報を取得中に例外が発生したときに発生するイベントのEventArgs
    /// </summary>
    public class FetchChannelsAsyncExceptionEventArgs : EventArgs
    {
        /// <summary>
        /// 例外が発生したヘッドライン
        /// </summary>
        HeadlineBase headline;

        /// <summary>
        /// 例外が発生したヘッドラインを取得する
        /// </summary>
        public HeadlineBase Headline
        {
            get { return headline; }
        }

        /// <summary>
        /// 例外
        /// </summary>
        Exception exception;

        /// <summary>
        /// 例外を取得する
        /// </summary>
        public Exception Exception
        {
            get { return exception; }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="headline">例外が発生したヘッドライン</param>
        /// <param name="exception">例外</param>
        public FetchChannelsAsyncExceptionEventArgs(HeadlineBase headline, Exception exception)
        {
            this.headline = headline;
            this.exception = exception;
        }
    }
}
