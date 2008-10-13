namespace PocketLadioDeux.ShoutCastHeadlinePlugin
{
    partial class SettingForm
    {
        /// <summary>
        /// 必要なデザイナ変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.MainMenu mainMenu1;

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
            this.mainMenu1 = new System.Windows.Forms.MainMenu();
            this.baseTabControl = new System.Windows.Forms.TabControl();
            this.generalTabPage = new System.Windows.Forms.TabPage();
            this.displayFormatTextBox2 = new OpenNETCF.Windows.Forms.TextBox2();
            this.displayFormatContextMenu = new System.Windows.Forms.ContextMenu();
            this.cutDisplayFormatMenuItem = new System.Windows.Forms.MenuItem();
            this.copyDisplayFormatMenuItem = new System.Windows.Forms.MenuItem();
            this.pasteDisplayFormatMenuItem = new System.Windows.Forms.MenuItem();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.insertMenuItem = new System.Windows.Forms.MenuItem();
            this.titleFormatMenuItem = new System.Windows.Forms.MenuItem();
            this.playingFormatMenuItem = new System.Windows.Forms.MenuItem();
            this.listenerFormatMenuItem = new System.Windows.Forms.MenuItem();
            this.genreFormatMenuItem = new System.Windows.Forms.MenuItem();
            this.bitFormatMenuItem = new System.Windows.Forms.MenuItem();
            this.displayFormatLabel = new System.Windows.Forms.Label();
            this.nameTextBox2 = new OpenNETCF.Windows.Forms.TextBox2();
            this.nameContextMenu = new System.Windows.Forms.ContextMenu();
            this.cutNameMenuItem = new System.Windows.Forms.MenuItem();
            this.copyNameMenuItem = new System.Windows.Forms.MenuItem();
            this.pasteNameMenuItem = new System.Windows.Forms.MenuItem();
            this.nameLabel = new System.Windows.Forms.Label();
            this.shoutCastTabPage = new System.Windows.Forms.TabPage();
            this.searchWordTextBox2 = new OpenNETCF.Windows.Forms.TextBox2();
            this.searchWordLabel = new System.Windows.Forms.Label();
            this.filter1TabPage = new System.Windows.Forms.TabPage();
            this.removeButton = new System.Windows.Forms.Button();
            this.filterListView = new System.Windows.Forms.ListView();
            this.wordColumnHeader = new System.Windows.Forms.ColumnHeader();
            this.typeColumnHeader = new System.Windows.Forms.ColumnHeader();
            this.filterListContextMenu = new System.Windows.Forms.ContextMenu();
            this.removeMenuItem = new System.Windows.Forms.MenuItem();
            this.filterListLabel = new System.Windows.Forms.Label();
            this.excludeButton = new System.Windows.Forms.Button();
            this.includeButton = new System.Windows.Forms.Button();
            this.addFilterTextBox2 = new OpenNETCF.Windows.Forms.TextBox2();
            this.addFilterContextMenu = new System.Windows.Forms.ContextMenu();
            this.cutAddFilterMenuItem = new System.Windows.Forms.MenuItem();
            this.copyAddFilterMenuItem = new System.Windows.Forms.MenuItem();
            this.pasteAddFilterMenuItem = new System.Windows.Forms.MenuItem();
            this.addFilterLabel = new System.Windows.Forms.Label();
            this.filter2TabPage = new System.Windows.Forms.TabPage();
            this.sortScendingPanel = new System.Windows.Forms.Panel();
            this.sortDescendingRadioButton = new System.Windows.Forms.RadioButton();
            this.sortAscendingRadioButton = new System.Windows.Forms.RadioButton();
            this.sortLabel = new System.Windows.Forms.Label();
            this.sortComboBox = new System.Windows.Forms.ComboBox();
            this.belowKbpsLabel = new System.Windows.Forms.Label();
            this.aboveKbpsLabel = new System.Windows.Forms.Label();
            this.belowBitrateNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.aboveBitrateNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.filteringBelowBitrateCheckBox = new System.Windows.Forms.CheckBox();
            this.filteringAboveBitrateCheckBox = new System.Windows.Forms.CheckBox();
            this.searchWordContextMenu = new System.Windows.Forms.ContextMenu();
            this.cutSearchWordMenuItem = new System.Windows.Forms.MenuItem();
            this.copySearchWordMenuItem = new System.Windows.Forms.MenuItem();
            this.pasteSearchWordMenuItem = new System.Windows.Forms.MenuItem();
            this.baseTabControl.SuspendLayout();
            this.generalTabPage.SuspendLayout();
            this.shoutCastTabPage.SuspendLayout();
            this.filter1TabPage.SuspendLayout();
            this.filter2TabPage.SuspendLayout();
            this.sortScendingPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // baseTabControl
            // 
            resources.ApplyResources(this.baseTabControl, "baseTabControl");
            this.baseTabControl.Controls.Add(this.generalTabPage);
            this.baseTabControl.Controls.Add(this.shoutCastTabPage);
            this.baseTabControl.Controls.Add(this.filter1TabPage);
            this.baseTabControl.Controls.Add(this.filter2TabPage);
            this.baseTabControl.Name = "baseTabControl";
            this.baseTabControl.SelectedIndex = 0;
            // 
            // generalTabPage
            // 
            resources.ApplyResources(this.generalTabPage, "generalTabPage");
            this.generalTabPage.Controls.Add(this.displayFormatTextBox2);
            this.generalTabPage.Controls.Add(this.displayFormatLabel);
            this.generalTabPage.Controls.Add(this.nameTextBox2);
            this.generalTabPage.Controls.Add(this.nameLabel);
            this.generalTabPage.Name = "generalTabPage";
            // 
            // displayFormatTextBox2
            // 
            resources.ApplyResources(this.displayFormatTextBox2, "displayFormatTextBox2");
            this.displayFormatTextBox2.ContextMenu = this.displayFormatContextMenu;
            this.displayFormatTextBox2.Name = "displayFormatTextBox2";
            // 
            // displayFormatContextMenu
            // 
            this.displayFormatContextMenu.MenuItems.Add(this.cutDisplayFormatMenuItem);
            this.displayFormatContextMenu.MenuItems.Add(this.copyDisplayFormatMenuItem);
            this.displayFormatContextMenu.MenuItems.Add(this.pasteDisplayFormatMenuItem);
            this.displayFormatContextMenu.MenuItems.Add(this.menuItem1);
            this.displayFormatContextMenu.MenuItems.Add(this.insertMenuItem);
            // 
            // cutDisplayFormatMenuItem
            // 
            resources.ApplyResources(this.cutDisplayFormatMenuItem, "cutDisplayFormatMenuItem");
            this.cutDisplayFormatMenuItem.Click += new System.EventHandler(this.cutDisplayFormatMenuItem_Click);
            // 
            // copyDisplayFormatMenuItem
            // 
            resources.ApplyResources(this.copyDisplayFormatMenuItem, "copyDisplayFormatMenuItem");
            this.copyDisplayFormatMenuItem.Click += new System.EventHandler(this.copyDisplayFormatMenuItem_Click);
            // 
            // pasteDisplayFormatMenuItem
            // 
            resources.ApplyResources(this.pasteDisplayFormatMenuItem, "pasteDisplayFormatMenuItem");
            this.pasteDisplayFormatMenuItem.Click += new System.EventHandler(this.pasteDisplayFormatMenuItem_Click);
            // 
            // menuItem1
            // 
            resources.ApplyResources(this.menuItem1, "menuItem1");
            // 
            // insertMenuItem
            // 
            resources.ApplyResources(this.insertMenuItem, "insertMenuItem");
            this.insertMenuItem.MenuItems.Add(this.titleFormatMenuItem);
            this.insertMenuItem.MenuItems.Add(this.playingFormatMenuItem);
            this.insertMenuItem.MenuItems.Add(this.listenerFormatMenuItem);
            this.insertMenuItem.MenuItems.Add(this.genreFormatMenuItem);
            this.insertMenuItem.MenuItems.Add(this.bitFormatMenuItem);
            // 
            // titleFormatMenuItem
            // 
            resources.ApplyResources(this.titleFormatMenuItem, "titleFormatMenuItem");
            this.titleFormatMenuItem.Click += new System.EventHandler(this.titleFormatMenuItem_Click);
            // 
            // playingFormatMenuItem
            // 
            resources.ApplyResources(this.playingFormatMenuItem, "playingFormatMenuItem");
            this.playingFormatMenuItem.Click += new System.EventHandler(this.playingFormatMenuItem_Click);
            // 
            // listenerFormatMenuItem
            // 
            resources.ApplyResources(this.listenerFormatMenuItem, "listenerFormatMenuItem");
            this.listenerFormatMenuItem.Click += new System.EventHandler(this.listenerFormatMenuItem_Click);
            // 
            // genreFormatMenuItem
            // 
            resources.ApplyResources(this.genreFormatMenuItem, "genreFormatMenuItem");
            this.genreFormatMenuItem.Click += new System.EventHandler(this.genreFormatMenuItem_Click);
            // 
            // bitFormatMenuItem
            // 
            resources.ApplyResources(this.bitFormatMenuItem, "bitFormatMenuItem");
            this.bitFormatMenuItem.Click += new System.EventHandler(this.bitFormatMenuItem_Click);
            // 
            // displayFormatLabel
            // 
            resources.ApplyResources(this.displayFormatLabel, "displayFormatLabel");
            this.displayFormatLabel.Name = "displayFormatLabel";
            // 
            // nameTextBox2
            // 
            resources.ApplyResources(this.nameTextBox2, "nameTextBox2");
            this.nameTextBox2.ContextMenu = this.nameContextMenu;
            this.nameTextBox2.Name = "nameTextBox2";
            // 
            // nameContextMenu
            // 
            this.nameContextMenu.MenuItems.Add(this.cutNameMenuItem);
            this.nameContextMenu.MenuItems.Add(this.copyNameMenuItem);
            this.nameContextMenu.MenuItems.Add(this.pasteNameMenuItem);
            // 
            // cutNameMenuItem
            // 
            resources.ApplyResources(this.cutNameMenuItem, "cutNameMenuItem");
            this.cutNameMenuItem.Click += new System.EventHandler(this.cutNameMenuItem_Click);
            // 
            // copyNameMenuItem
            // 
            resources.ApplyResources(this.copyNameMenuItem, "copyNameMenuItem");
            this.copyNameMenuItem.Click += new System.EventHandler(this.copyNameMenuItem_Click);
            // 
            // pasteNameMenuItem
            // 
            resources.ApplyResources(this.pasteNameMenuItem, "pasteNameMenuItem");
            this.pasteNameMenuItem.Click += new System.EventHandler(this.pasteNameMenuItem_Click);
            // 
            // nameLabel
            // 
            resources.ApplyResources(this.nameLabel, "nameLabel");
            this.nameLabel.Name = "nameLabel";
            // 
            // shoutCastTabPage
            // 
            resources.ApplyResources(this.shoutCastTabPage, "shoutCastTabPage");
            this.shoutCastTabPage.Controls.Add(this.searchWordTextBox2);
            this.shoutCastTabPage.Controls.Add(this.searchWordLabel);
            this.shoutCastTabPage.Name = "shoutCastTabPage";
            // 
            // searchWordTextBox2
            // 
            resources.ApplyResources(this.searchWordTextBox2, "searchWordTextBox2");
            this.searchWordTextBox2.ContextMenu = this.nameContextMenu;
            this.searchWordTextBox2.Name = "searchWordTextBox2";
            // 
            // searchWordLabel
            // 
            resources.ApplyResources(this.searchWordLabel, "searchWordLabel");
            this.searchWordLabel.Name = "searchWordLabel";
            // 
            // filter1TabPage
            // 
            resources.ApplyResources(this.filter1TabPage, "filter1TabPage");
            this.filter1TabPage.Controls.Add(this.removeButton);
            this.filter1TabPage.Controls.Add(this.filterListView);
            this.filter1TabPage.Controls.Add(this.filterListLabel);
            this.filter1TabPage.Controls.Add(this.excludeButton);
            this.filter1TabPage.Controls.Add(this.includeButton);
            this.filter1TabPage.Controls.Add(this.addFilterTextBox2);
            this.filter1TabPage.Controls.Add(this.addFilterLabel);
            this.filter1TabPage.Name = "filter1TabPage";
            // 
            // removeButton
            // 
            resources.ApplyResources(this.removeButton, "removeButton");
            this.removeButton.Name = "removeButton";
            this.removeButton.Click += new System.EventHandler(this.removeButton_Click);
            // 
            // filterListView
            // 
            resources.ApplyResources(this.filterListView, "filterListView");
            this.filterListView.Columns.Add(this.wordColumnHeader);
            this.filterListView.Columns.Add(this.typeColumnHeader);
            this.filterListView.ContextMenu = this.filterListContextMenu;
            this.filterListView.Name = "filterListView";
            this.filterListView.View = System.Windows.Forms.View.Details;
            this.filterListView.SelectedIndexChanged += new System.EventHandler(this.filterListView_SelectedIndexChanged);
            // 
            // wordColumnHeader
            // 
            resources.ApplyResources(this.wordColumnHeader, "wordColumnHeader");
            // 
            // typeColumnHeader
            // 
            resources.ApplyResources(this.typeColumnHeader, "typeColumnHeader");
            // 
            // filterListContextMenu
            // 
            this.filterListContextMenu.MenuItems.Add(this.removeMenuItem);
            // 
            // removeMenuItem
            // 
            resources.ApplyResources(this.removeMenuItem, "removeMenuItem");
            this.removeMenuItem.Popup += new System.EventHandler(this.removeMenuItem_Popup);
            this.removeMenuItem.Click += new System.EventHandler(this.removeMenuItem_Click);
            // 
            // filterListLabel
            // 
            resources.ApplyResources(this.filterListLabel, "filterListLabel");
            this.filterListLabel.Name = "filterListLabel";
            // 
            // excludeButton
            // 
            resources.ApplyResources(this.excludeButton, "excludeButton");
            this.excludeButton.Name = "excludeButton";
            this.excludeButton.Click += new System.EventHandler(this.excludeButton_Click);
            // 
            // includeButton
            // 
            resources.ApplyResources(this.includeButton, "includeButton");
            this.includeButton.Name = "includeButton";
            this.includeButton.Click += new System.EventHandler(this.includeButton_Click);
            // 
            // addFilterTextBox2
            // 
            resources.ApplyResources(this.addFilterTextBox2, "addFilterTextBox2");
            this.addFilterTextBox2.ContextMenu = this.addFilterContextMenu;
            this.addFilterTextBox2.Name = "addFilterTextBox2";
            this.addFilterTextBox2.TextChanged += new System.EventHandler(this.addFilterTextBox2_TextChanged);
            // 
            // addFilterContextMenu
            // 
            this.addFilterContextMenu.MenuItems.Add(this.cutAddFilterMenuItem);
            this.addFilterContextMenu.MenuItems.Add(this.copyAddFilterMenuItem);
            this.addFilterContextMenu.MenuItems.Add(this.pasteAddFilterMenuItem);
            // 
            // cutAddFilterMenuItem
            // 
            resources.ApplyResources(this.cutAddFilterMenuItem, "cutAddFilterMenuItem");
            this.cutAddFilterMenuItem.Click += new System.EventHandler(this.cutAddFilterMenuItem_Click);
            // 
            // copyAddFilterMenuItem
            // 
            resources.ApplyResources(this.copyAddFilterMenuItem, "copyAddFilterMenuItem");
            this.copyAddFilterMenuItem.Click += new System.EventHandler(this.copyAddFilterMenuItem_Click);
            // 
            // pasteAddFilterMenuItem
            // 
            resources.ApplyResources(this.pasteAddFilterMenuItem, "pasteAddFilterMenuItem");
            this.pasteAddFilterMenuItem.Click += new System.EventHandler(this.pasteAddFilterMenuItem_Click);
            // 
            // addFilterLabel
            // 
            resources.ApplyResources(this.addFilterLabel, "addFilterLabel");
            this.addFilterLabel.Name = "addFilterLabel";
            // 
            // filter2TabPage
            // 
            resources.ApplyResources(this.filter2TabPage, "filter2TabPage");
            this.filter2TabPage.Controls.Add(this.sortScendingPanel);
            this.filter2TabPage.Controls.Add(this.sortLabel);
            this.filter2TabPage.Controls.Add(this.sortComboBox);
            this.filter2TabPage.Controls.Add(this.belowKbpsLabel);
            this.filter2TabPage.Controls.Add(this.aboveKbpsLabel);
            this.filter2TabPage.Controls.Add(this.belowBitrateNumericUpDown);
            this.filter2TabPage.Controls.Add(this.aboveBitrateNumericUpDown);
            this.filter2TabPage.Controls.Add(this.filteringBelowBitrateCheckBox);
            this.filter2TabPage.Controls.Add(this.filteringAboveBitrateCheckBox);
            this.filter2TabPage.Name = "filter2TabPage";
            // 
            // sortScendingPanel
            // 
            resources.ApplyResources(this.sortScendingPanel, "sortScendingPanel");
            this.sortScendingPanel.Controls.Add(this.sortDescendingRadioButton);
            this.sortScendingPanel.Controls.Add(this.sortAscendingRadioButton);
            this.sortScendingPanel.Name = "sortScendingPanel";
            // 
            // sortDescendingRadioButton
            // 
            resources.ApplyResources(this.sortDescendingRadioButton, "sortDescendingRadioButton");
            this.sortDescendingRadioButton.Name = "sortDescendingRadioButton";
            // 
            // sortAscendingRadioButton
            // 
            resources.ApplyResources(this.sortAscendingRadioButton, "sortAscendingRadioButton");
            this.sortAscendingRadioButton.Checked = true;
            this.sortAscendingRadioButton.Name = "sortAscendingRadioButton";
            // 
            // sortLabel
            // 
            resources.ApplyResources(this.sortLabel, "sortLabel");
            this.sortLabel.Name = "sortLabel";
            // 
            // sortComboBox
            // 
            resources.ApplyResources(this.sortComboBox, "sortComboBox");
            this.sortComboBox.Items.Add(resources.GetString("sortComboBox.Items"));
            this.sortComboBox.Items.Add(resources.GetString("sortComboBox.Items1"));
            this.sortComboBox.Items.Add(resources.GetString("sortComboBox.Items2"));
            this.sortComboBox.Items.Add(resources.GetString("sortComboBox.Items3"));
            this.sortComboBox.Name = "sortComboBox";
            // 
            // belowKbpsLabel
            // 
            resources.ApplyResources(this.belowKbpsLabel, "belowKbpsLabel");
            this.belowKbpsLabel.Name = "belowKbpsLabel";
            // 
            // aboveKbpsLabel
            // 
            resources.ApplyResources(this.aboveKbpsLabel, "aboveKbpsLabel");
            this.aboveKbpsLabel.Name = "aboveKbpsLabel";
            // 
            // belowBitrateNumericUpDown
            // 
            resources.ApplyResources(this.belowBitrateNumericUpDown, "belowBitrateNumericUpDown");
            this.belowBitrateNumericUpDown.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.belowBitrateNumericUpDown.Name = "belowBitrateNumericUpDown";
            // 
            // aboveBitrateNumericUpDown
            // 
            resources.ApplyResources(this.aboveBitrateNumericUpDown, "aboveBitrateNumericUpDown");
            this.aboveBitrateNumericUpDown.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.aboveBitrateNumericUpDown.Name = "aboveBitrateNumericUpDown";
            // 
            // filteringBelowBitrateCheckBox
            // 
            resources.ApplyResources(this.filteringBelowBitrateCheckBox, "filteringBelowBitrateCheckBox");
            this.filteringBelowBitrateCheckBox.Name = "filteringBelowBitrateCheckBox";
            // 
            // filteringAboveBitrateCheckBox
            // 
            resources.ApplyResources(this.filteringAboveBitrateCheckBox, "filteringAboveBitrateCheckBox");
            this.filteringAboveBitrateCheckBox.Name = "filteringAboveBitrateCheckBox";
            // 
            // searchWordContextMenu
            // 
            this.searchWordContextMenu.MenuItems.Add(this.cutSearchWordMenuItem);
            this.searchWordContextMenu.MenuItems.Add(this.copySearchWordMenuItem);
            this.searchWordContextMenu.MenuItems.Add(this.pasteSearchWordMenuItem);
            // 
            // cutSearchWordMenuItem
            // 
            resources.ApplyResources(this.cutSearchWordMenuItem, "cutSearchWordMenuItem");
            this.cutSearchWordMenuItem.Click += new System.EventHandler(this.cutSearchWordMenuItem_Click);
            // 
            // copySearchWordMenuItem
            // 
            resources.ApplyResources(this.copySearchWordMenuItem, "copySearchWordMenuItem");
            this.copySearchWordMenuItem.Click += new System.EventHandler(this.copySearchWordMenuItem_Click);
            // 
            // pasteSearchWordMenuItem
            // 
            resources.ApplyResources(this.pasteSearchWordMenuItem, "pasteSearchWordMenuItem");
            this.pasteSearchWordMenuItem.Click += new System.EventHandler(this.pasteSearchWordMenuItem_Click);
            // 
            // SettingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.baseTabControl);
            this.Menu = this.mainMenu1;
            this.Name = "SettingForm";
            this.Load += new System.EventHandler(this.SettingForm_Load);
            this.Closing += new System.ComponentModel.CancelEventHandler(this.SettingForm_Closing);
            this.baseTabControl.ResumeLayout(false);
            this.generalTabPage.ResumeLayout(false);
            this.shoutCastTabPage.ResumeLayout(false);
            this.filter1TabPage.ResumeLayout(false);
            this.filter2TabPage.ResumeLayout(false);
            this.sortScendingPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl baseTabControl;
        private System.Windows.Forms.TabPage generalTabPage;
        private System.Windows.Forms.TabPage filter1TabPage;
        private System.Windows.Forms.TabPage filter2TabPage;
        private System.Windows.Forms.Label nameLabel;
        private OpenNETCF.Windows.Forms.TextBox2 displayFormatTextBox2;
        private System.Windows.Forms.Label displayFormatLabel;
        private OpenNETCF.Windows.Forms.TextBox2 nameTextBox2;
        private OpenNETCF.Windows.Forms.TextBox2 addFilterTextBox2;
        private System.Windows.Forms.Label addFilterLabel;
        private System.Windows.Forms.Button removeButton;
        private System.Windows.Forms.ListView filterListView;
        private System.Windows.Forms.ColumnHeader typeColumnHeader;
        private System.Windows.Forms.ColumnHeader wordColumnHeader;
        private System.Windows.Forms.Label filterListLabel;
        private System.Windows.Forms.Button excludeButton;
        private System.Windows.Forms.Button includeButton;
        private System.Windows.Forms.NumericUpDown belowBitrateNumericUpDown;
        private System.Windows.Forms.NumericUpDown aboveBitrateNumericUpDown;
        private System.Windows.Forms.CheckBox filteringBelowBitrateCheckBox;
        private System.Windows.Forms.CheckBox filteringAboveBitrateCheckBox;
        private System.Windows.Forms.Label sortLabel;
        private System.Windows.Forms.ComboBox sortComboBox;
        private System.Windows.Forms.Label belowKbpsLabel;
        private System.Windows.Forms.Label aboveKbpsLabel;
        private System.Windows.Forms.Panel sortScendingPanel;
        private System.Windows.Forms.RadioButton sortDescendingRadioButton;
        private System.Windows.Forms.RadioButton sortAscendingRadioButton;
        private System.Windows.Forms.ContextMenu nameContextMenu;
        private System.Windows.Forms.MenuItem cutNameMenuItem;
        private System.Windows.Forms.MenuItem copyNameMenuItem;
        private System.Windows.Forms.MenuItem pasteNameMenuItem;
        private System.Windows.Forms.ContextMenu addFilterContextMenu;
        private System.Windows.Forms.MenuItem cutAddFilterMenuItem;
        private System.Windows.Forms.MenuItem copyAddFilterMenuItem;
        private System.Windows.Forms.MenuItem pasteAddFilterMenuItem;
        private System.Windows.Forms.ContextMenu displayFormatContextMenu;
        private System.Windows.Forms.MenuItem cutDisplayFormatMenuItem;
        private System.Windows.Forms.MenuItem copyDisplayFormatMenuItem;
        private System.Windows.Forms.MenuItem pasteDisplayFormatMenuItem;
        private System.Windows.Forms.MenuItem insertMenuItem;
        private System.Windows.Forms.MenuItem playingFormatMenuItem;
        private System.Windows.Forms.MenuItem genreFormatMenuItem;
        private System.Windows.Forms.MenuItem listenerFormatMenuItem;
        private System.Windows.Forms.MenuItem titleFormatMenuItem;
        private System.Windows.Forms.MenuItem bitFormatMenuItem;
        private System.Windows.Forms.ContextMenu filterListContextMenu;
        private System.Windows.Forms.MenuItem removeMenuItem;
        private System.Windows.Forms.MenuItem menuItem1;
        private System.Windows.Forms.TabPage shoutCastTabPage;
        private OpenNETCF.Windows.Forms.TextBox2 searchWordTextBox2;
        private System.Windows.Forms.Label searchWordLabel;
        private System.Windows.Forms.ContextMenu searchWordContextMenu;
        private System.Windows.Forms.MenuItem cutSearchWordMenuItem;
        private System.Windows.Forms.MenuItem copySearchWordMenuItem;
        private System.Windows.Forms.MenuItem pasteSearchWordMenuItem;
    }
}