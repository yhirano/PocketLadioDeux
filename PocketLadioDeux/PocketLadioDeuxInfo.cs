using System;

using System.Drawing;
using System.Reflection;
using System.IO;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;
using MiscPocketCompactLibrary2.Reflection;


namespace PocketLadioDeux
{
    /// <summary>
    /// PocketLadio::Deuxの固有情報を記述しているクラス
    /// </summary>
    internal static class PocketLadioDeuxInfo
    {
        #region アプリケーションの設定

        /// <summary>
        /// メディアプレーヤーのパスのデフォルト設定
        /// </summary>
        private const string DEFAULT_MEDIA_PLAYER_PATH = @"\Program Files\TCPMP\player.exe";

        /// <summary>
        /// メディアプレーヤーのパスのデフォルト設定
        /// </summary>
        public static string DefaultMediaPlayerPath
        {
            get { return DEFAULT_MEDIA_PLAYER_PATH; }
        }

        /// <summary>
        /// ブラウザのパスのデフォルト設定
        /// </summary>
        private const string DEFAULT_BROWSER_PATH = @"\Windows\iexplore.exe";

        /// <summary>
        /// ブラウザのパスのデフォルト設定
        /// </summary>
        public static string WebDefaultBrowserPath
        {
            get { return DEFAULT_BROWSER_PATH; }
        }

        /// <summary>
        /// Web接続時のタイムアウト時間を取得する
        /// </summary>
        public static int WebRequestTimeoutMillSec
        {
            get { return 20000; }
        }

        /// <summary>
        /// Headlineリストのデフォルトフォントサイズ
        /// </summary>
        public static int HeadlineListDefaultFontSize
        {
            get { return 9; }
        }

        /// <summary>
        /// アプリケーション設定ファイルのファイルパスを取得する
        /// </summary>
        public static string UserSettingFilePath
        {
            get
            {
                string title = Regex.Replace(AssemblyUtility.GetTitle((Assembly.GetExecutingAssembly())), "[\\\\/:,;*?\"<>|]", "_");
                return Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\" + title + @"\Setting.xml";
            }
        }

        /// <summary>
        /// ヘッドライン情報ファイルのファイルパスを取得する
        /// </summary>
        public static string HeadlineSettingFilePath
        {
            get
            {
                string title = Regex.Replace(AssemblyUtility.GetTitle(Assembly.GetExecutingAssembly()), "[\\\\/:,;*?\"<>|]", "_");
                return Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\" + title + @"\Headlines.xml";
            }
        }

        /// <summary>
        /// 個別のヘッドラインの情報を格納するディレクトリ
        /// </summary>
        public static string HeadlineSettingDirectoryPath
        {
            get
            {
                string title = Regex.Replace(AssemblyUtility.GetTitle(Assembly.GetExecutingAssembly()), "[\\\\/:,;*?\"<>|]", "_");
                return Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\" + title;
            }
        }

        /// <summary>
        /// ヘッドラインプラグインが格納されているディレクトリ
        /// </summary>
        public static string HeadlinePluginDirectoryPath
        {
            get
            {
                return Path.GetDirectoryName(AssemblyUtility.GetLocation(Assembly.GetExecutingAssembly()));
            }
        }

        /// <summary>
        /// ヘッドラインプラグインではないdll。ここで指定されたdllファイルはプラグインとして解析されません。
        /// </summary>
        private static readonly string[] excludeHeadlinePluginFiles = new string[] {
            "FileDialog.dll", "OpenNETCF.dll", "OpenNETCF.Windows.Forms.dll",
            "MiscPocketCompactLibrary2.dll", "HeadlinePluginInterface.dll"
        };

        /// <summary>
        /// ヘッドラインプラグインではないdll。ここで指定されたdllファイルはプラグインとして解析されません。
        /// </summary>
        public static string[] ExcludeHeadlinePluginFiles
        {
            get { return excludeHeadlinePluginFiles; }
        }

        /// <summary>
        /// 例外に出力するログファイルを取得する
        /// </summary>
        public static string ExceptionLogFilePath
        {
            get
            {
                return Path.GetDirectoryName(AssemblyUtility.GetLocation(Assembly.GetExecutingAssembly()))
                    + @"\" + "PocketLadioDeuxExceptionLog.log";
            }
        }

        /// <summary>
        /// プレイリストと見なす拡張子
        /// </summary>
        private static string[] playListExtensions = { ".m3u", ".pls" };

        /// <summary>
        /// プレイリストと見なす拡張子を取得する
        /// </summary>
        public static string[] PlayListExtensions
        {
            get { return playListExtensions; }
        }

        /// <summary>
        /// 番組がプレイリストだった場合に作成するファイル名（拡張子はつけない）
        /// </summary>
        public static string GeneratePlayListFileName
        {
            get
            {
                return Path.GetDirectoryName(AssemblyUtility.GetLocation(Assembly.GetExecutingAssembly()))
                  + @"\" + "PocketLadioDeux_playlist";
            }
        }

        #endregion
    }
}
