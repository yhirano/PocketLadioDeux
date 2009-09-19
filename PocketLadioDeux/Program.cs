using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Windows.Forms;
using MiscPocketCompactLibrary2.Diagnostics;
using MiscPocketCompactLibrary2.Reflection;

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
            // エラーハンドラ登録
            System.Threading.Thread.GetDomain().UnhandledException += delegate(object sender, UnhandledExceptionEventArgs e)
            {
                Exception ex = e.ExceptionObject as Exception;
                SaveExceptionLog(ex);
            };

            try
            {
                // プラグインを検索する
                HeadlinePluginManager.FindPlugins();

                // 設定を読み込む
                UserSettingAdapter.Load();

                // ヘッドラインを読み込む
                HeadlineManager.Load();

                Application.Run(new MainForm());

                #region 番組取得のスレッドを終了する

                // ヘッドライン取得処理の中止
                HeadlineManager.CancelFetchChannelsAsync();

                // スレッドが中止されるまで待つ
                // 1秒待ってスレッドが中止されないようなら、スレッドの中止を待たずにここを抜ける
                for (int i = 0; HeadlineManager.IsExistBusyFetchChannelAsync() == true && i < 10; ++i)
                {
                    System.Threading.Thread.Sleep(100);
                }

                #endregion // 番組取得のスレッドを終了する
            }
            catch (Exception ex)
            {
                SaveExceptionLog(ex);

                // 例外の再送
                throw ex;
            }
            finally
            {
                // ヘッドラインを保存する
                HeadlineManager.Save();

                // 設定を保存する
                UserSettingAdapter.Save();
            }
        }

        /// <summary>
        /// 例外ログを書き出す
        /// </summary>
        /// <param name="ex">例外</param>
        static void SaveExceptionLog(Exception ex)
        {
            Log exceptionLog = new Log(PocketLadioDeuxInfo.ExceptionLogFilePath);
            StringBuilder error = new StringBuilder();

            Assembly entryAsm = OpenNETCF.Reflection.Assembly2.GetEntryAssembly();
            error.Append("Application:        " +
                AssemblyUtility.GetTitle(entryAsm) + " " + AssemblyUtility.GetVersion(entryAsm).ToString() + "\r\n");
            error.Append("Date:               " + System.DateTime.Now.ToString("G") + "\r\n");
            error.Append("OS:                 " + Environment.OSVersion.ToString() + "\r\n");
            error.Append("Culture:            " + System.Globalization.CultureInfo.CurrentCulture.Name + "\r\n");
            if (ex != null)
            {
                error.Append("Exception class:    " + ex.GetType().ToString() + "\r\n");
                error.Append("Exception ToString: " + ex.ToString() + "\r\n");
                error.Append("Exception message:  " + "\r\n");
                error.Append(ex.Message);
                error.Append("Exception stack     : " + "\r\n");
                error.Append(ex.StackTrace);
            }
            error.Append("\r\n");
            error.Append("\r\n");

            exceptionLog.LogThis(error.ToString(), Log.LogPrefix.date);
        }
    }
}