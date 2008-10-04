using System;
using System.Reflection;
using PocketLadioDeux.HeadlinePluginInterface;

namespace PocketLadioDeux
{
    internal class HeadlinePlugin
    {
        /// <summary>
        /// アセンブリファイルのパス
        /// </summary>
        private readonly string location;

        /// <summary>
        /// アセンブリファイルのパスを取得する
        /// </summary>
        public string Location
        {
            get { return location; }
        }

        /// <summary>
        /// クラスの名前を取得する
        /// </summary>
        private readonly string className;

        /// <summary>
        /// クラスの名前
        /// </summary>
        public string ClassName
        {
            get { return className; }
        }

        /// <summary>
        /// ヘッドラインの種類
        /// </summary>
        private readonly string kind;

        /// <summary>
        /// ヘッドラインの種類
        /// </summary>
        public string Kind
        {
            get { return kind; }
        } 

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="location">アセンブリファイルのパス</param>
        /// <param name="className">クラスの名前</param>
        public HeadlinePlugin(string location, string className)
        {
            this.location = location;
            this.className = className;
            this.kind = CreateInstance().Kind;
        }

        /// <summary>
        /// プラグインクラスのインスタンスを作成する
        /// </summary>
        /// <returns>プラグインクラスのインスタンス</returns>
        public HeadlineBase CreateInstance()
        {
            try
            {
                //アセンブリを読み込む
                Assembly asm = Assembly.LoadFrom(this.Location);
                // インスタンスの生成
                return (HeadlineBase)asm.CreateInstance(className);
            }
            catch
            {
                return null;
            }
        }
    }
}
