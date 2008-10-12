using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Resources;
using System.Reflection;
using System.Windows.Forms;
using MiscPocketCompactLibrary2.Reflection;
using PocketLadioDeux.HeadlinePluginInterface;

namespace PocketLadioDeux
{
    public partial class MainForm : Form
    {
        /// <summary>
        /// 現在選択されているヘッドライン
        /// </summary>
        private HeadlineBase _selectedHeadline;

        /// <summary>
        /// 現在選択されているヘッドライン
        /// </summary>
        private HeadlineBase selectedHeadline
        {
            get { return _selectedHeadline; }
            set
            {
                _selectedHeadline = value;
                UpdateHeadlineList();
            }
        }

        /// <summary>
        /// 選択されている番組
        /// </summary>
        private IChannel _selectedChannel;

        /// <summary>
        /// 選択されている番組
        /// </summary>
        private IChannel selectedChannel
        {
            get { return _selectedChannel; }
            set
            {
                _selectedChannel = value;
                if (_selectedChannel != null)
                {
                    infomationLabel.Text = selectedChannel.Display;
                    if (selectedChannel.PlayUrl != null)
                    {
                        playButton.Enabled = true;
                        playMenuItem.Enabled = true;
                    }
                    else
                    {
                        playButton.Enabled = false;
                        playMenuItem.Enabled = false;
                    }
                    if (selectedChannel.WebSiteUrl != null)
                    {
                        webButton.Enabled = true;
                        webMenuItem.Enabled = true;
                    }
                    else
                    {
                        webButton.Enabled = false;
                        webMenuItem.Enabled = false;
                    }
                    propertyButton.Enabled = true;
                    propertyMenuItem.Enabled = true;
                }
                else
                {
                    infomationLabel.Text = string.Empty;
                    playButton.Enabled = false;
                    playMenuItem.Enabled = false;
                    webButton.Enabled = false;
                    webMenuItem.Enabled = false;
                    propertyButton.Enabled = false;
                    propertyMenuItem.Enabled = false;
                }
            }
        }

        /// <summary>
        /// メッセージ表示用のリソース
        /// </summary>
        ResourceManager messagesResource = new ResourceManager("PocketLadioDeux.MessagesResource", Assembly.GetExecutingAssembly());

        private delegate void UpdateHeadlineListDelegate();
        private delegate void UpdateButtonEnableDelegate();
        private delegate void MessageBoxShowDelegate();

