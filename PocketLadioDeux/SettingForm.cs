using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;
using SmartPDA.Windows.Forms;
using MiscPocketCompactLibrary2.Net;

namespace PocketLadioDeux
{
    public partial class SettingForm : Form
    {
        public SettingForm()
        {
            InitializeComponent();
        }

        private void SettingForm_Load(object sender, EventArgs e)
        {
            #region 設定の読み込み

            mediaPlayerPathTextBox2.Text = UserSettingAdapter.Setting.MediaPlayerPath;
            webBrowserPathTextBox2.Text = UserSettingAdapter.Setting.WebBrowserPath;

            switch (UserSettingAdapter.Setting.HeadlineListFontSize)
            {
                case UserSetting.HeadlineListFontSizes.Size6pt:
                    headlineListFontSizeDomainUpDown.SelectedIndex = 1;
                    break;
                case UserSetting.HeadlineListFontSizes.Size7pt:
                    headlineListFontSizeDomainUpDown.SelectedIndex = 2;
                    break;
                case UserSetting.HeadlineListFontSizes.Size8pt:
                    headlineListFontSizeDomainUpDown.SelectedIndex = 3;
                    break;
                case UserSetting.HeadlineListFontSizes.Size9pt:
                    headlineListFontSizeDomainUpDown.SelectedIndex = 4;
                    break;
                case UserSetting.HeadlineListFontSizes.Size10pt:
                    headlineListFontSizeDomainUpDown.SelectedIndex = 5;
                    break;
                case UserSetting.HeadlineListFontSizes.Size11pt:
                    headlineListFontSizeDomainUpDown.SelectedIndex = 6;
                    break;
                case UserSetting.HeadlineListFontSizes.Size12pt:
                    headlineListFontSizeDomainUpDown.SelectedIndex = 7;
                    break;
                case UserSetting.HeadlineListFontSizes.Size13pt:
                    headlineListFontSizeDomainUpDown.SelectedIndex = 8;
                    break;
                case UserSetting.HeadlineListFontSizes.Size14pt:
                    headlineListFontSizeDomainUpDown.SelectedIndex = 9;
                    break;
                case UserSetting.HeadlineListFontSizes.Size15pt:
                    headlineListFontSizeDomainUpDown.SelectedIndex = 10;
                    break;
                case UserSetting.HeadlineListFontSizes.Size16pt:
                    headlineListFontSizeDomainUpDown.SelectedIndex = 11;
                    break;
                case UserSetting.HeadlineListFontSizes.Size17pt:
                    headlineListFontSizeDomainUpDown.SelectedIndex = 12;
                    break;
                case UserSetting.HeadlineListFontSizes.Size18pt:
                    headlineListFontSizeDomainUpDown.SelectedIndex = 13;
                    break;
                case UserSetting.HeadlineListFontSizes.Size19pt:
                    headlineListFontSizeDomainUpDown.SelectedIndex = 14;
                    break;
                case UserSetting.HeadlineListFontSizes.Size20pt:
                    headlineListFontSizeDomainUpDown.SelectedIndex = 15;
                    break;
                case UserSetting.HeadlineListFontSizes.DefaultSize:
                default:
                    headlineListFontSizeDomainUpDown.SelectedIndex = 0;
                    break;
            }

            switch (UserSettingAdapter.Setting.ProxySetting.ProxyUse)
            {
                case WebProxySetting.ProxyConnects.NoUse:
                    proxyNoUseRadioButton.Checked = true;
                    autoDetectProxySettingRadioButton.Checked = false;
                    manualProxySettingRadioButton.Checked = false;
                    break;
                case WebProxySetting.ProxyConnects.AutoDetect:
                    proxyNoUseRadioButton.Checked = false;
                    autoDetectProxySettingRadioButton.Checked = true;
                    manualProxySettingRadioButton.Checked = false;
                    break;
                case WebProxySetting.ProxyConnects.Manual:
                    proxyNoUseRadioButton.Checked = false;
                    autoDetectProxySettingRadioButton.Checked = false;
                    manualProxySettingRadioButton.Checked = true;
                    break;
                default:
                    // ここに到達することはあり得ない
                    Trace.Assert(false, "想定外の動作のため、終了します");
                    break;
            }

            proxyServerTextBox.Text = UserSettingAdapter.Setting.ProxySetting.ProxyServer;
            proxyPortTextBox.Text = UserSettingAdapter.Setting.ProxySetting.ProxyPort.ToString();

            #endregion
        }

