using System;

using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Reflection;
using System.Resources;
using System.Windows.Forms;

namespace PocketLadioDeux.PodcastHeadlinePlugin
{
    public partial class SettingForm : Form
    {
        /// <summary>
        /// ヘッドライン
        /// </summary>
        private Headline headline;

        /// <summary>
        /// メッセージ表示用のリソース
        /// </summary>
        private readonly ResourceManager messagesResource = new ResourceManager("PocketLadioDeux.ShoutCastHeadlinePlugin.MessagesResource", Assembly.GetExecutingAssembly());

        public SettingForm(Headline headline)
        {
            InitializeComponent();

            this.headline = headline;
        }

        private void UpdateFilterListView()
        {
            filterListView.BeginUpdate();
            filterListView.Items.Clear();
            foreach (string word in headline.Setting.FilterMatchWords)
            {
                ListViewItem item = new ListViewItem(new string[] { word, messagesResource.GetString("Include") });
                filterListView.Items.Add(item);
            }
            foreach (string word in headline.Setting.FilterExcludeWords)
            {
                ListViewItem item = new ListViewItem(new string[] { word, messagesResource.GetString("Exclude") });
                filterListView.Items.Add(item);
            }
            for (int i = 0; i < filterListView.Columns.Count; ++i)
            {
                filterListView.Columns[i].Width = -2;
            }
            filterListView.EndUpdate();
        }

        private void RemoveSelectedFilterWord()
        {
            if (filterListView.SelectedIndices.Count == 1 && filterListView.SelectedIndices[0] < headline.Setting.FilterMatchWords.Length + headline.Setting.FilterExcludeWords.Length)
            {
                if (filterListView.SelectedIndices[0] < headline.Setting.FilterMatchWords.Length)
                {
                    List<string> words = new List<string>(headline.Setting.FilterMatchWords);
                    words.RemoveAt(filterListView.SelectedIndices[0]);
                    headline.Setting.FilterMatchWords = words.ToArray();
                }
                else
                {
                    List<string> words = new List<string>(headline.Setting.FilterExcludeWords);
                    words.RemoveAt(filterListView.SelectedIndices[0] - headline.Setting.FilterMatchWords.Length);
                    headline.Setting.FilterExcludeWords = words.ToArray();
                }
                UpdateFilterListView();
            }
        }

        internal void ShowDialogAndFocusRssUrl()
        {
            baseTabControl.SelectedIndex = 2;
            rssUrlTextBox2.Focus();
            ShowDialog();
        }

        private void SettingForm_Load(object sender, EventArgs e)
        {
            nameTextBox2.Text = headline.Name;
            displayFormatTextBox2.Text = headline.Setting.DisplayFormat;
            rssUrlTextBox2.Text = headline.Setting.RssUrl.ToString();
            UpdateFilterListView();
            // ソート種類を読み込む
            switch (headline.Setting.SortKind)
            {
                case UserSetting.SortKinds.Title:
                    sortComboBox.SelectedIndex = 1;
                    break;
                case UserSetting.SortKinds.Category:
                    sortComboBox.SelectedIndex = 2;
                    break;
                case UserSetting.SortKinds.Author:
                    sortComboBox.SelectedIndex = 3;
                    break;
                case UserSetting.SortKinds.Length:
                    sortComboBox.SelectedIndex = 4;
                    break;
                case UserSetting.SortKinds.None:
                default:
                    sortComboBox.SelectedIndex = 0;
                    break;
            }
            switch (headline.Setting.SortScending)
            {
                case UserSetting.SortScendings.Descending:
                    sortAscendingRadioButton.Checked = false;
                    sortDescendingRadioButton.Checked = true;
                    break;
                case UserSetting.SortScendings.Ascending:
                default:
                    sortDescendingRadioButton.Checked = false;
                    sortAscendingRadioButton.Checked = true;
                    break;
            }
        }

        private void SettingForm_Closing(object sender, CancelEventArgs e)
        {
            headline.Name = nameTextBox2.Text;
            headline.Setting.DisplayFormat = displayFormatTextBox2.Text;
            try
            {
                headline.Setting.RssUrl = new Uri(rssUrlTextBox2.Text);
            }
            catch (UriFormatException)            { ; }
            // ソート種類を保存する
            switch (sortComboBox.SelectedIndex)
            {
                case 1:
                    headline.Setting.SortKind = UserSetting.SortKinds.Title;
                    break;
                case 2:
                    headline.Setting.SortKind = UserSetting.SortKinds.Category;
                    break;
                case 3:
                    headline.Setting.SortKind = UserSetting.SortKinds.Author;
                    break;
                case 4:
                    headline.Setting.SortKind = UserSetting.SortKinds.Length;
                    break;
                case 0:
                default:
                    headline.Setting.SortKind = UserSetting.SortKinds.None;
                    break;
            }
            if (sortDescendingRadioButton.Checked == true)
            {
                headline.Setting.SortScending = UserSetting.SortScendings.Descending;
            }
            else
            {
                headline.Setting.SortScending = UserSetting.SortScendings.Ascending;
            }
        }

