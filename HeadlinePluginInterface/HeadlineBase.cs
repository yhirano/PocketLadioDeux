﻿using System;
using System.Collections.Generic;
using System.IO;
using OpenNETCF.ComponentModel;
using MiscPocketCompactLibrary2.Net;

namespace PocketLadioDeux.HeadlinePluginInterface
{
    /// <summary>
    /// ヘッドラインインターフェース
    /// </summary>
    public abstract class HeadlineBase : IComparer<IChannel>
    {
        /// <summary>
        /// ヘッドラインの取得のためのネットワーク設定
        /// </summary>
        protected HttpConnection connectionSetting;

        /// <summary>
        /// ヘッドラインの取得のためのネットワーク設定を設定する
        /// </summary>
        public HttpConnection ConnectionSetting
        {
            set
            {
                if (value != null)
                {
                    connectionSetting = value;
                }
                else
                {
                    connectionSetting = new HttpConnection();
                }
            }
        }

        /// <summary>
        /// ヘッドライン名を取得・設定する
        /// </summary>
        public abstract string Name { get; set; }

        /// <summary>
        /// ヘッドラインの種類の名前を取得する
        /// </summary>
        public abstract string Kind { get; }

        private object channelsLock = new object();

        /// <summary>
        /// 取得している番組のリストを取得する
        /// </summary>
        public IChannel[] ChannelsA
        {
            get
            {
                lock (channelsLock)
                {
                    return Channels;
                }
            }
        }

        /// <summary>
        /// 取得している番組のリストを取得する
        /// </summary>
        protected abstract IChannel[] Channels { get; }

        private object channelsMatchesToFilterLock = new object();

        /// <summary>
        /// フィルターにマッチした番組を取得する
        /// </summary>
        public IChannel[] ChannelsMatchesToFilterA
        {
            get
            {
                lock (channelsMatchesToFilterLock)
                {
                    return ChannelsMatchesToFilter;
                }
            }
        }

        /// <summary>
        /// フィルターにマッチした番組を取得する
        /// </summary>
        protected abstract IChannel[] ChannelsMatchesToFilter { get; }

        private object channelsUnmatchesToFilterLock = new object();

        /// <summary>
        /// フィルターにマッチしない番組を取得する
        /// </summary>
        public IChannel[] ChannelsUnmatchesToFilterA
        {
            get
            {
                lock (channelsUnmatchesToFilterLock)
                {
                    return ChannelsUnmatchesToFilter;
                }
            }
        }

        /// <summary>
        /// フィルターにマッチしない番組を取得する
        /// </summary>
        protected abstract IChannel[] ChannelsUnmatchesToFilter { get; }

        /// <summary>
        /// ヘッドラインをネットから取得した時刻
        /// </summary>
        private DateTime checkTime = DateTime.MinValue;

        /// <summary>
        /// ヘッドラインをネットから取得した時刻を取得する
        /// </summary>
        public DateTime CheckTime
        {
            get { return checkTime; }
        }

        /// <summary>
        /// 現在ヘッドラインをネットから取得しているか
        /// </summary>
        private bool isFetching = false;