        private void SettingForm_Closing(object sender, CancelEventArgs e)
        {
            #region 設定の書き込み

            UserSettingAdapter.Setting.MediaPlayerPath = mediaPlayerPathTextBox2.Text.Trim();
            UserSettingAdapter.Setting.WebBrowserPath = webBrowserPathTextBox2.Text.Trim();

            switch (headlineListFontSizeDomainUpDown.SelectedIndex)
            {
                case 1:
                    UserSettingAdapter.Setting.HeadlineListFontSize = UserSetting.HeadlineListFontSizes.Size6pt;
                    break;
                case 2:
                    UserSettingAdapter.Setting.HeadlineListFontSize = UserSetting.HeadlineListFontSizes.Size7pt;
                    break;
                case 3:
                    UserSettingAdapter.Setting.HeadlineListFontSize = UserSetting.HeadlineListFontSizes.Size8pt;
                    break;
                case 4:
                    UserSettingAdapter.Setting.HeadlineListFontSize = UserSetting.HeadlineListFontSizes.Size9pt;
                    break;
                case 5:
                    UserSettingAdapter.Setting.HeadlineListFontSize = UserSetting.HeadlineListFontSizes.Size10pt;
                    break;
                case 6:
                    UserSettingAdapter.Setting.HeadlineListFontSize = UserSetting.HeadlineListFontSizes.Size11pt;
                    break;
                case 7:
                    UserSettingAdapter.Setting.HeadlineListFontSize = UserSetting.HeadlineListFontSizes.Size12pt;
                    break;
                case 8:
                    UserSettingAdapter.Setting.HeadlineListFontSize = UserSetting.HeadlineListFontSizes.Size13pt;
                    break;
                case 9:
                    UserSettingAdapter.Setting.HeadlineListFontSize = UserSetting.HeadlineListFontSizes.Size14pt;
                    break;
                case 10:
                    UserSettingAdapter.Setting.HeadlineListFontSize = UserSetting.HeadlineListFontSizes.Size15pt;
                    break;
                case 11:
                    UserSettingAdapter.Setting.HeadlineListFontSize = UserSetting.HeadlineListFontSizes.Size16pt;
                    break;
                case 12:
                    UserSettingAdapter.Setting.HeadlineListFontSize = UserSetting.HeadlineListFontSizes.Size17pt;
                    break;
                case 13:
                    UserSettingAdapter.Setting.HeadlineListFontSize = UserSetting.HeadlineListFontSizes.Size18pt;
                    break;
                case 14:
                    UserSettingAdapter.Setting.HeadlineListFontSize = UserSetting.HeadlineListFontSizes.Size19pt;
                    break;
                case 15:
                    UserSettingAdapter.Setting.HeadlineListFontSize = UserSetting.HeadlineListFontSizes.Size20pt;
                    break;
                case 0:
                default:
                    UserSettingAdapter.Setting.HeadlineListFontSize = UserSetting.HeadlineListFontSizes.DefaultSize;
                    break;
            }


            if (proxyNoUseRadioButton.Checked == true)
            {
                UserSettingAdapter.Setting.ProxySetting.ProxyUse = WebProxySetting.ProxyConnects.NoUse;
            }
            else if (autoDetectProxySettingRadioButton.Checked == true)
            {
                UserSettingAdapter.Setting.ProxySetting.ProxyUse = WebProxySetting.ProxyConnects.AutoDetect;
            }
            else if (manualProxySettingRadioButton.Checked == true)
            {
                UserSettingAdapter.Setting.ProxySetting.ProxyUse = WebProxySetting.ProxyConnects.Manual;
            }
            else
            {
                // ここに到達することはあり得ない
                Trace.Assert(false, "想定外の動作のため、終了します");
            }
            UserSettingAdapter.Setting.ProxySetting.ProxyServer = proxyServerTextBox.Text.Trim();
            try
            {
                UserSettingAdapter.Setting.ProxySetting.ProxyPort = int.Parse(proxyPortTextBox.Text.Trim());
            }
            catch (ArgumentException) { ; }
            catch (FormatException) { ; }
            catch (OverflowException) { ; }

            try
            {
                UserSettingAdapter.Save();
            }
            catch (IOException)
            {
                MessageBox.Show("設定ファイルが書き込めませんでした", "設定ファイル書き込みエラー");
            }

            #endregion
        }

