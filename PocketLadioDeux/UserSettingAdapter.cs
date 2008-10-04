using System;

using System.IO;
using System.Xml.Serialization;

namespace PocketLadioDeux
{
    /// <summary>
    /// PocketLadioの設定にアクセスするクラス。
    /// UserSettingクラスへのアクセスと、シリアライズ・デシリアライズを担当している。
    /// </summary>
    public static class UserSettingAdapter
    {
        /// <summary>
        /// ユーザー設定
        /// </summary>
        private static UserSetting setting = new UserSetting();

        /// <summary>
        /// ユーザー設定を取得する
        /// </summary>
        public static UserSetting Setting
        {
            get { return UserSettingAdapter.setting; }
        }

        /// <summary>
        /// Settingが今回新しく作られたものか。
        /// Settingが今回新しく作られたものの場合はtrue、ファイルから読み込まれたものの場合はfalse。
        /// </summary>
        private static bool isSettingCreatedNew = true;

        /// <summary>
        /// Settingが今回新しく作られたものかを取得する。
        /// Settingが今回新しく作られたものの場合はtrue、ファイルから読み込まれたものの場合はfalse。
        /// </summary>
        public static bool IsSettingCreatedNew
        {
            get { return isSettingCreatedNew; }
        }

        /// <summary>
        /// 設定を保存する
        /// </summary>
        public static void Save()
        {
            FileStream fs = null;
            try
            {
                if (Directory.Exists(Path.GetDirectoryName(PocketLadioDeuxInfo.UserSettingFilePath)) == false)
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(PocketLadioDeuxInfo.UserSettingFilePath));
                }
                fs = new FileStream(PocketLadioDeuxInfo.UserSettingFilePath, FileMode.Create, FileAccess.Write);
                XmlSerializer sr = new XmlSerializer(typeof(UserSetting));
                // シリアル化して書き込む
                sr.Serialize(fs, setting);
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
        }

        /// <summary>
        /// 設定を読み込む
        /// </summary>
        public static void Load()
        {
            if (File.Exists(PocketLadioDeuxInfo.UserSettingFilePath) == true)
            {
                FileStream fs = null;
                try
                {
                    fs = new FileStream(PocketLadioDeuxInfo.UserSettingFilePath, FileMode.Open, FileAccess.Read);
                    XmlSerializer sr = new XmlSerializer(typeof(UserSetting));
                    // シリアルを読み込む
                    setting = sr.Deserialize(fs) as UserSetting;

                    isSettingCreatedNew = false;
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
            }

            // 設定が空の場合は、ここまででエラーが起こっているため
            // 新たに設定のインスタンスを作成する
            if (setting == null)
            {
                setting = new UserSetting();

                isSettingCreatedNew = true;
            }
        }
    }
}