        private void includeButton_Click(object sender, EventArgs e)
        {
            if (addFilterTextBox2.Text != string.Empty)
            {
                List<string> words = new List<string>(headline.Setting.FilterMatchWords);
                words.Add(addFilterTextBox2.Text);
                headline.Setting.FilterMatchWords = words.ToArray();
                addFilterTextBox2.Text = string.Empty;
                UpdateFilterListView();
            }
        }

        private void excludeButton_Click(object sender, EventArgs e)
        {
            if (addFilterTextBox2.Text != string.Empty)
            {
                List<string> words = new List<string>(headline.Setting.FilterExcludeWords);
                words.Add(addFilterTextBox2.Text);
                headline.Setting.FilterExcludeWords = words.ToArray();
                addFilterTextBox2.Text = string.Empty;
                UpdateFilterListView();
            }
        }

        private void addFilterTextBox2_TextChanged(object sender, EventArgs e)
        {
            if (addFilterTextBox2.Text == string.Empty)
            {
                includeButton.Enabled = false;
                excludeButton.Enabled = false;
            }
            else
            {
                includeButton.Enabled = true;
                excludeButton.Enabled = true;
            }
        }

        private void filterListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (filterListView.SelectedIndices.Count == 0)
            {
                removeButton.Enabled = false;
            }
            else
            {
                removeButton.Enabled = true;
            }
        }

        private void removeMenuItem_Popup(object sender, EventArgs e)
        {
            if (filterListView.SelectedIndices.Count == 0)
            {
                removeMenuItem.Enabled = false;
            }
            else
            {
                removeMenuItem.Enabled = true;
            }
        }

        private void cutNameMenuItem_Click(object sender, EventArgs e)
        {
            nameTextBox2.Cut();
        }

        private void copyNameMenuItem_Click(object sender, EventArgs e)
        {
            nameTextBox2.Copy();
        }

        private void pasteNameMenuItem_Click(object sender, EventArgs e)
        {
            nameTextBox2.Paste();
        }

        private void cutAddFilterMenuItem_Click(object sender, EventArgs e)
        {
            addFilterTextBox2.Cut();
        }

        private void copyAddFilterMenuItem_Click(object sender, EventArgs e)
        {
            addFilterTextBox2.Copy();
        }

        private void pasteAddFilterMenuItem_Click(object sender, EventArgs e)
        {
            addFilterTextBox2.Paste();
        }

        private void cutDisplayFormatMenuItem_Click(object sender, EventArgs e)
        {
            displayFormatTextBox2.Cut();
        }

        private void copyDisplayFormatMenuItem_Click(object sender, EventArgs e)
        {
            displayFormatTextBox2.Copy();
        }

        private void pasteDisplayFormatMenuItem_Click(object sender, EventArgs e)
        {
            displayFormatTextBox2.Paste();
        }

        private void authorFormatMenuItem_Click(object sender, EventArgs e)
        {
            displayFormatTextBox2.Text =
                displayFormatTextBox2.Text.Remove(displayFormatTextBox2.SelectionStart, displayFormatTextBox2.SelectionLength)
                .Insert(displayFormatTextBox2.SelectionStart, "[[AUTHOR]]");
        }

        private void titleFormatMenuItem_Click(object sender, EventArgs e)
        {
            displayFormatTextBox2.Text =
                displayFormatTextBox2.Text.Remove(displayFormatTextBox2.SelectionStart, displayFormatTextBox2.SelectionLength)
                .Insert(displayFormatTextBox2.SelectionStart, "[[TITLE]]");
        }

        private void descriptionFormatMenuItem_Click(object sender, EventArgs e)
        {
            displayFormatTextBox2.Text =
                displayFormatTextBox2.Text.Remove(displayFormatTextBox2.SelectionStart, displayFormatTextBox2.SelectionLength)
                .Insert(displayFormatTextBox2.SelectionStart, "[[DESCRIPTION]]");
        }

        private void categoryFormatMenuItem_Click(object sender, EventArgs e)
        {
            displayFormatTextBox2.Text =
                displayFormatTextBox2.Text.Remove(displayFormatTextBox2.SelectionStart, displayFormatTextBox2.SelectionLength)
                .Insert(displayFormatTextBox2.SelectionStart, "[[CATEGORY]]");
        }

        private void removeMenuItem_Click(object sender, EventArgs e)
        {
            RemoveSelectedFilterWord();
        }

        private void removeButton_Click(object sender, EventArgs e)
        {
            RemoveSelectedFilterWord();
        }

        private void cutRssUrlMenuItem_Click(object sender, EventArgs e)
        {
            rssUrlTextBox2.Cut();
        }

        private void copyRssUrlMenuItem_Click(object sender, EventArgs e)
        {
            rssUrlTextBox2.Copy();
        }

        private void pasteRssUrlMenuItem_Click(object sender, EventArgs e)
        {
            rssUrlTextBox2.Paste();
        }
    }
}