        public MainForm()
        {
            InitializeComponent();

            headlineListComboBox.DisplayMember = "Display";
            headlineListComboBox.ValueMember = "Headline";

            HeadlineManager.FetchChannelsAsyncExceptionEventHandler += new EventHandler<FetchChannelsAsyncExceptionEventArgs>(
                delegate(object sender, FetchChannelsAsyncExceptionEventArgs e)
                {
                    string caption = messagesResource.GetString("Warning");
                    string message = string.Empty;

                    // 例外によってメッセージを切り替える
                    if (e.Exception is System.Net.WebException)
                    {
                        message = string.Format(messagesResource.GetString("CanNotFetchChannels"), e.Headline.Name);
                    }
                    else if (e.Exception is OutOfMemoryException)
                    {
                        message = string.Format(messagesResource.GetString("CanNotAllocateMemory"));
                    }
                    else if (e.Exception is IOException)
                    {
                        message = string.Format(messagesResource.GetString("ExceptIOErrorFetchChannels"), e.Headline.Name);
                    }
                    else if (e.Exception is UriFormatException)
                    {
                        message = string.Format(messagesResource.GetString("HeadlineUrlInvalid"), e.Headline.Name);
                    }
                    else if (e.Exception is System.Net.Sockets.SocketException)
                    {
                        message = string.Format(messagesResource.GetString("CanNotFetchChannels"), e.Headline.Name);
                    }
                    else if (e.Exception is NotSupportedException)
                    {
                        message = string.Format(messagesResource.GetString("HeadlineUrlInvalid"), e.Headline.Name);
                    }
                    else if (e.Exception is System.Xml.XmlException)
                    {
                        message = string.Format(messagesResource.GetString("ExceptXmlErrorFetchChannels"), e.Headline.Name);
                    }
                    else if (e.Exception is ArgumentException)
                    {
                        message = string.Format(messagesResource.GetString("HeadlineUrlInvalid"), e.Headline.Name);
                    }
                    else
                    {
                        message = string.Format(messagesResource.GetString("ExceptFetchChannels"), e.Headline.Name);
                    }

                    MessageBox.Show(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);

                    // 例外を発生させたヘッドラインが、選択しているヘッドラインと同一の場合、
                    // ステータスバーの更新をし、Updateボタンを有効にする。
                    if (e.Headline == selectedHeadline)
                    {
                        UpdateMainStatusBar();
                        updateButton.Enabled = true;
                        cancelButton.Enabled = false;
                    }
                });
            HeadlineManager.FetchChannelsAsyncCancelEventHandler += new EventHandler<FetchChannelsAsyncCancelEventArgs>(
                delegate(object sender, FetchChannelsAsyncCancelEventArgs e)
                {
                    string caption = messagesResource.GetString("Warning");
                    string message = string.Format(messagesResource.GetString("CancelFetchChannels"), e.Headline.Name);
                    MessageBox.Show(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);

                    // 取得をキャンセルしたヘッドラインが、選択しているヘッドラインと同一の場合、
                    // ステータスバーの更新をし、Updateボタンを有効にする。
                    if (e.Headline == selectedHeadline)
                    {
                        UpdateMainStatusBar();
                        updateButton.Enabled = true;
                        cancelButton.Enabled = false;
                    }
                });

            channelAddedEventHandler = new EventHandler<ChannelAddedEventArgs>(
                delegate(object sender, ChannelAddedEventArgs e)
                {
                    if (InvokeRequired == true)
                    {
                        Invoke((UpdateHeadlineListDelegate)delegate
                        {
                            if ((HeadlineBase)sender == selectedHeadline)
                            {
                                if (filterCheckBox.Checked == false || (filterCheckBox.Checked == true && selectedHeadline.IsMatchToFilter(e.Channel) == true))
                                {
                                    AddChannelToHeadlineList(e.Channel);
                                    UpdateMainStatusBar();
                                }
                            }
                        });
                    }
                    else
                    {
                        if ((HeadlineBase)sender == selectedHeadline)
                        {
                            if (filterCheckBox.Checked == false || (filterCheckBox.Checked == true && selectedHeadline.IsMatchToFilter(e.Channel) == true))
                            {
                                AddChannelToHeadlineList(e.Channel);
                                UpdateMainStatusBar();
                            }
                        }
                    }
                });
            channelFetchedEventHandler = new EventHandler(
                delegate(object sender, EventArgs e)
                {
                    // 取得を終了したヘッドラインが、選択しているヘッドラインと同一の場合、
                    // ステータスバーの更新をし、Updateボタンを有効にする。
                    if (InvokeRequired == true)
                    {
                        Invoke(
                            (UpdateButtonEnableDelegate)delegate
                            {
                                if ((HeadlineBase)sender == selectedHeadline)
                                {
                                    UpdateMainStatusBar();
                                    updateButton.Enabled = true;
                                    cancelButton.Enabled = false;
                                }
                            });
                    }
                    else
                    {
                        if ((HeadlineBase)sender == selectedHeadline)
                        {
                            UpdateMainStatusBar();
                            updateButton.Enabled = true;
                            cancelButton.Enabled = false;
                        }
                    }
                });
        }

        /// <summary>
        /// ヘッドラインコンボボックスの値クラス
        /// </summary>
        private class HeadlineCombo
        {
            /// <summary>
            /// 表示
            /// </summary>
            public string Display
            {
                get { return string.Format("{0} - {1}", headline.Name, headline.Kind); }
            }

            /// <summary>
            /// ヘッドライン
            /// </summary>
            private readonly HeadlineBase headline;

