using System;

using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Reflection;
using System.Resources;
using System.Windows.Forms;

namespace PocketLadioDeux.ShoutCastHeadlinePlugin
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
        ResourceManager messagesResource = new ResourceManager("PocketLadioDeux.ShoutCastHeadlinePlugin.MessagesResource", Assembly.GetExecutingAssembly());

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
            string[] genreProperty = { messagesResource.GetString("Category"), channel.Category.Trim() };
            string[] clusterProperty = { messagesResource.GetString("Cluster"), ((channel.WebSiteUrl != null) ? channel.WebSiteUrl.ToString().Trim() : string.Empty) };
            string[] playingProperty = { messagesResource.GetString("Playing"), channel.Playing.Trim() };
            string[] listenerPorperty = { messagesResource.GetString("Listener"), ((channel.Listener != Channel.UNKNOWN_LISTENER_NUM)?channel.Listener.ToString(): string.Empty) };
            string[] bitratePorperty = { messagesResource.GetString("Bitrate"), ((channel.Bitrate != Channel.UNKNOWN_BITRATE)? channel.Bitrate.ToString() + " Kbps":string.Empty) };

            propertyListView.Items.Add(new ListViewItem(titleProperty));
            propertyListView.Items.Add(new ListViewItem(genreProperty));
            propertyListView.Items.Add(new ListViewItem(clusterProperty));
            propertyListView.Items.Add(new ListViewItem(playingProperty));
            propertyListView.Items.Add(new ListViewItem(listenerPorperty));
            propertyListView.Items.Add(new ListViewItem(bitratePorperty));

            // プロパティフォームの各カラムの幅を適正な値にする
            foreach (ColumnHeader ch in propertyListView.Columns)
            {
                ch.Width = -2;
            }
        }
    }
}