using System;

using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using PocketLadioDeux.HeadlinePluginInterface;

namespace PocketLadioDeux
{
    public partial class HeadlinesForm : Form
    {
        public HeadlinesForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 種類のコンボボックスの値クラス
        /// </summary>
        private class KindCombo
        {
            /// <summary>
            /// ヘッドラインプラグイン名
            /// </summary>
            private readonly string kind;

            /// <summary>
            /// ヘッドラインプラグイン名を取得する
            /// </summary>
            public string Kind
            {
                get { return kind; }
            }

            /// <summary>
            /// ヘッドラインプラグイン
            /// </summary>
            private readonly HeadlinePlugin plugin;

            /// <summary>
            /// ヘッドラインプラグインを取得する
            /// </summary>
            public HeadlinePlugin Plugin
            {
                get { return plugin; }
            }

            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="kind">ヘッドラインプラグイン名</param>
            /// <param name="plugin">ヘッドラインプラグイン</param>
            public KindCombo(string kind, HeadlinePlugin plugin)
            {
                this.kind = kind;
                this.plugin = plugin;
            }
        }

        /// <summary>
        /// HeadlineListViewの内容を更新する
        /// </summary>
        private void UpdateHeadlineListView()
        {
            headlineListView.BeginUpdate();

            headlineListView.Items.Clear();

            foreach (HeadlineBase headline in HeadlineManager.Headlines)
            {
                ListViewItem item = new ListViewItem();
                item.Text = string.Format("{0} - {1}", headline.Name, headline.Kind);
                item.Tag = headline;
                headlineListView.Items.Add(item);
            }

            // カラムの大きさを設定する
            headlineColumnHeader.Width = -2;

            headlineListView.EndUpdate();
        }

        private void HeadlinesForm_Load(object sender, EventArgs e)
        {
            #region kindComboBoxの初期化

            kindComboBox.DisplayMember = "Kind";
            kindComboBox.ValueMember = "Plugin";

            kindComboBox.BeginUpdate();
            foreach (HeadlinePlugin plugin in HeadlinePluginManager.Plugins)
            {
                kindComboBox.Items.Add(new KindCombo(plugin.Kind, plugin));
            }
            // 1番目のアイテムを選択状態にする
            if (kindComboBox.Items.Count > 0)
            {
                kindComboBox.SelectedIndex = 0;
            }
            kindComboBox.EndUpdate();

            #endregion // kindComboBoxの初期化

            UpdateHeadlineListView();
        }

        private void nameCutMenuItem_Click(object sender, EventArgs e)
        {
            nameTextBox2.Cut();
        }

        private void nameCopyMenuItem_Click(object sender, EventArgs e)
        {
            nameTextBox2.Copy();
        }

        private void namePasteMenuItem_Click(object sender, EventArgs e)
        {
            nameTextBox2.Paste();
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            HeadlinePlugin plugin = ((KindCombo)kindComboBox.SelectedItem).Plugin;
            HeadlineBase headline = plugin.CreateInstance();
            headline.Name = nameTextBox2.Text;
            headline.CreatedHeadlineByManual();

            HeadlineManager.AddHeadline(headline);

            nameTextBox2.Text = string.Empty;
            UpdateHeadlineListView();
        }

        private void removeButton_Click(object sender, EventArgs e)
        {
            RemoveHeadline();
        }

        /// <summary>
        /// リストビューで選択されているヘッドラインを削除する
        /// </summary>
        private void RemoveHeadline()
        {
            // 削除するHeadlineのリスト
            List<HeadlineBase> removed = new List<HeadlineBase>();
            foreach (int v in headlineListView.SelectedIndices)
            {
                removed.Add(HeadlineManager.Headlines[v]);
            }

            // 削除
            foreach (HeadlineBase headline in removed)
            {
                HeadlineManager.RemoveHeadline(headline);
            }

            UpdateHeadlineListView();
        }