            /// <summary>
            /// ヘッドラインを取得する
            /// </summary>
            public HeadlineBase Headline
            {
                get { return headline; }
            }

            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="headline">ヘッドライン</param>
            public HeadlineCombo(HeadlineBase headline)
            {
                this.headline = headline;
            }
        }

        /// <summary>
        /// フォームに関する設定を呼び出す
        /// </summary>
        private void LoadFormSetting()
        {
            topPanel.Height = UserSettingAdapter.Setting.TopPanelHeight;
            headlineListView.Columns[0].Width = UserSettingAdapter.Setting.HeadlineListViewChannelColumnWidth;
            filterCheckBox.Checked = UserSettingAdapter.Setting.FilterEnabled;
        }

        /// <summary>
        /// フォームに関する設定を保存する
        /// </summary>
        private void SaveFormSetting()
        {
            UserSettingAdapter.Setting.TopPanelHeight = topPanel.Height;
            UserSettingAdapter.Setting.HeadlineListViewChannelColumnWidth = headlineListView.Columns[0].Width;
            UserSettingAdapter.Setting.FilterEnabled = filterCheckBox.Checked;
        }

        /// <summary>
        /// コンボボックスのテキスト表示内容
        /// </summary>
        private List<string> headlineComboBoxTexts = new List<string>();

        /// <summary>
        /// ヘッドラインコンボボックスの内容を更新する
        /// </summary>
        private void UpdateHeadlineListComboBox()
        {
            #region ヘッドラインを更新する必要があるかをチェック

            // ヘッドラインの数がコンボボックス項目数と一致しない場合は更新の必要あり
            if (HeadlineManager.Headlines.Length == headlineListComboBox.Items.Count)
            {
                bool isUpdate = false;
                for (int i = 0; i < headlineListComboBox.Items.Count; ++i)
                {
                    // コンボボックスのヘッドラインと、所持しているヘッドラインが異なる場合には更新の必要あり
                    if (HeadlineManager.Headlines[i] != ((HeadlineCombo)headlineListComboBox.Items[i]).Headline)
                    {
                        isUpdate = true;
                        break;
                    }
                    // コンボボックスのテキスト表示内容と、所持しているヘッドラインのテキスト表示内容が異なる場合には更新の必要あり
                    if (((HeadlineCombo)headlineListComboBox.Items[i]).Display != headlineComboBoxTexts[i])
                    {
                        isUpdate = true;
                        break;
                    }
                }
                // 更新の必要がない場合はここで終了
                if (isUpdate == false)
                {
                    return;
                }
            }

            #endregion // ヘッドラインを更新する必要があるかをチェック

            // 選択されているヘッドラインを取得
            HeadlineCombo selectedHeadlineCombo = (HeadlineCombo)headlineListComboBox.SelectedItem;
            HeadlineBase selected = (selectedHeadlineCombo != null) ? selectedHeadlineCombo.Headline : null;

            headlineListComboBox.BeginUpdate();
            headlineComboBoxTexts.Clear();
            // コンボボックスの内容を生成
            headlineListComboBox.Items.Clear();

            foreach (HeadlineBase headline in HeadlineManager.Headlines)
            {
                HeadlineCombo combo = new HeadlineCombo(headline);
                headlineListComboBox.Items.Add(combo);
                headlineComboBoxTexts.Add(combo.Display);
            }

            // コンボボックスの選択を復元
            if (selected != null)
            {
                for (int i = 0; i < headlineListComboBox.Items.Count; ++i)
                {
                    if (((HeadlineCombo)headlineListComboBox.Items[i]).Headline == selected)
                    {
                        headlineListComboBox.SelectedIndex = i;
                        break;
                    }
                }
            }

            // 選択が存在しない場合
            if (selected == null || headlineListComboBox.SelectedIndex == -1)
            {
                // ヘッドラインが存在したら一番最初のヘッドラインを選択
                if (headlineListComboBox.Items.Count > 0)
                {
                    headlineListComboBox.SelectedIndex = 0;
                }
            }

            headlineListComboBox.EndUpdate();
        }

