using System;

namespace PocketLadioDeux
{
    [Serializable()]
    public class HeadlineSettingFilePathStore
    {
        /// <summary>
        /// ヘッドラインのクラスパス
        /// </summary>
        private string className;

        /// <summary>
        /// ヘッドラインのクラスパスを取得する
        /// </summary>
        public string ClassName
        {
            get { return className; }
            set { className = value; }
        }

        /// <summary>
        /// ヘッドラインの保存先
        /// </summary>
        private string filePath;

        /// <summary>
        /// ヘッドラインの保存先を取得・設定する
        /// </summary>
        public string FilePath
        {
            get { return filePath; }
            set { filePath = value; }
        }
    }
}
