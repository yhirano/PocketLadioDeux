using System;

using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Resources;
using System.Windows.Forms;
using System.Reflection;
using MiscPocketCompactLibrary2.Reflection;

namespace PocketLadioDeux.NetLadioHeadlinePlugin
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
        private readonly ResourceManager messagesResource = new ResourceManager("PocketLadioDeux.NetLadioHeadlinePlugin.MessagesResource", Assembly.GetExecutingAssembly());

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
            string[] namPorperty = { messagesResource.GetString("Nam"), channel.Nam.Trim() };
            string[] gnlPorperty = { messagesResource.GetString("Gnl"), channel.Gnl.Trim() };
            string[] djPorperty = { messagesResource.GetString("Dj"), channel.Dj.Trim() };
            string[] descPorperty = { messagesResource.GetString("Desc"), channel.Desc.Trim() };
            string[] urlPorperty = { messagesResource.GetString("Url"), ((channel.WebSiteUrl != null) ? channel.WebSiteUrl.ToString().Trim() : string.Empty) };
            string[] surlPorperty = { messagesResource.GetString("Surl"), ((channel.Surl != null) ? channel.Surl.ToString().Trim() : string.Empty) };
            string[] timsPorperty = { messagesResource.GetString("Tims"), channel.Tims.ToString().Trim() };
            string[] typePorperty = { messagesResource.GetString("Type"), channel.Type.ToString().Trim() };

            // リスナ数表示格納文字列
            string cln = string.Empty;
            if (channel.Cln != Channel.UNKNOWN_LISTENER_NUM)
            {
                cln = channel.Cln.ToString();
            }
            if (channel.Clns != Channel.UNKNOWN_LISTENER_NUM)
            {
                cln += " / " + channel.Clns.ToString();
            }
            string[] clnPorperty = { messagesResource.GetString("Cln"), cln };

            string bit = string.Empty;
            if (channel.Bit != Channel.UNKNOWN_BITRATE)
            {
                bit = channel.Bit.ToString() + " Kbps";
            }
            string[] bitPorperty = { messagesResource.GetString("Bit"), bit };

            string smpl = string.Empty;
            if (channel.Smpl != Channel.UNKNOWN_SMPL)
            {
                smpl = channel.Smpl.ToString() + " Hz";
            }
            string[] smplPorperty = { messagesResource.GetString("Smpl"), smpl };

            string chs = string.Empty;
            if (channel.Chs != Channel.UNKNOWN_CHS)
            {
                switch (channel.Chs) {
                    case 1:
                        chs = messagesResource.GetString("Mono");
                        break;
                    case 2:
                        chs = messagesResource.GetString("Stereo");
                        break;
                    default:
                        chs = channel.Chs.ToString();
                        break;
                }
            }
            string[] chsPorperty = { messagesResource.GetString("Chs"), chs };

            propertyListView.Items.Add(new ListViewItem(namPorperty));
            propertyListView.Items.Add(new ListViewItem(gnlPorperty));
            propertyListView.Items.Add(new ListViewItem(djPorperty));
            propertyListView.Items.Add(new ListViewItem(descPorperty));
            propertyListView.Items.Add(new ListViewItem(urlPorperty));
            propertyListView.Items.Add(new ListViewItem(surlPorperty));
            propertyListView.Items.Add(new ListViewItem(timsPorperty));
            propertyListView.Items.Add(new ListViewItem(typePorperty));
            propertyListView.Items.Add(new ListViewItem(clnPorperty));
            propertyListView.Items.Add(new ListViewItem(bitPorperty));
            propertyListView.Items.Add(new ListViewItem(smplPorperty));
            propertyListView.Items.Add(new ListViewItem(chsPorperty));

            // プロパティフォームの各カラムの幅を適正な値にする
            foreach (ColumnHeader ch in propertyListView.Columns)
            {
                ch.Width = -2;
            }
        }
    }
}