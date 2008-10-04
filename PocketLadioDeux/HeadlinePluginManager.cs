using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using MiscPocketCompactLibrary2.Reflection;
using PocketLadioDeux.HeadlinePluginInterface;

namespace PocketLadioDeux
{
    internal static class HeadlinePluginManager
    {
        /// <summary>
        /// 読み込んだプラグイン
        /// </summary>
        private static HeadlinePlugin[] plugins;

        /// <summary>
        /// 読み込んだプラグインを取得する
        /// </summary>
        internal static HeadlinePlugin[] Plugins
        {
            get { return plugins; }
        }

        /// <summary>
        /// 有効なプラグインを探す
        /// </summary>
        /// <returns>有効なプラグインのPluginInfo配列</returns>
        public static void FindPlugins()
        {
            // プラグインディレクトリが存在しない場合は終了
            if (Directory.Exists(PocketLadioDeuxInfo.HeadlinePluginDirectoryPath) == false)
            {
                return;
            }

            // 見つかったプラグイン
            List<HeadlinePlugin> plugins = new List<HeadlinePlugin>();

            // .dllファイルを探す
            string[] dlls = Directory.GetFiles(PocketLadioDeuxInfo.HeadlinePluginDirectoryPath, "*.dll");

            // プラグインとして解析しないファイル
            List<string> excludePluginFiles = new List<string>(PocketLadioDeuxInfo.ExcludeHeadlinePluginFiles);

            foreach (string dll in dlls)
            {
                #region プラグインとして解析しないファイルは飛ばす
                {
                    if (excludePluginFiles.Exists(
                        delegate(string v)
                        {
                            return Path.GetFileName(dll).ToLower() == v.ToLower();
                        }
                    ))
                    {
                        continue;
                    }
                }
                #endregion // プラグインとして解析しないファイルは飛ばす

                try
                {
                    // アセンブリとして読み込む
                    Assembly asm = Assembly.LoadFrom(dll);

                    foreach (Type type in asm.GetTypes())
                    {
                        // アセンブリ内のすべての型について、プラグインとして有効か調べる
                        if (type.IsClass == true && type.IsPublic == true && type.IsAbstract == false &&
                            type.BaseType == typeof(HeadlineBase))
                        {
                            // PluginInfoをコレクションに追加する
                            plugins.Add(new HeadlinePlugin(dll, type.FullName));
                        }
                    }
                }
                catch
                {
                    ;
                }
            }

            HeadlinePluginManager.plugins = plugins.ToArray();
        }
    }
}
