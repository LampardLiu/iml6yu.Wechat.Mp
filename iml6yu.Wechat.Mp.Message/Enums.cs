﻿using System;
using System.Collections.Generic;
using System.Text;

namespace iml6yu.Wechat.Mp.Message
{
    public enum BasicMessageType
    {
        /// <summary>
        /// 文本消息
        /// </summary>
        TEXT,
        /// <summary>
        /// 图片消息
        /// </summary>
        IMAGE,
        /// <summary>
        /// 声音消息
        /// </summary>
        VOICE,
        /// <summary>
        /// 视频
        /// </summary>
        VIDEO,
        /// <summary>
        /// 短视频
        /// </summary>
        SHORTVIDEO,
        /// <summary>
        /// 定位
        /// </summary>
        LOCATION,
        /// <summary>
        /// 连接
        /// </summary>
        LINK,
        /// <summary>
        /// 事件
        /// </summary>
        EVENT,
        ///// <summary>
        ///// 订阅
        ///// </summary>
        //EVENT_SUBSCRIBE,
        ///// <summary>
        ///// 取消订阅
        ///// </summary>
        //EVENT_UNSUBSCRIBE
    }

    /// <summary>
    /// 事件行为
    /// </summary>
    public enum BasicEventType
    {
        /// <summary>
        /// 订阅
        /// </summary>
        SUBSCRIBE,
        /// <summary>
        /// 取消订阅
        /// </summary>
        UNSUBSCRIBE,
        /// <summary>
        /// 菜单点击
        /// </summary>
        CLICK
    }
}
