using System;

using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;
using System.Xml.Serialization;
using OpenNETCF.ComponentModel;
using MiscPocketCompactLibrary2.Net;
using MiscPocketCompactLibrary2.Reflection;
using PocketLadioDeux.HeadlinePluginInterface;

namespace PocketLadioDeux
{
    internal static class HeadlineManager
    {
        /// <summary>
        /// ヘッドライン
        /// </summary>
        private static List<HeadlineBase> headlines = new List<HeadlineBase>();

        /// <summary>
        /// ヘッドラインを取得する
        /// </summary>
        public static HeadlineBase[] Headlines
        {
            get { return headlines.ToArray(); }
        }

        /// <summary>
        /// ヘッドラインを追加する
        /// </summary>
        /// <param name="headline">ヘッドライン</param>
        public static void AddHeadline(HeadlineBase headline)
        {
            if (headline == null)
            {
                throw new ArgumentNullException();
            }

            headlines.Add(headline);

            // ヘッドラインの保存先を生成する
            headlineSettingFiles[headline] = new HeadlineSettingFilePathStore();
            headlineSettingFiles[headline].ClassName = headline.GetType().FullName;
            string filePath;
            Random rand = new Random(DateTime.Now.Second);
            do{
                filePath = rand.Next().ToString() + ".conf";
            } while (File.Exists(PocketLadioDeuxInfo.HeadlineSettingDirectoryPath + @"\" + filePath));
            headlineSettingFiles[headline].FilePath = filePath;
        }

        /// <summary>
        /// ヘッドラインを削除する
        /// </summary>
        /// <param name="headline">ヘッドライン</param>
        public static void RemoveHeadline(HeadlineBase headline)
        {
            if (headline == null)
            {
                throw new ArgumentNullException();
            }
            if (headlines.Exists(delegate(HeadlineBase v)
            {
                return v == headline;
            }) == false)
            {
                throw new ArgumentException();
            }

            headlines.Remove(headline);

            // ヘッドラインの保存先のファイルを削除する
            if (headlineSettingFiles.ContainsKey(headline) == true)
            {
                if (File.Exists(PocketLadioDeuxInfo.HeadlineSettingDirectoryPath + @"\" + headlineSettingFiles[headline].FilePath) == true)
                {
                    File.Delete(PocketLadioDeuxInfo.HeadlineSettingDirectoryPath + @"\" + headlineSettingFiles[headline].FilePath);
                }
                headlineSettingFiles.Remove(headline);
            }
        }

        /// <summary>
        /// ヘッドラインから番組の情報を非同期で取得する
        /// </summary>
        /// <param name="headline">ヘッドライン</param>
        public static void FetchChannelsAsync(HeadlineBase headline)
        {
            // 接続の設定
            HttpConnection connectionSetting = new HttpConnection();
            connectionSetting.Timeout = PocketLadioDeuxInfo.WebRequestTimeoutMillSec;
            connectionSetting.UserAgent = string.Format("{0}/{1}",
                AssemblyUtility.GetTitle(Assembly.GetExecutingAssembly()), AssemblyUtility.GetVersion(Assembly.GetExecutingAssembly()).ToString());
            connectionSetting.ProxySetting = UserSettingAdapter.Setting.ProxySetting;

            BackgroundWorker bg = new BackgroundWorker();
            bg.DoWork += new DoWorkEventHandler(delegate(object sender, DoWorkEventArgs e) { headline.FetchHeadlineA(connectionSetting); });
            bg.RunWorkerCompleted += new RunWorkerCompletedEventHandler(
                delegate(object sender, RunWorkerCompletedEventArgs e)
                {
                    if (e.Error != null)
                    {
                        OnFetchChannelsAsyncExceptionEvent(headline, e.Error);
                    }
                    else if (e.Cancelled == true)
                    {
                        OnFetchChannelsAsyncCancelEvent(headline);
                    }
                });
            bg.RunWorkerAsync();
        }

        /// <summary>
        /// ヘッドラインから番組の情報を取得中に例外が発生したときに発生するイベント
        /// </summary>
        public static event EventHandler<FetchChannelsAsyncExceptionEventArgs> FetchChannelsAsyncExceptionEventHandler;

        /// <summary>
        /// FetchChannelsAsyncExceptionEventHandlerの処理
        /// </summary>
        /// <param name="headline">例外が発生したヘッドライン</param>
        /// <param name="exception">例外</param>
        private static void OnFetchChannelsAsyncExceptionEvent(HeadlineBase headline, Exception exception)
        {
            if (FetchChannelsAsyncExceptionEventHandler != null)
            {
                FetchChannelsAsyncExceptionEventHandler(null, new FetchChannelsAsyncExceptionEventArgs(headline, exception));
            }
        }

        /// <summary>
        /// ヘッドラインから番組の情報を取得中にキャンセルされたときに発生するイベント
        /// </summary>
        public static event EventHandler<FetchChannelsAsyncCancelEventArgs> FetchChannelsAsyncCancelEventHandler;

        /// <summary>
        /// FetchChannelsAsyncCancelEventHandlerの処理
        /// </summary>
        /// <param name="headline">キャンセルされたヘッドライン</param>
        private static void OnFetchChannelsAsyncCancelEvent(HeadlineBase headline)
        {
            if (FetchChannelsAsyncCancelEventHandler != null)
            {
                FetchChannelsAsyncCancelEventHandler(null, new FetchChannelsAsyncCancelEventArgs(headline));
            }
        }

        /// <summary>
        /// 指定のヘッドラインの順番を上げる
        /// </summary>
        /// <param name="headline">ヘッドライン</param>
        public static void Up(HeadlineBase headline)
        {
            if (headline == null)
            {
                throw new ArgumentNullException();
            }
            if (headlines.Exists(delegate(HeadlineBase v)
            {
                return v == headline;
            }) == false)
            {
                throw new ArgumentException();
            }

            int index = headlines.FindIndex(delegate(HeadlineBase v)
            {
                return v == headline;
            });

            if (index > 0)
            {
                Swap(headlines, index, index - 1);
            }
        }

        /// <summary>
        /// 指定のヘッドラインの順番を下げる
        /// </summary>
        /// <param name="headline">ヘッドライン</param>
        public static void Down(HeadlineBase headline)
        {
            if (headline == null)
            {
                throw new ArgumentNullException();
            }
            if (headlines.Exists(delegate(HeadlineBase v)
            {
                return v == headline;
            }) == false)
            {
                throw new ArgumentException();
            }

            int index = headlines.FindIndex(delegate(HeadlineBase v)
            {
                return v == headline;
            });

            if (index != -1 && index < headlines.Count - 1)
            {
                Swap(headlines, index, index + 1);
            }
        }

        /// <summary>
        ///  スワップする
        /// </summary>
        /// <param name="list">リスト</param>
        /// <param name="x">スワップ位置1</param>
        /// <param name="y">スワップ位置2</param>
        private static void Swap(System.Collections.IList list, int x, int y)
        {
            object tmp;

            tmp = list[x];
            list[x] = list[y];
            list[y] = tmp;
        }

        /// <summary>
        /// ヘッドラインとヘッドラインの設定ファイルのセット
        /// </summary>
        private static Dictionary<HeadlineBase, HeadlineSettingFilePathStore> headlineSettingFiles = new Dictionary<HeadlineBase, HeadlineSettingFilePathStore>();

        /// <summary>
        /// ヘッドライン情報を書き込む
        /// </summary>
        public static void Save()
        {
            #region ヘッドライン情報ファイルの書き込み

            FileStream fs = null;
            try
            {
                if (Directory.Exists(Path.GetDirectoryName(PocketLadioDeuxInfo.HeadlineSettingFilePath)) == false)
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(PocketLadioDeuxInfo.HeadlineSettingFilePath));
                }
                fs = new FileStream(PocketLadioDeuxInfo.HeadlineSettingFilePath, FileMode.Create, FileAccess.Write);
                XmlSerializer sr = new XmlSerializer(typeof(HeadlineSettingFilePathStore[]));
                // シリアル化して書き込む
                List<HeadlineSettingFilePathStore> settingStores = new List<HeadlineSettingFilePathStore>();
                foreach (HeadlineBase headline in headlines)
                {
                    if (headlineSettingFiles.ContainsKey(headline) == true)
                    {
                        settingStores.Add(headlineSettingFiles[headline]);
                    }
                }
                sr.Serialize(fs, settingStores.ToArray());
                // 個別のヘッドラインの情報を保存する
                foreach (KeyValuePair<HeadlineBase, HeadlineSettingFilePathStore> pair in headlineSettingFiles)
                {
                    FileStream hfs = null;
                    try
                    {
                        hfs = new FileStream(PocketLadioDeuxInfo.HeadlineSettingDirectoryPath + @"\" + pair.Value.FilePath, FileMode.Create, FileAccess.Write);
                        pair.Key.Save(hfs);
                    }
#if !DEBUG
                    catch (InvalidOperationException) { ; }
                    catch (IOException) { ; }
#endif // !DEBUG
                    finally
                    {
                        if (hfs != null)
                        {
                            hfs.Close();
                        }
                    }
                }
            }
#if !DEBUG
            catch (InvalidOperationException) { ; }
            catch (IOException) { ; }
#endif // !DEBUG
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                }
            }

            #endregion // ヘッドライン情報ファイルの書き込み
        }

        /// <summary>
        /// ヘッドライン情報を読み込む
        /// </summary>
        public static void Load()
        {
            if (File.Exists(PocketLadioDeuxInfo.HeadlineSettingFilePath) == true)
            {
                #region ヘッドライン情報ファイルの読み込み

                FileStream fs = null;
                try
                {
                    fs = new FileStream(PocketLadioDeuxInfo.HeadlineSettingFilePath, FileMode.Open, FileAccess.Read);
                    XmlSerializer sr = new XmlSerializer(typeof(HeadlineSettingFilePathStore[]));
                    HeadlineSettingFilePathStore[] settingStores = sr.Deserialize(fs) as HeadlineSettingFilePathStore[];
                    // 個別のヘッドラインの情報を復元する
                    foreach (HeadlineSettingFilePathStore setting in settingStores)
                    {
                        if (File.Exists(PocketLadioDeuxInfo.HeadlineSettingDirectoryPath + @"\" + setting.FilePath))
                        {
                            // ヘッドラインのインスタンスを生成
                            HeadlineBase headline = null;
                            foreach (HeadlinePlugin plugin in HeadlinePluginManager.Plugins)
                            {
                                if (setting.ClassName == plugin.ClassName)
                                {
                                    headline = plugin.CreateInstance();
                                    break;
                                }
                            }
                            // ヘッドラインの読み込み
                            if (headline != null)
                            {
                                FileStream hfs = null;
                                try
                                {
                                    hfs = new FileStream(PocketLadioDeuxInfo.HeadlineSettingDirectoryPath + @"\" + setting.FilePath, FileMode.Open, FileAccess.Read);
                                    headline.Load(hfs);
                                }
#if !DEBUG
                                catch (IOException) { ; }
#endif // !DEBUG
                                finally
                                {
                                    if (hfs != null)
                                    {
                                        hfs.Close();
                                    }
                                }

                                headlineSettingFiles[headline] = setting;
                                headlines.Add(headline);
                            }
                        }
                    }
                }
#if !DEBUG
                catch (InvalidOperationException) { ; }
                catch (IOException) { ; }
#endif // !DEBUG
                finally
                {
                    if (fs != null)
                    {
                        fs.Close();
                    }
                }

                #endregion // ヘッドライン情報ファイルの読み込み
            }
        }
    }
}
