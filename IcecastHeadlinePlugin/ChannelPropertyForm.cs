using System;

using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Reflection;
using System.Resources;
using System.Windows.Forms;

namespace PocketLadioDeux.IcecastHeadlinePlugin
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
        ResourceManager messagesResource = new ResourceManager("PocketLadioDeux.IcecastHeadlinePlugin.MessagesResource", Assembly.GetExecutingAssembly());

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
            string[] serverNameProperty = { messagesResource.GetString("ServerName"), channel.ServerName.Trim() };
            string[] genreProperty = { messagesResource.GetString("Genre"), channel.Genre.Trim() };
            string[] currentSongProperty = { messagesResource.GetString("CurrentSong"), channel.CurrentSong.Trim() };
            string[] sampleRatePorperty = { messagesResource.GetString("SampleRate"), channel.SampleRate.ToString() };
            string[] channelsPorperty = { messagesResource.GetString("Channels"), channel.Channels };
            string[] bitratePorperty = { messagesResource.GetString("Bitrate"), ((channel.Bitrate != Channel.UNKNOWN_BITRATE) ? channel.Bitrate.ToString() + " Kbps" : string.Empty) };

            propertyListView.Items.Add(new ListViewItem(serverNameProperty));
            propertyListView.Items.Add(new ListViewItem(genreProperty));
            propertyListView.Items.Add(new ListViewItem(currentSongProperty));
            propertyListView.Items.Add(new ListViewItem(sampleRatePorperty));
            propertyListView.Items.Add(new ListViewItem(channelsPorperty));
            propertyListView.Items.Add(new ListViewItem(bitratePorperty));

            // プロパティフォームの各カラムの幅を適正な値にする
            foreach (ColumnHeader ch in propertyListView.Columns)
            {
                ch.Width = -2;
            }
        }
    }
}