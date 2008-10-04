using System;

using System.Xml.Serialization;

namespace PocketLadioDeux.HeadlinePluginInterface
{
    /// <summary>
    /// 番組インターフェース
    /// </summary>
    public interface IChannel
    {
        /// <summary>
        /// 番組の放送URLを取得する
        /// </summary>
        Uri PlayUrl { get; }

        /// <summary>
        /// 番組のウェブサイトURLを取得する
        /// </summary>
        Uri WebSiteUrl { get; }

        /// <summary>
        /// 番組情報の表示文字列
        /// </summary>
        string Display { get; }

        /// <summary>
        /// フィルタリング対象のワードを取得する。
        /// 返されたワードに従い、フィルタリングを行う。
        /// </summary>
        string[] FilteredWords { get; }

        /// <summary>
        /// 番組の詳細フォームを表示する
        /// </summary>
        void ShowPropertyForm();
    }
}
