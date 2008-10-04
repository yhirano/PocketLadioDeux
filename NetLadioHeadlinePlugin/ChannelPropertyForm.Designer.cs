namespace PocketLadioDeux.NetLadioHeadlinePlugin
{
    partial class ChannelPropertyForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChannelPropertyForm));
            this.mainMenu1 = new System.Windows.Forms.MainMenu();
            this.propertyListView = new System.Windows.Forms.ListView();
            this.titleColumnHeader = new System.Windows.Forms.ColumnHeader();
            this.contentColumnHeader = new System.Windows.Forms.ColumnHeader();
            this.SuspendLayout();
            // 
            // propertyListView
            // 
            resources.ApplyResources(this.propertyListView, "propertyListView");
            this.propertyListView.Columns.Add(this.titleColumnHeader);
            this.propertyListView.Columns.Add(this.contentColumnHeader);
            this.propertyListView.Name = "propertyListView";
            this.propertyListView.View = System.Windows.Forms.View.Details;
            // 
            // titleColumnHeader
            // 
            resources.ApplyResources(this.titleColumnHeader, "titleColumnHeader");
            // 
            // contentColumnHeader
            // 
            resources.ApplyResources(this.contentColumnHeader, "contentColumnHeader");
            // 
            // ChannelPropertyForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.propertyListView);
            this.Menu = this.mainMenu1;
            this.Name = "ChannelPropertyForm";
            this.Load += new System.EventHandler(this.ChannelPropertyForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView propertyListView;
        private System.Windows.Forms.ColumnHeader titleColumnHeader;
        private System.Windows.Forms.ColumnHeader contentColumnHeader;
    }
}