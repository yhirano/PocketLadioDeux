using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace PocketLadioDeux
{
    static class Program
    {
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [MTAThread]
        static void Main()
        {
            // プラグインを検索する
            HeadlinePluginManager.FindPlugins();

            // 設定を読み込む
            UserSettingAdapter.Load();

            // ヘッドラインを読み込む
            HeadlineManager.Load();

            Application.Run(new MainForm());

            // ヘッドラインを保存する
            HeadlineManager.Save();

            // 設定を保存する
            UserSettingAdapter.Save();
        }
    }
}