        /// <summary>
        /// 選択されたヘッドラインを上に上げる
        /// </summary>
        private void UpHeadline()
        {
            if (headlineListView.SelectedIndices.Count == 1 && headlineListView.SelectedIndices[0] > 0)
            {
                HeadlineBase selectedHeadline = (HeadlineBase)headlineListView.Items[headlineListView.SelectedIndices[0]].Tag;

                HeadlineManager.Up(selectedHeadline);

                headlineListView.BeginUpdate();
                
                UpdateHeadlineListView();

                // 選択を復元する
                for (int i = 0; i < headlineListView.Items.Count; ++i)
                {
                    if ((HeadlineBase)headlineListView.Items[i].Tag == selectedHeadline)
                    {
                        headlineListView.Items[i].Selected = true;
                        break;
                    }
                }

                headlineListView.EndUpdate();
            }
        }

        /// <summary>
        /// 選択されたヘッドラインを下に下げる
        /// </summary>
        private void DownHeadline()
        {
            if (headlineListView.SelectedIndices.Count == 1
                && headlineListView.SelectedIndices[0] != -1 && headlineListView.SelectedIndices[0] < headlineListView.Items.Count - 1)
            {
                HeadlineBase selectedHeadline = (HeadlineBase)headlineListView.Items[headlineListView.SelectedIndices[0]].Tag;

                HeadlineManager.Down(selectedHeadline);

                headlineListView.BeginUpdate();

                UpdateHeadlineListView();

                // 選択を復元する
                for (int i = 0; i < headlineListView.Items.Count; ++i)
                {
                    if ((HeadlineBase)headlineListView.Items[i].Tag == selectedHeadline)
                    {
                        headlineListView.Items[i].Selected = true;
                        break;
                    }
                }

                headlineListView.EndUpdate();
            }
        }

        private void upButton_Click(object sender, EventArgs e)
        {
            UpHeadline();
        }

        private void downButton_Click(object sender, EventArgs e)
        {
            DownHeadline();
        }

        private void upHeadlineMenuItem_Click(object sender, EventArgs e)
        {
            UpHeadline();
        }

        private void downHeadlineMenuItem_Click(object sender, EventArgs e)
        {
            DownHeadline();
        }

        private void settingMenuItem_Click(object sender, EventArgs e)
        {
            if (headlineListView.SelectedIndices.Count == 1
                && headlineListView.SelectedIndices[0] != -1)
            {
                ((HeadlineBase)headlineListView.Items[headlineListView.SelectedIndices[0]].Tag).ShowSettingForm();
            }
        }

        private void removeHeadlineMenuItem_Click(object sender, EventArgs e)
        {
            RemoveHeadline();
        }

        private void headlineListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (headlineListView.SelectedIndices.Count == 0)
            {
                removeButton.Enabled = false;
                removeHeadlineMenuItem.Enabled = false;
                upButton.Enabled = false;
                upHeadlineMenuItem.Enabled = false;
                downButton.Enabled = false;
                downHeadlineMenuItem.Enabled = false;
                settingMenuItem.Enabled = false;
            }
            else
            {
                removeButton.Enabled = true;
                removeHeadlineMenuItem.Enabled = true;
                if (headlineListView.SelectedIndices.Count == 1 && headlineListView.SelectedIndices[0] > 0)
                {
                    upButton.Enabled = true;
                    upHeadlineMenuItem.Enabled = true;
                }
                else
                {
                    upButton.Enabled = false;
                    upHeadlineMenuItem.Enabled = false;
                }
                if (headlineListView.SelectedIndices.Count == 1
                    && headlineListView.SelectedIndices[0] != -1 && headlineListView.SelectedIndices[0] < headlineListView.Items.Count - 1)
                {
                    downButton.Enabled = true;
                    downHeadlineMenuItem.Enabled = true;
                }
                else
                {
                    downButton.Enabled = false;
                    downHeadlineMenuItem.Enabled = false;
                }
                settingMenuItem.Enabled = true;
            }
        }
    }
}