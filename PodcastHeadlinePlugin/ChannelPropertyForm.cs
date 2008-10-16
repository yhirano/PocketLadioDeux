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
    public partial class ChannelPropertyForm : Form
    {
        /// <summary>
        /// 番組
        /// </summary>
        private readonly Channel channel;

        /// <summary>
        /// メッセージ表示用のリソース
        /// </summary>
        private readonly ResourceManager messagesResource = new ResourceManager("PocketLadioDeux.PodcastHeadlinePlugin.MessagesResource", Assembly.GetExecutingAssembly());

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="channel">番組</param>
        public ChannelPropertyForm(Channel channel)
        {
            this.channel = channel;

            InitializeComponent();
        }

        private void ChannelPropertyForm_Load(object sender, EventArgs e)
        {
            string[] titleProperty = { messagesResource.GetString("Title"), channel.Title.Trim() };
            string[] descriptionProperty = { messagesResource.GetString("Description"), channel.Description.Trim() };
            string[] dateProperty = { messagesResource.GetString("Date"), channel.Date.ToString() };
            string[] categoryPorperty = { messagesResource.GetString("Category"), channel.Category.Trim() };
            string[] anthorPorperty = { messagesResource.GetString("Author"), channel.Author.Trim() };
            string[] lengthPorperty = { messagesResource.GetString("Length"), channel.Length.Trim() };

            propertyListView.Items.Add(new ListViewItem(titleProperty));
            propertyListView.Items.Add(new ListViewItem(descriptionProperty));
            propertyListView.Items.Add(new ListViewItem(dateProperty));
            propertyListView.Items.Add(new ListViewItem(categoryPorperty));
            propertyListView.Items.Add(new ListViewItem(anthorPorperty));
            propertyListView.Items.Add(new ListViewItem(lengthPorperty));

            // プロパティフォームの各カラムの幅を適正な値にする
            foreach (ColumnHeader ch in propertyListView.Columns)
            {
                ch.Width = -2;
            }
        }
    }
}