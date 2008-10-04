namespace PocketLadioDeux
{
    partial class HeadlinesForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HeadlinesForm));
            this.mainMenu1 = new System.Windows.Forms.MainMenu();
            this.nameLabel = new System.Windows.Forms.Label();
            this.kindComboBox = new System.Windows.Forms.ComboBox();
            this.addButton = new System.Windows.Forms.Button();
            this.headlinesLabel = new System.Windows.Forms.Label();
            this.headlineListViewContextMenu = new System.Windows.Forms.ContextMenu();
            this.upHeadlineMenuItem = new System.Windows.Forms.MenuItem();
            this.downHeadlineMenuItem = new System.Windows.Forms.MenuItem();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.settingMenuItem = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.removeHeadlineMenuItem = new System.Windows.Forms.MenuItem();
            this.removeButton = new System.Windows.Forms.Button();
            this.nameTextBox2 = new OpenNETCF.Windows.Forms.TextBox2();
            this.nameContextMenu = new System.Windows.Forms.ContextMenu();
            this.nameCutMenuItem = new System.Windows.Forms.MenuItem();
            this.nameCopyMenuItem = new System.Windows.Forms.MenuItem();
            this.namePasteMenuItem = new System.Windows.Forms.MenuItem();
            this.upButton = new System.Windows.Forms.Button();
            this.downButton = new System.Windows.Forms.Button();
            this.headlineListView = new System.Windows.Forms.ListView();
            this.headlineColumnHeader = new System.Windows.Forms.ColumnHeader();
            this.SuspendLayout();
            // 
            // nameLabel
            // 
            resources.ApplyResources(this.nameLabel, "nameLabel");
            this.nameLabel.Name = "nameLabel";
            // 
            // kindComboBox
            // 
            resources.ApplyResources(this.kindComboBox, "kindComboBox");
            this.kindComboBox.Name = "kindComboBox";
            // 
            // addButton
            // 
            resources.ApplyResources(this.addButton, "addButton");
            this.addButton.Name = "addButton";
            this.addButton.Click += new System.EventHandler(this.addButton_Click);
            // 
            // headlinesLabel
            // 
            resources.ApplyResources(this.headlinesLabel, "headlinesLabel");
            this.headlinesLabel.Name = "headlinesLabel";
            // 
            // headlineListViewContextMenu
            // 
            this.headlineListViewContextMenu.MenuItems.Add(this.upHeadlineMenuItem);
            this.headlineListViewContextMenu.MenuItems.Add(this.downHeadlineMenuItem);
            this.headlineListViewContextMenu.MenuItems.Add(this.menuItem1);
            this.headlineListViewContextMenu.MenuItems.Add(this.settingMenuItem);
            this.headlineListViewContextMenu.MenuItems.Add(this.menuItem2);
            this.headlineListViewContextMenu.MenuItems.Add(this.removeHeadlineMenuItem);
            // 
            // upHeadlineMenuItem
            // 
            resources.ApplyResources(this.upHeadlineMenuItem, "upHeadlineMenuItem");
            this.upHeadlineMenuItem.Click += new System.EventHandler(this.upHeadlineMenuItem_Click);
            // 
            // downHeadlineMenuItem
            // 
            resources.ApplyResources(this.downHeadlineMenuItem, "downHeadlineMenuItem");
            this.downHeadlineMenuItem.Click += new System.EventHandler(this.downHeadlineMenuItem_Click);
            // 
            // menuItem1
            // 
            resources.ApplyResources(this.menuItem1, "menuItem1");
            // 
            // settingMenuItem
            // 
            resources.ApplyResources(this.settingMenuItem, "settingMenuItem");
            this.settingMenuItem.Click += new System.EventHandler(this.settingMenuItem_Click);
            // 
            // menuItem2
            // 
            resources.ApplyResources(this.menuItem2, "menuItem2");
            // 
            // removeHeadlineMenuItem
            // 
            resources.ApplyResources(this.removeHeadlineMenuItem, "removeHeadlineMenuItem");
            this.removeHeadlineMenuItem.Click += new System.EventHandler(this.removeHeadlineMenuItem_Click);
            // 
            // removeButton
            // 
            resources.ApplyResources(this.removeButton, "removeButton");
            this.removeButton.Name = "removeButton";
            this.removeButton.Click += new System.EventHandler(this.removeButton_Click);
            // 
            // nameTextBox2
            // 
            resources.ApplyResources(this.nameTextBox2, "nameTextBox2");
            this.nameTextBox2.ContextMenu = this.nameContextMenu;
            this.nameTextBox2.Name = "nameTextBox2";
            // 
            // nameContextMenu
            // 
            this.nameContextMenu.MenuItems.Add(this.nameCutMenuItem);
            this.nameContextMenu.MenuItems.Add(this.nameCopyMenuItem);
            this.nameContextMenu.MenuItems.Add(this.namePasteMenuItem);
            // 
            // nameCutMenuItem
            // 
            resources.ApplyResources(this.nameCutMenuItem, "nameCutMenuItem");
            this.nameCutMenuItem.Click += new System.EventHandler(this.nameCutMenuItem_Click);
            // 
            // nameCopyMenuItem
            // 
            resources.ApplyResources(this.nameCopyMenuItem, "nameCopyMenuItem");
            this.nameCopyMenuItem.Click += new System.EventHandler(this.nameCopyMenuItem_Click);
            // 
            // namePasteMenuItem
            // 
            resources.ApplyResources(this.namePasteMenuItem, "namePasteMenuItem");
            this.namePasteMenuItem.Click += new System.EventHandler(this.namePasteMenuItem_Click);
            // 
            // upButton
            // 
            resources.ApplyResources(this.upButton, "upButton");
            this.upButton.Name = "upButton";
            this.upButton.Click += new System.EventHandler(this.upButton_Click);
            // 
            // downButton
            // 
            resources.ApplyResources(this.downButton, "downButton");
            this.downButton.Name = "downButton";
            this.downButton.Click += new System.EventHandler(this.downButton_Click);
            // 
            // headlineListView
            // 
            resources.ApplyResources(this.headlineListView, "headlineListView");
            this.headlineListView.Columns.Add(this.headlineColumnHeader);
            this.headlineListView.ContextMenu = this.headlineListViewContextMenu;
            this.headlineListView.Name = "headlineListView";
            this.headlineListView.View = System.Windows.Forms.View.Details;
            this.headlineListView.SelectedIndexChanged += new System.EventHandler(this.headlineListView_SelectedIndexChanged);
            // 
            // headlineColumnHeader
            // 
            resources.ApplyResources(this.headlineColumnHeader, "headlineColumnHeader");
            // 
            // HeadlinesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.headlineListView);
            this.Controls.Add(this.downButton);
            this.Controls.Add(this.upButton);
            this.Controls.Add(this.nameTextBox2);
            this.Controls.Add(this.removeButton);
            this.Controls.Add(this.headlinesLabel);
            this.Controls.Add(this.addButton);
            this.Controls.Add(this.kindComboBox);
            this.Controls.Add(this.nameLabel);
            this.Menu = this.mainMenu1;
            this.Name = "HeadlinesForm";
            this.Load += new System.EventHandler(this.HeadlinesForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label nameLabel;
        private System.Windows.Forms.ComboBox kindComboBox;
        private System.Windows.Forms.Button addButton;
        private System.Windows.Forms.Label headlinesLabel;
        private System.Windows.Forms.Button removeButton;
        private OpenNETCF.Windows.Forms.TextBox2 nameTextBox2;
        private System.Windows.Forms.Button upButton;
        private System.Windows.Forms.Button downButton;
        private System.Windows.Forms.ContextMenu nameContextMenu;
        private System.Windows.Forms.MenuItem nameCutMenuItem;
        private System.Windows.Forms.MenuItem nameCopyMenuItem;
        private System.Windows.Forms.MenuItem namePasteMenuItem;
        private System.Windows.Forms.ContextMenu headlineListViewContextMenu;
        private System.Windows.Forms.MenuItem upHeadlineMenuItem;
        private System.Windows.Forms.MenuItem downHeadlineMenuItem;
        private System.Windows.Forms.MenuItem menuItem1;
        private System.Windows.Forms.MenuItem settingMenuItem;
        private System.Windows.Forms.MenuItem menuItem2;
        private System.Windows.Forms.MenuItem removeHeadlineMenuItem;
        private System.Windows.Forms.ListView headlineListView;
        private System.Windows.Forms.ColumnHeader headlineColumnHeader;
    }
}