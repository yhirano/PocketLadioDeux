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
        /// 設定用ファイルパス生成用ランダムシード
        /// </summary>
        private static Random rand = new Random(DateTime.Now.Second);

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

            // 接続の設定
            headline.ConnectionSetting = connectionSetting;

            headlines.Add(headline);

            // ヘッドラインの保存先を生成する
            headlineSettingFiles[headline] = new HeadlineSettingFilePathStore();
            headlineSettingFiles[headline].ClassName = headline.GetType().FullName;
            string filePath;
            do
            {
                filePath = rand.Next().ToString() + ".conf";
            } while (File.Exists(PocketLadioDeuxInfo.HeadlineSettingDirectoryPath + @"\" + filePath) == true || IsExistHeadlineSettingFilePath(filePath) == true);
            headlineSettingFiles[headline].FilePath = filePath;
        }

        /// <summary>
        /// 指定したファイルパスに既に設定用ファイルパスが存在するかを調べる
        /// </summary>
        /// <param name="filePath">設定用ファイルパス</param>
        /// <returns>指定したファイルパスに既に設定用ファイルパスが存在する場合はtrue、それ以外はfalse</returns>
        private static bool IsExistHeadlineSettingFilePath(string filePath)
        {
            foreach (HeadlineSettingFilePathStore store in headlineSettingFiles.Values)
            {
                if (store.FilePath == filePath)
                {
                    return true;
                }
            }
            return false;
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
        /// ネットワークへの接続設定
        /// </summary>
        private static HttpConnection connectionSetting = CreateHttpConnection();

        /// <summary>
        /// ネットワークへの接続設定を取得する
        /// </summary>
        public static HttpConnection ConnectionSetting
        {
            get { return HeadlineManager.connectionSetting; }
        }

        private static HttpConnection CreateHttpConnection()
        {
            // 接続の設定
            HttpConnection connectionSetting = new HttpConnection();
            connectionSetting.Timeout = PocketLadioDeuxInfo.WebRequestTimeoutMillSec;
            connectionSetting.UserAgent = string.Format("{0}/{1}",
                AssemblyUtility.GetTitle(Assembly.GetExecutingAssembly()), AssemblyUtility.GetVersion(Assembly.GetExecutingAssembly()).ToString());
            connectionSetting.ProxySetting = UserSettingAdapter.Setting.ProxySetting;
            return connectionSetting;
        }

        /// <summary>
        /// 現在動作しているヘッドラインから番組を非同期で取得しているBackgroundWorkerのリスト
        /// </summary>
        private static Dictionary<HeadlineBase, BackgroundWorker> fetchingChannelBackgroundWorkers = new Dictionary<HeadlineBase, BackgroundWorker>();

        private static object fetchingChannelBackgroundWorkersLock = new object();

        /// <summary>
        /// ヘッドラインから番組の情報を非同期で取得する
        /// </summary>
        /// <param name="headline">ヘッドライン</param>
        public static void FetchChannelsAsync(HeadlineBase headline)
        {
            // 指定のヘッドラインが取得処理中の場合は、何もせず終了
            if (headline.IsFetching == true || fetchingChannelBackgroundWorkers.ContainsKey(headline) == true)
            {
                return;
            }

            BackgroundWorker bg = new BackgroundWorker();
            bg.WorkerSupportsCancellation = true;
            EventHandler<ChannelAddedEventArgs> cancelEventHandler = null;
            bg.DoWork += new DoWorkEventHandler(
                delegate(object sender, DoWorkEventArgs e)
                {
                    // 番組の取得をキャンセルするための処理
                    cancelEventHandler = new EventHandler<ChannelAddedEventArgs>(
                        delegate
                        {
                            if (bg.CancellationPending == true)
                            {
                                e.Cancel = true;
                                // 番組の取得をキャンセル
                                headline.FetchCancel();
                            }
                        });
                    headline.ChannelAddedEventHandler += cancelEventHandler;

                    // 番組の取得を開始
                    headline.FetchHeadlineA();
                });
            bg.RunWorkerCompleted += new RunWorkerCompletedEventHandler(
                delegate(object sender, RunWorkerCompletedEventArgs e)
                {
                    // ヘッドラインから番組取得キャンセルのためのイベントを削除
                    headline.ChannelAddedEventHandler -= cancelEventHandler;
                    // 非同期動作中のBackgroundWorkerリストからこのBackgroundWorkerを削除
                    lock (fetchingChannelBackgroundWorkersLock)
                    {
                        if (fetchingChannelBackgroundWorkers.ContainsKey(headline) == true && fetchingChannelBackgroundWorkers[headline] == (BackgroundWorker)sender)
                        {
                            fetchingChannelBackgroundWorkers.Remove(headline);
                        }
                    }

                    if (e.Error != null)
                    {
                        OnFetchChannelsAsyncExceptionEvent(headline, e.Error);
                    }
                    else if (e.Cancelled == true)
                    {
                        OnFetchChannelsAsyncCancelEvent(headline);
                    }
                });

            // 非同期動作中のBackgroundWorkerリストにこのBackgroundWorkerを追加
            lock (fetchingChannelBackgroundWorkersLock)
            {
                fetchingChannelBackgroundWorkers.Add(headline, bg);
            }

            OnFetchChannelsAsync(headline);

            bg.RunWorkerAsync();

            OnFetchedChannelsAsync(headline);
        }

        /// <summary>
        /// ヘッドラインの番組の取得前に発生するイベント
        /// </summary>
        public static event EventHandler<HeadlineEventArgs> FetchChannelsAsyncEventHandler;

        /// <summary>
        /// FetchChannelsAsyncEventHandlerの処理
        /// </summary>
        /// <param name="headline">ヘッドライン</param>
        private static void OnFetchChannelsAsync(HeadlineBase headline)
        {
            if (FetchChannelsAsyncEventHandler != null)
            {
                FetchChannelsAsyncEventHandler(null, new HeadlineEventArgs(headline));
            }
        }

        /// <summary>
        /// ヘッドラインの非同期取得処理開始後に発生するイベント
        /// </summary>
        public static event EventHandler<HeadlineEventArgs> FetchedChannelsAsyncEventHandler;

        /// <summary>
        /// FetchedChannelsAsyncEventHandlerの処理
        /// </summary>
        /// <param name="headline">ヘッドライン</param>
        private static void OnFetchedChannelsAsync(HeadlineBase headline)
        {
            if (FetchedChannelsAsyncEventHandler != null)
            {
                FetchedChannelsAsyncEventHandler(null, new HeadlineEventArgs(headline));
            }
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
        public static event EventHandler<HeadlineEventArgs> FetchChannelsAsyncCancelEventHandler;

        /// <summary>
        /// FetchChannelsAsyncCancelEventHandlerの処理
        /// </summary>
        /// <param name="headline">キャンセルされたヘッドライン</param>
        private static void OnFetchChannelsAsyncCancelEvent(HeadlineBase headline)
        {
            if (FetchChannelsAsyncCancelEventHandler != null)
            {
                FetchChannelsAsyncCancelEventHandler(null, new HeadlineEventArgs(headline));
            }
        }

        /// <summary>
        /// 指定の非同期で動作している番組取得処理をキャンセルする
        /// </summary>
        /// <param name="headline"></param>
        public static void CancelFetchChannelsAsync(HeadlineBase headline)
        {
            lock (fetchingChannelBackgroundWorkersLock)
            {
                if (fetchingChannelBackgroundWorkers.ContainsKey(headline) == true)
                {
                    fetchingChannelBackgroundWorkers[headline].CancelAsync();
                }
            }
        }

        /// <summary>
        /// 非同期で動作している番組取得処理すべてをキャンセルする
        /// </summary>
        public static void CancelFetchChannelsAsync()
        {
            lock (fetchingChannelBackgroundWorkersLock)
            {
                foreach (KeyValuePair<HeadlineBase, BackgroundWorker> pair in fetchingChannelBackgroundWorkers)
                {
                    pair.Value.CancelAsync();
                }
            }
        }

        /// <summary>
        /// 非同期で動作している番組取得処理が存在するかを取得する
        /// </summary>
        /// <returns>非同期で動作している番組取得処理が存在する場合はtrue、それ以外はfalse</returns>
        public static bool IsExistBusyFetchChannelAsync()
        {
            return fetchingChannelBackgroundWorkers.Count != 0;
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

                                // 接続の設定
                                headline.ConnectionSetting = connectionSetting;

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
