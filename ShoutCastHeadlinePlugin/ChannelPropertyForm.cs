﻿using System;

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
            string[] descriptionoProperty = { messagesResource.GetString("Description"), channel.Description.Trim() };
            string[] genreProperty = { messagesResource.GetString("Category"), channel.Category.Trim() };
            string[] lengthProperty = { messagesResource.GetString("Length"), channel.Length.Trim() };
            string[] listenerPorperty = { messagesResource.GetString("Listener"), ((channel.Listener != Channel.UNKNOWN_LISTENER_NUM)?channel.Listener.ToString(): string.Empty) };
            string[] bitratePorperty = { messagesResource.GetString("Bitrate"), ((channel.Bitrate != Channel.UNKNOWN_BITRATE)? channel.Bitrate.ToString() + " Kbps":string.Empty) };

            propertyListView.Items.Add(new ListViewItem(titleProperty));
            propertyListView.Items.Add(new ListViewItem(descriptionoProperty));
            propertyListView.Items.Add(new ListViewItem(genreProperty));
            propertyListView.Items.Add(new ListViewItem(lengthProperty));
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