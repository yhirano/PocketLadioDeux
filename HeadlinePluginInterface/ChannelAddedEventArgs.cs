using System;

namespace PocketLadioDeux.HeadlinePluginInterface
{
    /// <summary>
    /// 番組が追加された時に発生するイベントのEventArgs
    /// </summary>
    public class ChannelAddedEventArgs : EventArgs
    {
        /// <summary>
        /// 追加された番組
        /// </summary>
        private IChannel channel;

        /// <summary>
        /// 追加された番組を取得する
        /// </summary>
        public IChannel Channel
        {
            get { return channel; }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="channel">追加された番組</param>
        public ChannelAddedEventArgs(IChannel channel)
        {
            this.channel = channel;
        }
    }
}
