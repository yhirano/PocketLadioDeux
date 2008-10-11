namespace PocketLadioDeux
{
    partial class MainForm
    {
        /// <summary>
        /// 必要なデザイナ変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.MainMenu mainMenu;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.mainMenu = new System.Windows.Forms.MainMenu();
            this.menuItem = new System.Windows.Forms.MenuItem();
            this.addRemoveHeadlineMenuItem = new System.Windows.Forms.MenuItem();
            this.headlineSettingMenuItem = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.settingMenuItem = new System.Windows.Forms.MenuItem();
            this.menuItem3 = new System.Windows.Forms.MenuItem();
            this.aboutMenuItem = new System.Windows.Forms.MenuItem();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.exitMenuItem = new System.Windows.Forms.MenuItem();
            this.topPanel = new System.Windows.Forms.Panel();
            this.cancelButton = new System.Windows.Forms.Button();
            this.headlineListView = new System.Windows.Forms.ListView();
            this.channelColumnHeader = new System.Windows.Forms.ColumnHeader();
            this.headlineListViewContextMenu = new System.Windows.Forms.ContextMenu();
            this.playMenuItem = new System.Windows.Forms.MenuItem();
            this.webMenuItem = new System.Windows.Forms.MenuItem();
            this.propertyMenuItem = new System.Windows.Forms.MenuItem();
            this.filterCheckBox = new System.Windows.Forms.CheckBox();
            this.headlineListComboBox = new System.Windows.Forms.ComboBox();
            this.updateButton = new System.Windows.Forms.Button();
            this.bottomPanel = new System.Windows.Forms.Panel();
            this.infomationLabel = new System.Windows.Forms.Label();
            this.propertyButton = new System.Windows.Forms.Button();
            this.webButton = new System.Windows.Forms.Button();
            this.playButton = new System.Windows.Forms.Button();
            this.mainSplitter = new System.Windows.Forms.Splitter();
            this.mainStatusBar = new System.Windows.Forms.StatusBar();
            this.topPanel.SuspendLayout();
            this.bottomPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainMenu
            // 
            this.mainMenu.MenuItems.Add(this.menuItem);
            // 
            // menuItem
            // 
            this.menuItem.MenuItems.Add(this.addRemoveHeadlineMenuItem);
            this.menuItem.MenuItems.Add(this.headlineSettingMenuItem);
            this.menuItem.MenuItems.Add(this.menuItem2);
            this.menuItem.MenuItems.Add(this.settingMenuItem);
            this.menuItem.MenuItems.Add(this.menuItem3);
            this.menuItem.MenuItems.Add(this.aboutMenuItem);
            this.menuItem.MenuItems.Add(this.menuItem1);
            this.menuItem.MenuItems.Add(this.exitMenuItem);
            resources.ApplyResources(this.menuItem, "menuItem");
            this.menuItem.Popup += new System.EventHandler(this.menuItem_Popup);
            // 
            // addRemoveHeadlineMenuItem
            // 
            resources.ApplyResources(this.addRemoveHeadlineMenuItem, "addRemoveHeadlineMenuItem");
            this.addRemoveHeadlineMenuItem.Click += new System.EventHandler(this.addRemoveHeadlineMenuItem_Click);
            // 
            // headlineSettingMenuItem
            // 
            resources.ApplyResources(this.headlineSettingMenuItem, "headlineSettingMenuItem");
            // 
            // menuItem2
            // 
            resources.ApplyResources(this.menuItem2, "menuItem2");
            // 
            // settingMenuItem
            // 
            resources.ApplyResources(this.settingMenuItem, "settingMenuItem");
            this.settingMenuItem.Click += new System.EventHandler(this.settingMenuItem_Click);
            // 
            // menuItem3
            // 
            resources.ApplyResources(this.menuItem3, "menuItem3");
            // 
            // aboutMenuItem
            // 
            resources.ApplyResources(this.aboutMenuItem, "aboutMenuItem");
            this.aboutMenuItem.Click += new System.EventHandler(this.aboutMenuItem_Click);
            // 
            // menuItem1
            // 
            resources.ApplyResources(this.menuItem1, "menuItem1");
            // 
            // exitMenuItem
            // 
            resources.ApplyResources(this.exitMenuItem, "exitMenuItem");
            this.exitMenuItem.Click += new System.EventHandler(this.exitMenuItem_Click);
            // 
            // topPanel
            // 
            this.topPanel.Controls.Add(this.cancelButton);
            this.topPanel.Controls.Add(this.headlineListView);
            this.topPanel.Controls.Add(this.filterCheckBox);
            this.topPanel.Controls.Add(this.headlineListComboBox);
            this.topPanel.Controls.Add(this.updateButton);
            resources.ApplyResources(this.topPanel, "topPanel");
            this.topPanel.Name = "topPanel";
            // 
            // cancelButton
            // 
            resources.ApplyResources(this.cancelButton, "cancelButton");
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // headlineListView
            // 
            resources.ApplyResources(this.headlineListView, "headlineListView");
            this.headlineListView.Columns.Add(this.channelColumnHeader);
            this.headlineListView.ContextMenu = this.headlineListViewContextMenu;
            this.headlineListView.Name = "headlineListView";
            this.headlineListView.View = System.Windows.Forms.View.Details;
            this.headlineListView.SelectedIndexChanged += new System.EventHandler(this.headlineListView_SelectedIndexChanged);
            this.headlineListView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.headlineListView_KeyDown);
            // 
            // channelColumnHeader
            // 
            resources.ApplyResources(this.channelColumnHeader, "channelColumnHeader");
            // 
            // headlineListViewContextMenu
            // 
            this.headlineListViewContextMenu.MenuItems.Add(this.playMenuItem);
            this.headlineListViewContextMenu.MenuItems.Add(this.webMenuItem);
            this.headlineListViewContextMenu.MenuItems.Add(this.propertyMenuItem);
            // 
            // playMenuItem
            // 
            resources.ApplyResources(this.playMenuItem, "playMenuItem");
            this.playMenuItem.Click += new System.EventHandler(this.playMenuItem_Click);
            // 
            // webMenuItem
            // 
            resources.ApplyResources(this.webMenuItem, "webMenuItem");
            this.webMenuItem.Click += new System.EventHandler(this.webMenuItem_Click);
            // 
            // propertyMenuItem
            // 
            resources.ApplyResources(this.propertyMenuItem, "propertyMenuItem");
            this.propertyMenuItem.Click += new System.EventHandler(this.propertyMenuItem_Click);
            // 
            // filterCheckBox
            // 
            resources.ApplyResources(this.filterCheckBox, "filterCheckBox");
            this.filterCheckBox.Name = "filterCheckBox";
            this.filterCheckBox.CheckStateChanged += new System.EventHandler(this.filterCheckBox_CheckStateChanged);
            // 
            // headlineListComboBox
            // 
            resources.ApplyResources(this.headlineListComboBox, "headlineListComboBox");
            this.headlineListComboBox.Name = "headlineListComboBox";
            this.headlineListComboBox.SelectedIndexChanged += new System.EventHandler(this.headlineListComboBox_SelectedIndexChanged);
            // 
            // updateButton
            // 
            resources.ApplyResources(this.updateButton, "updateButton");
            this.updateButton.Name = "updateButton";
            this.updateButton.Click += new System.EventHandler(this.updateButton_Click);
            // 
            // bottomPanel
            // 
            this.bottomPanel.Controls.Add(this.infomationLabel);
            this.bottomPanel.Controls.Add(this.propertyButton);
            this.bottomPanel.Controls.Add(this.webButton);
            this.bottomPanel.Controls.Add(this.playButton);
            resources.ApplyResources(this.bottomPanel, "bottomPanel");
            this.bottomPanel.Name = "bottomPanel";
            // 
            // infomationLabel
            // 
            resources.ApplyResources(this.infomationLabel, "infomationLabel");
            this.infomationLabel.Name = "infomationLabel";
            // 
            // propertyButton
            // 
            resources.ApplyResources(this.propertyButton, "propertyButton");
            this.propertyButton.Name = "propertyButton";
            this.propertyButton.Click += new System.EventHandler(this.propertyButton_Click);
            // 
            // webButton
            // 
            resources.ApplyResources(this.webButton, "webButton");
            this.webButton.Name = "webButton";
            this.webButton.Click += new System.EventHandler(this.webButton_Click);
            // 
            // playButton
            // 
            resources.ApplyResources(this.playButton, "playButton");
            this.playButton.Name = "playButton";
            this.playButton.Click += new System.EventHandler(this.playButton_Click);
            // 
            // mainSplitter
            // 
            resources.ApplyResources(this.mainSplitter, "mainSplitter");
            this.mainSplitter.Name = "mainSplitter";
            // 
            // mainStatusBar
            // 
            resources.ApplyResources(this.mainStatusBar, "mainStatusBar");
            this.mainStatusBar.Name = "mainStatusBar";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.mainSplitter);
            this.Controls.Add(this.bottomPanel);
            this.Controls.Add(this.topPanel);
            this.Controls.Add(this.mainStatusBar);
            this.KeyPreview = true;
            this.Menu = this.mainMenu;
            this.Name = "MainForm";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.MainForm_Paint);
            this.Activated += new System.EventHandler(this.MainForm_Activated);
            this.Closing += new System.ComponentModel.CancelEventHandler(this.MainForm_Closing);
            this.topPanel.ResumeLayout(false);
            this.bottomPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel topPanel;
        private System.Windows.Forms.Panel bottomPanel;
        private System.Windows.Forms.Splitter mainSplitter;
        private System.Windows.Forms.Label infomationLabel;
        private System.Windows.Forms.Button propertyButton;
        private System.Windows.Forms.Button webButton;
        private System.Windows.Forms.Button playButton;
        private System.Windows.Forms.ComboBox headlineListComboBox;
        private System.Windows.Forms.Button updateButton;
        private System.Windows.Forms.ListView headlineListView;
        private System.Windows.Forms.CheckBox filterCheckBox;
        private System.Windows.Forms.StatusBar mainStatusBar;
        private System.Windows.Forms.MenuItem menuItem;
        private System.Windows.Forms.MenuItem aboutMenuItem;
        private System.Windows.Forms.MenuItem menuItem1;
        private System.Windows.Forms.MenuItem exitMenuItem;
        private System.Windows.Forms.MenuItem settingMenuItem;
        private System.Windows.Forms.MenuItem menuItem3;
        private System.Windows.Forms.MenuItem addRemoveHeadlineMenuItem;
        private System.Windows.Forms.MenuItem menuItem2;
        private System.Windows.Forms.ColumnHeader channelColumnHeader;
        private System.Windows.Forms.MenuItem headlineSettingMenuItem;
        private System.Windows.Forms.ContextMenu headlineListViewContextMenu;
        private System.Windows.Forms.MenuItem playMenuItem;
        private System.Windows.Forms.MenuItem webMenuItem;
        private System.Windows.Forms.MenuItem propertyMenuItem;
        private System.Windows.Forms.Button cancelButton;
    }
}

