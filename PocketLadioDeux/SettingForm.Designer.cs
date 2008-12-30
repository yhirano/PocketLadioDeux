namespace PocketLadioDeux
{
    partial class SettingForm
    {
        /// <summary>
        /// 必要なデザイナ変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナで生成されたコード

        /// <summary>
        /// デザイナ サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディタで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingForm));
            this.baseTabControl = new System.Windows.Forms.TabControl();
            this.generalTabPage = new System.Windows.Forms.TabPage();
            this.headlineAutomaticUpdateIntervalLabel = new System.Windows.Forms.Label();
            this.hedlineAutomaticUpdatesIntervalNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.webBrowserPathReference = new System.Windows.Forms.Button();
            this.mediaPlayerPathReferenceButton = new System.Windows.Forms.Button();
            this.webBrowserPathTextBox2 = new OpenNETCF.Windows.Forms.TextBox2();
            this.webBrowserPathContextMenu = new System.Windows.Forms.ContextMenu();
            this.webBrowserPathCutMenuItem = new System.Windows.Forms.MenuItem();
            this.webBrowserPathCopyMenuItem = new System.Windows.Forms.MenuItem();
            this.webBrowserPathPasteMenuItem = new System.Windows.Forms.MenuItem();
            this.mediaPlayerPathTextBox2 = new OpenNETCF.Windows.Forms.TextBox2();
            this.mediaPlayerPathContextMenu = new System.Windows.Forms.ContextMenu();
            this.mediaPlayerPathCutMenuItem = new System.Windows.Forms.MenuItem();
            this.mediaPlayerPathCopyMenuItem = new System.Windows.Forms.MenuItem();
            this.mediaPlayerPathPasteMenuItem = new System.Windows.Forms.MenuItem();
            this.mediaPlayerPathLabel = new System.Windows.Forms.Label();
            this.webBrowserPathLabel = new System.Windows.Forms.Label();
            this.viewTabPage = new System.Windows.Forms.TabPage();
            this.headlineListFontSizeLabel = new System.Windows.Forms.Label();
            this.headlineListFontSizeDomainUpDown = new System.Windows.Forms.DomainUpDown();
            this.networkTabPage = new System.Windows.Forms.TabPage();
            this.proxySettingPanel = new System.Windows.Forms.Panel();
            this.manualProxySettingRadioButton = new System.Windows.Forms.RadioButton();
            this.proxyPortTextBox = new OpenNETCF.Windows.Forms.TextBox2();
            this.proxyPortContextMenu = new System.Windows.Forms.ContextMenu();
            this.proxyPortCutMenuItem = new System.Windows.Forms.MenuItem();
            this.proxyPortCopyMenuItem = new System.Windows.Forms.MenuItem();
            this.proxyPortPasteMenuItem = new System.Windows.Forms.MenuItem();
            this.autoDetectProxySettingRadioButton = new System.Windows.Forms.RadioButton();
            this.proxyServerTextBox = new OpenNETCF.Windows.Forms.TextBox2();
            this.proxyServerContextMenu = new System.Windows.Forms.ContextMenu();
            this.proxyServerCutMenuItem = new System.Windows.Forms.MenuItem();
            this.proxyServerCopyMenuItem = new System.Windows.Forms.MenuItem();
            this.proxyServerPasteMenuItem = new System.Windows.Forms.MenuItem();
            this.proxyNoUseRadioButton = new System.Windows.Forms.RadioButton();
            this.proxyPortLabel = new System.Windows.Forms.Label();
            this.proxyServerLabel = new System.Windows.Forms.Label();
            this.advancedTabPage = new System.Windows.Forms.TabPage();
            this.playlistSaveLabel = new System.Windows.Forms.Label();
            this.playlistSaveCheckBox = new System.Windows.Forms.CheckBox();
            this.mainMenu1 = new System.Windows.Forms.MainMenu();
            this.baseTabControl.SuspendLayout();
            this.generalTabPage.SuspendLayout();
            this.viewTabPage.SuspendLayout();
            this.networkTabPage.SuspendLayout();
            this.proxySettingPanel.SuspendLayout();
            this.advancedTabPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // baseTabControl
            // 
            resources.ApplyResources(this.baseTabControl, "baseTabControl");
            this.baseTabControl.Controls.Add(this.generalTabPage);
            this.baseTabControl.Controls.Add(this.viewTabPage);
            this.baseTabControl.Controls.Add(this.networkTabPage);
            this.baseTabControl.Controls.Add(this.advancedTabPage);
            this.baseTabControl.Name = "baseTabControl";
            this.baseTabControl.SelectedIndex = 0;
            // 
            // generalTabPage
            // 
            resources.ApplyResources(this.generalTabPage, "generalTabPage");
            this.generalTabPage.Controls.Add(this.headlineAutomaticUpdateIntervalLabel);
            this.generalTabPage.Controls.Add(this.hedlineAutomaticUpdatesIntervalNumericUpDown);
            this.generalTabPage.Controls.Add(this.webBrowserPathReference);
            this.generalTabPage.Controls.Add(this.mediaPlayerPathReferenceButton);
            this.generalTabPage.Controls.Add(this.webBrowserPathTextBox2);
            this.generalTabPage.Controls.Add(this.mediaPlayerPathTextBox2);
            this.generalTabPage.Controls.Add(this.mediaPlayerPathLabel);
            this.generalTabPage.Controls.Add(this.webBrowserPathLabel);
            this.generalTabPage.Name = "generalTabPage";
            // 
            // headlineAutomaticUpdateIntervalLabel
            // 
            resources.ApplyResources(this.headlineAutomaticUpdateIntervalLabel, "headlineAutomaticUpdateIntervalLabel");
            this.headlineAutomaticUpdateIntervalLabel.Name = "headlineAutomaticUpdateIntervalLabel";
            // 
            // hedlineAutomaticUpdatesIntervalNumericUpDown
            // 
            resources.ApplyResources(this.hedlineAutomaticUpdatesIntervalNumericUpDown, "hedlineAutomaticUpdatesIntervalNumericUpDown");
            this.hedlineAutomaticUpdatesIntervalNumericUpDown.Name = "hedlineAutomaticUpdatesIntervalNumericUpDown";
            this.hedlineAutomaticUpdatesIntervalNumericUpDown.Value = new decimal(new int[] {
            60,
            0,
            0,
            0});
            // 
            // webBrowserPathReference
            // 
            resources.ApplyResources(this.webBrowserPathReference, "webBrowserPathReference");
            this.webBrowserPathReference.Name = "webBrowserPathReference";
            this.webBrowserPathReference.Click += new System.EventHandler(this.webBrowserPathReference_Click);
            // 
            // mediaPlayerPathReferenceButton
            // 
            resources.ApplyResources(this.mediaPlayerPathReferenceButton, "mediaPlayerPathReferenceButton");
            this.mediaPlayerPathReferenceButton.Name = "mediaPlayerPathReferenceButton";
            this.mediaPlayerPathReferenceButton.Click += new System.EventHandler(this.mediaPlayerPathReferenceButton_Click);
            // 
            // webBrowserPathTextBox2
            // 
            resources.ApplyResources(this.webBrowserPathTextBox2, "webBrowserPathTextBox2");
            this.webBrowserPathTextBox2.ContextMenu = this.webBrowserPathContextMenu;
            this.webBrowserPathTextBox2.Name = "webBrowserPathTextBox2";
            // 
            // webBrowserPathContextMenu
            // 
            this.webBrowserPathContextMenu.MenuItems.Add(this.webBrowserPathCutMenuItem);
            this.webBrowserPathContextMenu.MenuItems.Add(this.webBrowserPathCopyMenuItem);
            this.webBrowserPathContextMenu.MenuItems.Add(this.webBrowserPathPasteMenuItem);
            // 
            // webBrowserPathCutMenuItem
            // 
            resources.ApplyResources(this.webBrowserPathCutMenuItem, "webBrowserPathCutMenuItem");
            this.webBrowserPathCutMenuItem.Click += new System.EventHandler(this.webBrowserPathCutMenuItem_Click);
            // 
            // webBrowserPathCopyMenuItem
            // 
            resources.ApplyResources(this.webBrowserPathCopyMenuItem, "webBrowserPathCopyMenuItem");
            this.webBrowserPathCopyMenuItem.Click += new System.EventHandler(this.webBrowserPathCopyMenuItem_Click);
            // 
            // webBrowserPathPasteMenuItem
            // 
            resources.ApplyResources(this.webBrowserPathPasteMenuItem, "webBrowserPathPasteMenuItem");
            this.webBrowserPathPasteMenuItem.Click += new System.EventHandler(this.webBrowserPathPasteMenuItem_Click);
            // 
            // mediaPlayerPathTextBox2
            // 
            resources.ApplyResources(this.mediaPlayerPathTextBox2, "mediaPlayerPathTextBox2");
            this.mediaPlayerPathTextBox2.ContextMenu = this.mediaPlayerPathContextMenu;
            this.mediaPlayerPathTextBox2.Name = "mediaPlayerPathTextBox2";
            // 
            // mediaPlayerPathContextMenu
            // 
            this.mediaPlayerPathContextMenu.MenuItems.Add(this.mediaPlayerPathCutMenuItem);
            this.mediaPlayerPathContextMenu.MenuItems.Add(this.mediaPlayerPathCopyMenuItem);
            this.mediaPlayerPathContextMenu.MenuItems.Add(this.mediaPlayerPathPasteMenuItem);
            // 
            // mediaPlayerPathCutMenuItem
            // 
            resources.ApplyResources(this.mediaPlayerPathCutMenuItem, "mediaPlayerPathCutMenuItem");
            this.mediaPlayerPathCutMenuItem.Click += new System.EventHandler(this.mediaPlayerPathCutMenuItem_Click);
            // 
            // mediaPlayerPathCopyMenuItem
            // 
            resources.ApplyResources(this.mediaPlayerPathCopyMenuItem, "mediaPlayerPathCopyMenuItem");
            this.mediaPlayerPathCopyMenuItem.Click += new System.EventHandler(this.mediaPlayerPathCopyMenuItem_Click);
            // 
            // mediaPlayerPathPasteMenuItem
            // 
            resources.ApplyResources(this.mediaPlayerPathPasteMenuItem, "mediaPlayerPathPasteMenuItem");
            this.mediaPlayerPathPasteMenuItem.Click += new System.EventHandler(this.mediaPlayerPathPasteMenuItem_Click);
            // 
            // mediaPlayerPathLabel
            // 
            resources.ApplyResources(this.mediaPlayerPathLabel, "mediaPlayerPathLabel");
            this.mediaPlayerPathLabel.Name = "mediaPlayerPathLabel";
            // 
            // webBrowserPathLabel
            // 
            resources.ApplyResources(this.webBrowserPathLabel, "webBrowserPathLabel");
            this.webBrowserPathLabel.Name = "webBrowserPathLabel";
            // 
            // viewTabPage
            // 
            resources.ApplyResources(this.viewTabPage, "viewTabPage");
            this.viewTabPage.Controls.Add(this.headlineListFontSizeLabel);
            this.viewTabPage.Controls.Add(this.headlineListFontSizeDomainUpDown);
            this.viewTabPage.Name = "viewTabPage";
            // 
            // headlineListFontSizeLabel
            // 
            resources.ApplyResources(this.headlineListFontSizeLabel, "headlineListFontSizeLabel");
            this.headlineListFontSizeLabel.Name = "headlineListFontSizeLabel";
            // 
            // headlineListFontSizeDomainUpDown
            // 
            resources.ApplyResources(this.headlineListFontSizeDomainUpDown, "headlineListFontSizeDomainUpDown");
            this.headlineListFontSizeDomainUpDown.Items.Add(resources.GetString("headlineListFontSizeDomainUpDown.Items"));
            this.headlineListFontSizeDomainUpDown.Items.Add(resources.GetString("headlineListFontSizeDomainUpDown.Items1"));
            this.headlineListFontSizeDomainUpDown.Items.Add(resources.GetString("headlineListFontSizeDomainUpDown.Items2"));
            this.headlineListFontSizeDomainUpDown.Items.Add(resources.GetString("headlineListFontSizeDomainUpDown.Items3"));
            this.headlineListFontSizeDomainUpDown.Items.Add(resources.GetString("headlineListFontSizeDomainUpDown.Items4"));
            this.headlineListFontSizeDomainUpDown.Items.Add(resources.GetString("headlineListFontSizeDomainUpDown.Items5"));
            this.headlineListFontSizeDomainUpDown.Items.Add(resources.GetString("headlineListFontSizeDomainUpDown.Items6"));
            this.headlineListFontSizeDomainUpDown.Items.Add(resources.GetString("headlineListFontSizeDomainUpDown.Items7"));
            this.headlineListFontSizeDomainUpDown.Items.Add(resources.GetString("headlineListFontSizeDomainUpDown.Items8"));
            this.headlineListFontSizeDomainUpDown.Items.Add(resources.GetString("headlineListFontSizeDomainUpDown.Items9"));
            this.headlineListFontSizeDomainUpDown.Items.Add(resources.GetString("headlineListFontSizeDomainUpDown.Items10"));
            this.headlineListFontSizeDomainUpDown.Items.Add(resources.GetString("headlineListFontSizeDomainUpDown.Items11"));
            this.headlineListFontSizeDomainUpDown.Items.Add(resources.GetString("headlineListFontSizeDomainUpDown.Items12"));
            this.headlineListFontSizeDomainUpDown.Items.Add(resources.GetString("headlineListFontSizeDomainUpDown.Items13"));
            this.headlineListFontSizeDomainUpDown.Items.Add(resources.GetString("headlineListFontSizeDomainUpDown.Items14"));
            this.headlineListFontSizeDomainUpDown.Items.Add(resources.GetString("headlineListFontSizeDomainUpDown.Items15"));
            this.headlineListFontSizeDomainUpDown.Name = "headlineListFontSizeDomainUpDown";
            this.headlineListFontSizeDomainUpDown.ReadOnly = true;
            // 
            // networkTabPage
            // 
            resources.ApplyResources(this.networkTabPage, "networkTabPage");
            this.networkTabPage.Controls.Add(this.proxySettingPanel);
            this.networkTabPage.Name = "networkTabPage";
            // 
            // proxySettingPanel
            // 
            resources.ApplyResources(this.proxySettingPanel, "proxySettingPanel");
            this.proxySettingPanel.Controls.Add(this.manualProxySettingRadioButton);
            this.proxySettingPanel.Controls.Add(this.proxyPortTextBox);
            this.proxySettingPanel.Controls.Add(this.autoDetectProxySettingRadioButton);
            this.proxySettingPanel.Controls.Add(this.proxyServerTextBox);
            this.proxySettingPanel.Controls.Add(this.proxyNoUseRadioButton);
            this.proxySettingPanel.Controls.Add(this.proxyPortLabel);
            this.proxySettingPanel.Controls.Add(this.proxyServerLabel);
            this.proxySettingPanel.Name = "proxySettingPanel";
            // 
            // manualProxySettingRadioButton
            // 
            resources.ApplyResources(this.manualProxySettingRadioButton, "manualProxySettingRadioButton");
            this.manualProxySettingRadioButton.Name = "manualProxySettingRadioButton";
            // 
            // proxyPortTextBox
            // 
            resources.ApplyResources(this.proxyPortTextBox, "proxyPortTextBox");
            this.proxyPortTextBox.ContextMenu = this.proxyPortContextMenu;
            this.proxyPortTextBox.Name = "proxyPortTextBox";
            // 
            // proxyPortContextMenu
            // 
            this.proxyPortContextMenu.MenuItems.Add(this.proxyPortCutMenuItem);
            this.proxyPortContextMenu.MenuItems.Add(this.proxyPortCopyMenuItem);
            this.proxyPortContextMenu.MenuItems.Add(this.proxyPortPasteMenuItem);
            // 
            // proxyPortCutMenuItem
            // 
            resources.ApplyResources(this.proxyPortCutMenuItem, "proxyPortCutMenuItem");
            this.proxyPortCutMenuItem.Click += new System.EventHandler(this.proxyPortCutMenuItem_Click);
            // 
            // proxyPortCopyMenuItem
            // 
            resources.ApplyResources(this.proxyPortCopyMenuItem, "proxyPortCopyMenuItem");
            this.proxyPortCopyMenuItem.Click += new System.EventHandler(this.proxyPortCopyMenuItem_Click);
            // 
            // proxyPortPasteMenuItem
            // 
            resources.ApplyResources(this.proxyPortPasteMenuItem, "proxyPortPasteMenuItem");
            this.proxyPortPasteMenuItem.Click += new System.EventHandler(this.proxyPortPasteMenuItem_Click);
            // 
            // autoDetectProxySettingRadioButton
            // 
            resources.ApplyResources(this.autoDetectProxySettingRadioButton, "autoDetectProxySettingRadioButton");
            this.autoDetectProxySettingRadioButton.Checked = true;
            this.autoDetectProxySettingRadioButton.Name = "autoDetectProxySettingRadioButton";
            // 
            // proxyServerTextBox
            // 
            resources.ApplyResources(this.proxyServerTextBox, "proxyServerTextBox");
            this.proxyServerTextBox.ContextMenu = this.proxyServerContextMenu;
            this.proxyServerTextBox.Name = "proxyServerTextBox";
            // 
            // proxyServerContextMenu
            // 
            this.proxyServerContextMenu.MenuItems.Add(this.proxyServerCutMenuItem);
            this.proxyServerContextMenu.MenuItems.Add(this.proxyServerCopyMenuItem);
            this.proxyServerContextMenu.MenuItems.Add(this.proxyServerPasteMenuItem);
            // 
            // proxyServerCutMenuItem
            // 
            resources.ApplyResources(this.proxyServerCutMenuItem, "proxyServerCutMenuItem");
            this.proxyServerCutMenuItem.Click += new System.EventHandler(this.proxyServerCutMenuItem_Click);
            // 
            // proxyServerCopyMenuItem
            // 
            resources.ApplyResources(this.proxyServerCopyMenuItem, "proxyServerCopyMenuItem");
            this.proxyServerCopyMenuItem.Click += new System.EventHandler(this.proxyServerCopyMenuItem_Click);
            // 
            // proxyServerPasteMenuItem
            // 
            resources.ApplyResources(this.proxyServerPasteMenuItem, "proxyServerPasteMenuItem");
            this.proxyServerPasteMenuItem.Click += new System.EventHandler(this.proxyServerPasteMenuItem_Click);
            // 
            // proxyNoUseRadioButton
            // 
            resources.ApplyResources(this.proxyNoUseRadioButton, "proxyNoUseRadioButton");
            this.proxyNoUseRadioButton.Name = "proxyNoUseRadioButton";
            // 
            // proxyPortLabel
            // 
            resources.ApplyResources(this.proxyPortLabel, "proxyPortLabel");
            this.proxyPortLabel.Name = "proxyPortLabel";
            // 
            // proxyServerLabel
            // 
            resources.ApplyResources(this.proxyServerLabel, "proxyServerLabel");
            this.proxyServerLabel.Name = "proxyServerLabel";
            // 
            // advancedTabPage
            // 
            resources.ApplyResources(this.advancedTabPage, "advancedTabPage");
            this.advancedTabPage.Controls.Add(this.playlistSaveLabel);
            this.advancedTabPage.Controls.Add(this.playlistSaveCheckBox);
            this.advancedTabPage.Name = "advancedTabPage";
            // 
            // playlistSaveLabel
            // 
            resources.ApplyResources(this.playlistSaveLabel, "playlistSaveLabel");
            this.playlistSaveLabel.Name = "playlistSaveLabel";
            // 
            // playlistSaveCheckBox
            // 
            resources.ApplyResources(this.playlistSaveCheckBox, "playlistSaveCheckBox");
            this.playlistSaveCheckBox.Name = "playlistSaveCheckBox";
            // 
            // SettingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.baseTabControl);
            this.KeyPreview = true;
            this.Menu = this.mainMenu1;
            this.Name = "SettingForm";
            this.Load += new System.EventHandler(this.SettingForm_Load);
            this.Closing += new System.ComponentModel.CancelEventHandler(this.SettingForm_Closing);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SettingForm_KeyDown);
            this.baseTabControl.ResumeLayout(false);
            this.generalTabPage.ResumeLayout(false);
            this.viewTabPage.ResumeLayout(false);
            this.networkTabPage.ResumeLayout(false);
            this.proxySettingPanel.ResumeLayout(false);
            this.advancedTabPage.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl baseTabControl;
        private System.Windows.Forms.TabPage generalTabPage;
        private System.Windows.Forms.TabPage networkTabPage;
        private System.Windows.Forms.Panel proxySettingPanel;
        private System.Windows.Forms.RadioButton manualProxySettingRadioButton;
        private OpenNETCF.Windows.Forms.TextBox2 proxyPortTextBox;
        private System.Windows.Forms.RadioButton autoDetectProxySettingRadioButton;
        private OpenNETCF.Windows.Forms.TextBox2 proxyServerTextBox;
        private System.Windows.Forms.RadioButton proxyNoUseRadioButton;
        private System.Windows.Forms.Label proxyPortLabel;
        private System.Windows.Forms.Label proxyServerLabel;
        private System.Windows.Forms.ContextMenu proxyServerContextMenu;
        private System.Windows.Forms.ContextMenu proxyPortContextMenu;
        private System.Windows.Forms.MenuItem proxyPortCutMenuItem;
        private System.Windows.Forms.MenuItem proxyPortCopyMenuItem;
        private System.Windows.Forms.MenuItem proxyPortPasteMenuItem;
        private System.Windows.Forms.MenuItem proxyServerCutMenuItem;
        private System.Windows.Forms.MenuItem proxyServerCopyMenuItem;
        private System.Windows.Forms.MenuItem proxyServerPasteMenuItem;
        private OpenNETCF.Windows.Forms.TextBox2 webBrowserPathTextBox2;
        private OpenNETCF.Windows.Forms.TextBox2 mediaPlayerPathTextBox2;
        private System.Windows.Forms.Label mediaPlayerPathLabel;
        private System.Windows.Forms.Label webBrowserPathLabel;
        private System.Windows.Forms.Button webBrowserPathReference;
        private System.Windows.Forms.Button mediaPlayerPathReferenceButton;
        private System.Windows.Forms.ContextMenu mediaPlayerPathContextMenu;
        private System.Windows.Forms.MenuItem mediaPlayerPathCutMenuItem;
        private System.Windows.Forms.MenuItem mediaPlayerPathCopyMenuItem;
        private System.Windows.Forms.MenuItem mediaPlayerPathPasteMenuItem;
        private System.Windows.Forms.ContextMenu webBrowserPathContextMenu;
        private System.Windows.Forms.MenuItem webBrowserPathCutMenuItem;
        private System.Windows.Forms.MenuItem webBrowserPathCopyMenuItem;
        private System.Windows.Forms.MenuItem webBrowserPathPasteMenuItem;
        private System.Windows.Forms.MainMenu mainMenu1;
        private System.Windows.Forms.TabPage viewTabPage;
        private System.Windows.Forms.Label headlineListFontSizeLabel;
        private System.Windows.Forms.DomainUpDown headlineListFontSizeDomainUpDown;
        private System.Windows.Forms.TabPage advancedTabPage;
        private System.Windows.Forms.Label headlineAutomaticUpdateIntervalLabel;
        private System.Windows.Forms.NumericUpDown hedlineAutomaticUpdatesIntervalNumericUpDown;
        private System.Windows.Forms.Label playlistSaveLabel;
        private System.Windows.Forms.CheckBox playlistSaveCheckBox;
    }
}