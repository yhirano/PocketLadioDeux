using System;

using System.Collections.Generic;
using System.Text;
using PocketLadioDeux.HeadlinePluginInterface;

namespace PocketLadioDeux.NetLadioHeadlinePlugin
{
    public sealed class Channel : IChannel
    {
        /// <summary>
        /// DSPツールで指定されるURL
        /// </summary>
        private Uri webSiteUrl;

        /// <summary>
        /// DSPツールで指定されるURLを設定する
        /// </summary>
        public Uri WebSiteUrl
        {
            get { return webSiteUrl; }
            internal set { webSiteUrl = value; }
        }

        /// <summary>
        /// 番組情報の表示文字列
        /// </summary>
        public string Display
        {
            get
            {
                if (dislpayFormat != string.Empty)
                {
                    string result = dislpayFormat.Replace("[[NAME]]", Nam)
                        .Replace("[[GENRE]]", Gnl)
                        .Replace("[[DJ]]", dj)
                        .Replace("[[DESC]]", Desc)
                        .Replace("[[CLN]]", ((Cln >= 0) ? Cln.ToString() : "na"))
                        .Replace("[[CLNS]]", ((Clns >= 0) ? Clns.ToString() : "na"))
                        .Replace("[[TITLE]]", Tit)
                        .Replace("[[TIMES]]", Tims.ToString())
                        .Replace("[[BIT]]", ((Bit > 0) ? Bit.ToString() : "na"))
                        .Replace("[[PLAYURL]]", ((PlayUrl != null) ? PlayUrl.ToString() : string.Empty));

                    return result;
                }
                else
                {
                    return Nam;
                }
            }
        }

        /// <summary>
        /// 表示フォーマット
        /// </summary>
        private string dislpayFormat = string.Empty;

        /// <summary>
        /// 表示フォーマットを設定する
        /// </summary>
        internal string DislpayFormat
        {
            set { dislpayFormat = (value != null) ? value : string.Empty; }
        }

        /// <summary>
        /// DSPツールで指定されるジャンル欄
        /// </summary>
        private string gnl = string.Empty;

        /// <summary>
        /// DSPツールで指定されるジャンル欄を取得・設定する
        /// </summary>
        internal string Gnl
        {
            get { return gnl; }
            set { gnl = (value != null) ? value : string.Empty; }
        }

        /// <summary>
        /// DSPツールで指定されるタイトル欄
        /// </summary>
        private string nam = string.Empty;

        /// <summary>
        /// DSPツールで指定されるタイトル欄を取得・設定する
        /// </summary>
        internal string Nam
        {
            get { return nam; }
            set { nam = (value != null) ? value : string.Empty; }
        }

        /// <summary>
        /// DSPツールが送信する現在の曲名情報
        /// </summary>
        private string tit = string.Empty;

        /// <summary>
        /// DSPツールが送信する現在の曲名情報を取得・設定する
        /// </summary>
        internal string Tit
        {
            get { return tit; }
            set { tit = (value != null) ? value : string.Empty; }
        }

        private string desc = string.Empty;

        internal string Desc {
            get { return desc; }
            set { desc = value; }
        }

        private string dj = string.Empty;

        internal string Dj
        {
            get { return dj; }
            set { dj = value; }
        }

        /// <summary>
        /// マウントポイント
        /// </summary>
        private string mnt = string.Empty;

        /// <summary>
        /// マウントポイントを設定する
        /// </summary>
        internal string Mnt
        {
            set { mnt = (value != null) ? value : string.Empty; }
        }

        private Uri surl;

        internal Uri Surl {
            get { return surl; }
            set { surl = value; }
        }

        /// <summary>
        /// Unix epochでの放送開始時間
        /// </summary>
        private int tim;

        /// <summary>
        /// Unix epochでの放送開始時間を取得・設定する
        /// </summary>
        public int Tim
        {
            get { return tim; }
            internal set { tim = value; }
        }

        /// <summary>
        /// yy/mm/dd hh:mm:ss　表記での放送開始時間
        /// </summary>
        private DateTime tims = DateTime.Now;

        /// <summary>
        /// yy/mm/dd hh:mm:ss　表記での放送開始時間を取得・設定する
        /// </summary>
        public DateTime Tims
        {
            set { tims = value; }
            internal get { return tims; }
        }

        /// <summary>
        /// 現リスナ数
        /// </summary>
        private int cln = UNKNOWN_LISTENER_NUM;

        /// <summary>
        /// 現リスナ数を取得・設定する
        /// </summary>
        internal int Cln
        {
            get { return cln; }
            set { cln = value; }
        }

        /// <summary>
        /// リスナ数が不明
        /// </summary>
        internal const int UNKNOWN_LISTENER_NUM = -1;

        /// <summary>
        /// 延べリスナ数
        /// </summary>
        private int clns = UNKNOWN_LISTENER_NUM;

        /// <summary>
        /// 延べリスナ数を取得・設定する
        /// </summary>
        internal int Clns
        {
            get { return clns; }
            set { clns = value; }
        }

        /// <summary>
        /// 配信サーバホスト名
        /// </summary>
        private string srv = string.Empty;

        /// <summary>
        /// 配信サーバホスト名を設定する
        /// </summary>
        internal string Srv
        {
            set { srv = (value != null) ? value : string.Empty; }
        }

        /// <summary>
        /// 配信サーバポート番号
        /// </summary>
        private string prt = string.Empty;

        /// <summary>
        /// 配信サーバポート番号を設定する
        /// </summary>
        internal string Prt
        {
            set { prt = (value != null) ? value : string.Empty; }
        }

        private string type = string.Empty;

        internal string Type
        {
            get { return type; }
            set { type = value; }
        }

        /// <summary>
        /// ビットレート
        /// </summary>
        private int bit = UNKNOWN_BITRATE;

        /// <summary>
        /// ビットレートを取得・設定する
        /// </summary>
        internal int Bit
        {
            get { return bit; }
            set { bit = value; }
        }

        /// <summary>
        /// ビットレートが不明
        /// </summary>
        internal const int UNKNOWN_BITRATE = -1;

        private int smpl = UNKNOWN_SMPL;

        internal int Smpl
        {
            get { return smpl; }
            set { smpl = value; }
        }

        internal const int UNKNOWN_SMPL = -1;

        private int chs = UNKNOWN_CHS;

        internal int Chs
        {
            get { return chs; }
            set { chs = value; }
        }

        internal const int UNKNOWN_CHS = -1;

        /// <summary>
        /// 番組の放送URLを取得する
        /// </summary>
        public Uri PlayUrl
        {
            get
            {
                try
                {
                    return new Uri("http://" + srv + ":" + prt + mnt);
                }
                catch (UriFormatException)
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// フィルタリング対象のワードを取得する。
        /// 返されたワードに従い、フィルタリングを行う。
        /// </summary>
        public string[] FilteredWords
        {
            get
            {
                if (PlayUrl != null)
                {
                    return new string[] { Nam, Gnl, PlayUrl.ToString() };
                }
                else
                {
                    return new string[] { Nam, Gnl };
                }
            }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Channel()
        {
        }

        /// <summary>
        /// 番組の詳細フォームを表示する
        /// </summary>
        public void ShowPropertyForm()
        {
            ChannelPropertyForm channelPropertyForm = new ChannelPropertyForm(this);
            channelPropertyForm.ShowDialog();
            channelPropertyForm.Dispose();
        }
    }
}