        /// <summary>
        /// 現在ヘッドラインをネットから取得しているかを取得する
        /// </summary>
        public bool IsFetching
        {
            get { return isFetching; }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public HeadlineBase()
        {
            ChannelFetchedEventHandler += delegate
            {
                checkTime = DateTime.Now;
            };
        }

        /// <summary>
        /// 手動でヘッドラインが作成された後に実行されるメソッド。
        /// 手動でヘッドラインが作成された後に何らかの処理をする場合は、
        /// オーバーライドして処理を実装してください。
        /// </summary>
        public virtual void CreatedHeadlineByManual()
        {
            ;
        }

        private object fetchHeadlineLock = new object();

        /// <summary>
        /// ヘッドラインをネットから取得する
        /// </summary>
        public void FetchHeadlineA()
        {
            // ヘッドラインをネットから取得中の場合は何もしないで終了
            if (isFetching == true)
            {
                return;
            }

            lock (fetchHeadlineLock)
            {
                try
                {
                    isFetching = true;

                    OnChannelFetch();

                    FetchHeadline();

                    OnChannelFetched();
                }
                finally
                {
                    isFetching = false;
                    // ヘッドラインのキャンセルフラグを元に戻す
                    fetchCancel = false;
                }
            }
        }

        /// <summary>
        /// ヘッドラインをネットから取得する
        /// </summary>
        protected abstract void FetchHeadline();

        /// <summary>
        /// ヘッドラインの取得をキャンセルする場合はこの値がtrueになる
        /// </summary>
        protected bool fetchCancel = false;

        /// <summary>
        /// ヘッドラインの取得をキャンセルする
        /// </summary>
        public void FetchCancel()
        {
            fetchCancel = true;
        }

        /// <summary>
        /// 指定の番組がフィルターにマッチするか
        /// </summary>
        /// <param name="channel">番組</param>
        /// <returns>番組がフィルターにマッチする場合はtrue、しない場合はfalse</returns>
        public abstract bool IsMatchToFilter(IChannel channel);

        /// <summary>
        /// 番組が追加された時に発生するイベント
        /// </summary>
        public virtual event EventHandler<ChannelAddedEventArgs> ChannelAddedEventHandler;

        /// <summary>
        /// HeadlineAddedEventHandlerイベントの実行
        /// </summary>
        /// <param name="headline">追加されたヘッドライン</param>
        protected virtual void OnChannelAdded(IChannel channel)
        {
            if (ChannelAddedEventHandler != null)
            {
                ChannelAddedEventHandler(this, new ChannelAddedEventArgs(channel));
            }
        }

        /// <summary>
        /// 番組の取得直前に発生するイベント
        /// </summary>
        public event EventHandler ChannelFetchEventHandler;

        /// <summary>
        /// ChannelFetchEventHandlerイベントの実行
        /// </summary>
        private void OnChannelFetch()
        {
            if (ChannelFetchEventHandler != null)
            {
                ChannelFetchEventHandler(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// 番組の取得が終了した時に発生するイベント
        /// </summary>
        public event EventHandler ChannelFetchedEventHandler;

        /// <summary>
        /// ChannelFetchedEventHandlerイベントの実行
        /// </summary>
        private void OnChannelFetched()
        {
            if (ChannelFetchedEventHandler != null)
            {
                ChannelFetchedEventHandler(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// ヘッドラインの設定フォームを表示する
        /// </summary>
        public abstract void ShowSettingForm();

        /// <summary>
        /// フィルターに単語を登録するためにヘッドライン設定フォームを表示する
        /// </summary>
        /// <param name="filterWord">フィルターに追加する単語</param>
        public abstract void ShowSettingFormForAddFilter(string filterWord);

        /// <summary>
        /// ヘッドライン固有の設定を保存する。
        /// Stream（FileMode.Create/FileAccess.Write）が渡されるのでここにこのヘッドライン固有の情報を書き出してください。
        /// StreamはCloseしないでください。
        /// </summary>
        /// <param name="stream"></param>
        public abstract void Save(Stream stream);

        /// <summary>
        /// ヘッドライン固有の設定を読み込む。
        /// Stream（FileMode.Open/FileAccess.Read）が渡されるのでここにこのヘッドライン固有の情報を読み込んでください。
        /// StreamはCloseしないでください。
        /// </summary>
        public abstract void Load(Stream stream);

        /// <summary>
        /// ヘッドライン内の番組を比較する。
        /// フィルターでソート処理を実装する場合は、オーバーライドして番組を比較できるようにしてください。
        /// </summary>
        /// <param name="x">番組1</param>
        /// <param name="y">番組2</param>
        /// <returns>
        /// 2 つの比較対照値の構文上の関係を示す 32 ビット符号付き整数。
        /// 0より小の場合、番組1が番組2より小さい。
        /// 0の場合、番組1と番組2は等しい。
        /// 0より大の場合、番組1が番組2より大きい。
        /// </returns>
        public virtual int Compare(IChannel x, IChannel y)
        {
            if (x == null)
            {
                if (y == null) { return 0; }
                else { return -1; }
            }
            else
            {
                // 比較しない
                return 1;
            }
        }
    }
}
