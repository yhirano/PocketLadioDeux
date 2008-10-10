using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Reflection;
using System.Windows.Forms;
using MiscPocketCompactLibrary2.Reflection;

namespace PocketLadioDeux
{
    public partial class AboutForm : Form
    {
        public AboutForm()
        {
            InitializeComponent();
        }

        private void AboutForm_Load(object sender, EventArgs e)
        {
            applicationNameLabel.Text = AssemblyUtility.GetTitle(Assembly.GetExecutingAssembly());
            versionNumberLabel.Text = AssemblyUtility.GetVersion(Assembly.GetExecutingAssembly()).ToString();
            copyrightLabel.Text = AssemblyUtility.GetCopyright(Assembly.GetExecutingAssembly());

            // プラグインのバージョンを表示
            pluginListBox.BeginUpdate();
            pluginListBox.Items.Clear();
            foreach (HeadlinePlugin plugin in HeadlinePluginManager.Plugins)
            {
                Assembly asm = Assembly.LoadFrom(plugin.Location);
                pluginListBox.Items.Add(string.Format("{0} / {1}", plugin.Kind, AssemblyUtility.GetVersion(asm).ToString()));
            }
            pluginListBox.EndUpdate();
        }
    }
}