        private void proxyServerCutMenuItem_Click(object sender, EventArgs e)
        {
            proxyServerTextBox.Cut();
        }

        private void proxyServerCopyMenuItem_Click(object sender, EventArgs e)
        {
            proxyServerTextBox.Copy();
        }

        private void proxyServerPasteMenuItem_Click(object sender, EventArgs e)
        {
            proxyServerTextBox.Paste();
        }

        private void proxyPortCutMenuItem_Click(object sender, EventArgs e)
        {
            proxyPortTextBox.Cut();
        }

        private void proxyPortCopyMenuItem_Click(object sender, EventArgs e)
        {
            proxyPortTextBox.Copy();
        }

        private void proxyPortPasteMenuItem_Click(object sender, EventArgs e)
        {
            proxyPortTextBox.Paste();
        }

        private void mediaPlayerPathCutMenuItem_Click(object sender, EventArgs e)
        {
            mediaPlayerPathTextBox2.Cut();
        }

        private void mediaPlayerPathCopyMenuItem_Click(object sender, EventArgs e)
        {
            mediaPlayerPathTextBox2.Copy();
        }

        private void mediaPlayerPathPasteMenuItem_Click(object sender, EventArgs e)
        {
            mediaPlayerPathTextBox2.Paste();
        }

        private void webBrowserPathCutMenuItem_Click(object sender, EventArgs e)
        {
            webBrowserPathTextBox2.Cut();
        }

        private void webBrowserPathCopyMenuItem_Click(object sender, EventArgs e)
        {
            webBrowserPathTextBox2.Copy();
        }

        private void webBrowserPathPasteMenuItem_Click(object sender, EventArgs e)
        {
            webBrowserPathTextBox2.Paste();
        }

        private void mediaPlayerPathReferenceButton_Click(object sender, EventArgs e)
        {
            SmartPDA.Windows.Forms.OpenFileDialog fd = FileDialogFactory.MakeOpenFileDialog();
            if (Directory.Exists(Path.GetDirectoryName(mediaPlayerPathTextBox2.Text.Trim())))
            {
                fd.InitialDirectory = Path.GetDirectoryName(mediaPlayerPathTextBox2.Text.Trim());
            }
            fd.Filter = "*.exe|*.exe|*.*|*.*";
            fd.Activation = ItemActivation.OneClick;
            if (fd.ShowDialog() == DialogResult.OK)
            {
                mediaPlayerPathTextBox2.Text = fd.FileName;
            }
            fd.Dispose();
        }

        private void webBrowserPathReference_Click(object sender, EventArgs e)
        {
            SmartPDA.Windows.Forms.OpenFileDialog fd = FileDialogFactory.MakeOpenFileDialog();
            if (Directory.Exists(Path.GetDirectoryName(webBrowserPathTextBox2.Text.Trim())))
            {
                fd.InitialDirectory = Path.GetDirectoryName(webBrowserPathTextBox2.Text.Trim());
            }
            fd.Filter = "*.exe|*.exe|*.*|*.*";
            fd.Activation = ItemActivation.OneClick;
            if (fd.ShowDialog() == DialogResult.OK)
            {
                webBrowserPathTextBox2.Text = fd.FileName;
            }
            fd.Dispose();
        }
    }
}