        private void UpdateHeadlineList()
        {
            IChannel[] channels = null;
            if (selectedHeadline != null)
            {
                channels = (filterCheckBox.Checked == true) ?
                    selectedHeadline.ChannelsMatchesToFilterA : selectedHeadline.ChannelsA;
            }

            headlineListView.BeginUpdate();

            // 現在選択されている番組を取得
            IChannel selected = selectedChannel;
            // 選択されている番組が見つかったか
            bool isSelectedFound = false;

            headlineListView.Items.Clear();

            if (selectedHeadline != null && channels != null)
            {
                foreach (IChannel channel in channels)
                {
                    ListViewItem item = AddChannelToHeadlineList(channel);
                    // 番組の選択を復元
                    if (isSelectedFound == false && selected == channel)
                    {
                        item.Selected = true;
                        isSelectedFound = true;
                    }
                }
            }

            headlineListView.EndUpdate();

            UpdateMainStatusBar();
        }

        private ListViewItem AddChannelToHeadlineList(IChannel channel)
        {
            ListViewItem item = new ListViewItem(channel.Display);
            item.Tag = channel;
            // フィルターを使用する場合
            if (filterCheckBox.Checked == true)
            {
                // 番組がリストビューに1つもない場合は番組を追加
                if (headlineListView.Items.Count == 0)
                {
                    headlineListView.Items.Add(item);
                }
                // 番組をソートしながらリストビューに追加する
                else
                {
                    bool isAddedItem = false; // リストに追加されたか
                    // ソートとしながら追加
                    for (int i = 0; i < headlineListView.Items.Count; ++i)
                    {
                        int compResult = selectedHeadline.Compare(channel, (IChannel)headlineListView.Items[i].Tag);
                        if (compResult == -1)
                        {
                            headlineListView.Items.Insert(i, item);
                            isAddedItem = true;
                            break;
                        }
                    }
                    // 末尾に追加するアイテムだったっぽいので
                    if (isAddedItem == false)
                    {
                        headlineListView.Items.Add(item);
                    }
                }
            }
            // フィルターを使用しない場合
            else
            {
                headlineListView.Items.Add(item);
            }
            return item;
        }

