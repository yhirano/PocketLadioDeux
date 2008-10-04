using System;
using System.Xml.Serialization;
using MiscPocketCompactLibrary2.Net;

namespace PocketLadioDeux
{
    /// <summary>
    /// PocketLadio::Deuxの固有情報を記述しているクラス
    /// </summary>
    public class UserSetting
    {
        /// <summary>
        /// 音声再生用のメディアプレーヤーのファイルパス
        /// </summary>
        private string mediaPlayerPath = PocketLadioDeuxInfo.DefaultMediaPlayerPath;

        /// <summary>
        /// 音声再生用のメディアプレーヤーのファイルパスを取得・設定する
        /// </summary>
        public string MediaPlayerPath
        {
            get { return mediaPlayerPath; }
            set { mediaPlayerPath = value; }
        }

        /// <summary>
        /// Webブラウザのファイルパス
        /// </summary>
        private string webBrowserPath = PocketLadioDeuxInfo.WebDefaultBrowserPath;

        /// <summary>
        /// Webブラウザのファイルパスを取得・設定する
        /// </summary>
        public string WebBrowserPath
        {
            get { return webBrowserPath; }
            set { webBrowserPath = value; }
        }

        /// <summary>
        /// Headlineリストのフォントサイズ列挙
        /// </summary>
        public enum HeadlineListFontSizes
        {
            DefaultSize,
            Size6pt = 6,
            Size7pt = 7,
            Size8pt = 8,
            Size9pt = 9,
            Size10pt = 10,
            Size11pt = 11,
            Size12pt = 12,
            Size13pt = 13,
            Size14pt = 14,
            Size15pt = 15,
            Size16pt = 16,
            Size17pt = 17,
            Size18pt = 18,
            Size19pt = 19,
            Size20pt = 20
        }

        /// <summary>
        /// Headlineリストのフォントサイズ
        /// </summary>
        private HeadlineListFontSizes headlineListFontSize = HeadlineListFontSizes.DefaultSize;

        /// <summary>
        /// Headlineリストのフォントサイズを取得・設定する
        /// </summary>
        public HeadlineListFontSizes HeadlineListFontSize
        {
            get { return headlineListFontSize; }
            set { headlineListFontSize = value; }
        }

        /// <summary>
        /// プロキシ設定
        /// </summary>
        private WebProxySetting proxySetting = new WebProxySetting();

        /// <summary>
        /// プロキシ設定を取得・設定する
        /// </summary>
        public WebProxySetting ProxySetting
        {
            get { return proxySetting; }
            set { proxySetting = value; }
        }

        /// <summary>
        /// メインフォームのスプリッターの位置
        /// </summary>
        private int topPanelHeight = 180;

        /// <summary>
        /// メインフォームのスプリッターの位置を取得・設定する
        /// </summary>
        public int TopPanelHeight
        {
            get { return topPanelHeight; }
            set
            {
                if (topPanelHeight >= 0)
                {
                    topPanelHeight = value;
                }
                else { ; }
            }
        }

        /// <summary>
        /// メインフォームヘッドラインリストのチャンネル欄の幅
        /// </summary>
        private int headlineListViewChannelColumnWidth = 210;

        /// <summary>
        /// メインフォームヘッドラインリストのチャンネル欄の幅を取得・設定する
        /// </summary>
        public int HeadlineListViewChannelColumnWidth
        {
            get { return headlineListViewChannelColumnWidth; }
            set { headlineListViewChannelColumnWidth = value; }
        }

        /// <summary>
        /// フィルターが有効か
        /// </summary>
        private bool filterEnabled = false;

        /// <summary>
        /// フィルターが有効か
        /// </summary>
        public bool FilterEnabled
        {
            get { return filterEnabled; }
            set { filterEnabled = value; }
        }
    }
}