        private void UpdateMainStatusBar()
        {
            // ステータスバーの更新
            if (selectedHeadline != null)
            {
                mainStatusBar.Text = string.Format("{0}CHs / {1}",
                    headlineListView.Items.Count.ToString(),
                    (selectedHeadline.CheckTime != DateTime.MinValue) ? selectedHeadline.CheckTime.ToString("G") : string.Empty);
            }
            else
            {
                mainStatusBar.Text = string.Empty;
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // フォームのテキストバーを設定
            this.Text = AssemblyUtility.GetTitle(Assembly.GetExecutingAssembly());

            // 設定の復元
            LoadFormSetting();

            // 初めて起動された場合
            if (UserSettingAdapter.IsSettingCreatedNew == true)
            {
                // メッセージボックスにファイルパスを設定するように表示する
                MessageBox.Show(messagesResource.GetString("PleaseSettingPath"), messagesResource.GetString("Infomation"), MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1);
                SettingForm settingForm = new SettingForm();
                settingForm.ShowDialog();
                settingForm.Dispose();

                // ねとらじプラグインが見つかった場合は、ねとらじのヘッドラインを作成する
                foreach (HeadlinePlugin plugin in HeadlinePluginManager.Plugins)
                {
                    if (plugin.ClassName == "PocketLadioDeux.NetLadioHeadlinePlugin.Headline")
                    {
                        HeadlineBase headline = plugin.CreateInstance();
                        headline.Name = "ねとらじ";
                        HeadlineManager.AddHeadline(headline);
                        break;
                    }
                }
            }
        }

        private void MainForm_Closing(object sender, CancelEventArgs e)
        {
            SaveFormSetting();
        }

        private void exitMenuItem_Click(object sender, EventArgs e)
        {
            SaveFormSetting();

            Application.Exit();
        }

        private void aboutMenuItem_Click(object sender, EventArgs e)
        {
            AboutForm aboutForm = new AboutForm();
            aboutForm.ShowDialog();
            aboutForm.Dispose();
        }

        private void settingMenuItem_Click(object sender, EventArgs e)
        {
            SettingForm settingForm = new SettingForm();
            settingForm.ShowDialog();
            settingForm.Dispose();
        }

        private void addRemoveHeadlineMenuItem_Click(object sender, EventArgs e)
        {
            HeadlinesForm headlinePluginForm = new HeadlinesForm();
            headlinePluginForm.ShowDialog();
            headlinePluginForm.Dispose();
        }

        private void menuItem_Popup(object sender, EventArgs e)
        {
            #region ヘッドラインの設定のメニューを生成する
            headlineSettingMenuItem.MenuItems.Clear();
            foreach (HeadlineBase headline in HeadlineManager.Headlines)
            {
                MenuItem headlineMenuItem = new MenuItem();
                headlineMenuItem.Text = string.Format("{0} - {1}", headline.Name, headline.Kind);
                headlineMenuItem.Click += new EventHandler(
                    delegate
                    {
                        headline.ShowSettingForm();
                    });
                headlineSettingMenuItem.MenuItems.Add(headlineMenuItem);
            }
            #endregion // ヘッドラインの設定のメニューを生成する

            // メニューの有効・無効を切り替える
            if (HeadlineManager.Headlines.Length > 0)
            {
                headlineSettingMenuItem.Enabled = true;
            }
            else
            {
                headlineSettingMenuItem.Enabled = false;
            }
        }

        private void MainForm_Activated(object sender, EventArgs e)
        {
            UpdateHeadlineListComboBox();
        }

        /// <summary>
        /// 取得中ヘッドラインに番組が追加されたときに発生するイベントのハンドル
        /// </summary>
        private EventHandler<ChannelAddedEventArgs> channelAddedEventHandler;

        /// <summary>
        /// 取得していたヘッドラインの取得が終了したときに発生するイベントのハンドル
        /// </summary>
        private EventHandler channelFetchedEventHandler;

        private void headlineListComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            #region コントロールを選択できないようにする

            filterCheckBox.Enabled = false;

            #endregion // コントロールを選択できないようにする

            if (selectedHeadline != null)
            {
                selectedHeadline.ChannelAddedEventHandler -= channelAddedEventHandler;
                selectedHeadline.ChannelFetchedEventHandler -= channelFetchedEventHandler;
            }

            // 選択しているヘッドラインを更新する
            if (headlineListComboBox.SelectedIndex == -1)
            {
                selectedHeadline = null;
            }
            else
            {
                selectedHeadline = ((HeadlineCombo)headlineListComboBox.Items[headlineListComboBox.SelectedIndex]).Headline;
            }

            if (selectedHeadline != null)
            {
                selectedHeadline.ChannelAddedEventHandler += channelAddedEventHandler;
                selectedHeadline.ChannelFetchedEventHandler += channelFetchedEventHandler;
            }

            // ヘッドラインが未選択の場合はUpdateボタンを押せないようにする
            if (selectedHeadline == null)
            {
                updateButton.Enabled = false;
                cancelButton.Enabled = false;
            }
            // ヘッドラインが取得中の場合はUpdateボタンを押せないようにする
            else if (selectedHeadline.IsFetching == true)
            {
                updateButton.Enabled = false;
                cancelButton.Enabled = true;
            }
            else
            {
                updateButton.Enabled = true;
            }

            #region コントロールを選択できるようにする

            filterCheckBox.Enabled = true;

            #endregion // コントロールを選択できるようにする
        }

        private void headlineListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (headlineListView.SelectedIndices.Count == 1
                && headlineListView.Items[headlineListView.SelectedIndices[0]].Tag is IChannel)
            {
                // 選択されている番組
                selectedChannel = (IChannel)headlineListView.Items[headlineListView.SelectedIndices[0]].Tag;
            }
            else
            {
                selectedChannel = null;
            }
        }

        private void filterCheckBox_CheckStateChanged(object sender, EventArgs e)
        {
            UpdateHeadlineList();
        }

        private void propertyButton_Click(object sender, EventArgs e)
        {
            if (selectedChannel != null)
            {
                selectedChannel.ShowPropertyForm();
            }
        }

        private void playButton_Click(object sender, EventArgs e)
        {
            PlayStreaming();
        }

        /// <summary>
        /// 放送を再生する
        /// </summary>
        private void PlayStreaming()
        {
            if (File.Exists(UserSettingAdapter.Setting.MediaPlayerPath) == false)
            {
                string caption = messagesResource.GetString("Error");
                string message = messagesResource.GetString("MediaPlayerNotFound");
                MessageBox.Show(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
            }
            else if (selectedChannel == null)
            {
                string caption = messagesResource.GetString("Error");
                string message = messagesResource.GetString("SelectedChannelIsEmpty");
                MessageBox.Show(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
            }
            else if (selectedChannel.PlayUrl == null)
            {
                string caption = messagesResource.GetString("Error");
                string message = messagesResource.GetString("PlayUrlIsEmpty");
                MessageBox.Show(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
            }
            else
            {
                Process.Start(UserSettingAdapter.Setting.MediaPlayerPath, selectedChannel.PlayUrl.ToString());
            }
        }

        private void webButton_Click(object sender, EventArgs e)
        {
            AccessToWebSite();
        }

        private void AccessToWebSite()
        {
            if (File.Exists(UserSettingAdapter.Setting.WebBrowserPath) == false)
            {
                string caption = messagesResource.GetString("Error");
                string message = messagesResource.GetString("WebBrowserNotFound");
                MessageBox.Show(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
            }
            else if (selectedChannel == null)
            {
                string caption = messagesResource.GetString("Error");
                string message = messagesResource.GetString("SelectedChannelIsEmpty");
                MessageBox.Show(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
            }
            else if (selectedChannel.WebSiteUrl == null)
            {
                string caption = messagesResource.GetString("Error");
                string message = messagesResource.GetString("WebSiteUrlIsEmpty");
                MessageBox.Show(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
            }
            else
            {
                Process.Start(UserSettingAdapter.Setting.WebBrowserPath, selectedChannel.WebSiteUrl.ToString());
            }
        }

        private void updateButton_Click(object sender, EventArgs e)
        {
            headlineListView.Items.Clear();
            updateButton.Enabled = false;
            cancelButton.Enabled = true;

            HeadlineManager.FetchChannelsAsync(selectedHeadline);
        }

        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            // ヘッドラインリストのフォントサイズを変える
            if (UserSettingAdapter.Setting.HeadlineListFontSize == UserSetting.HeadlineListFontSizes.DefaultSize)
            {
                if (headlineListView.Font.Size != PocketLadioDeuxInfo.HeadlineListDefaultFontSize)
                {
                    headlineListView.Font = new Font(headlineListView.Font.Name, PocketLadioDeuxInfo.HeadlineListDefaultFontSize, headlineListView.Font.Style);
                }
            }
            else
            {
                if (headlineListView.Font.Size != (int)UserSettingAdapter.Setting.HeadlineListFontSize)
                {
                    headlineListView.Font = new Font(headlineListView.Font.Name, (int)UserSettingAdapter.Setting.HeadlineListFontSize, headlineListView.Font.Style);
                }
            }
        }

        private void headlineListView_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter: // 入力ボタンを押したとき
                    PlayStreaming();
                    break;
                default:
                    break;
            }
        }

        private void playMenuItem_Click(object sender, EventArgs e)
        {
            PlayStreaming();
        }

        private void webMenuItem_Click(object sender, EventArgs e)
        {
            AccessToWebSite();
        }

        private void propertyMenuItem_Click(object sender, EventArgs e)
        {
            if (selectedChannel != null)
            {
                selectedChannel.ShowPropertyForm();
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            if (selectedHeadline != null && selectedHeadline.IsFetching == true)
            {
                HeadlineManager.CancelFetchChannelsAsync(selectedHeadline);
            }
        }
    }
}
