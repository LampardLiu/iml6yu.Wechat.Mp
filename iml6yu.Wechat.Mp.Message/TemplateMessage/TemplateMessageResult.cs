﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace iml6yu.Wechat.Mp.Message.TemplateMessage
{
    public class TemplateMessageResult
    {
        [JsonProperty("errcode")]
        public int ErrCode { get; set; }

        [JsonProperty("errmsg")]
        public string ErrMsg { get; set; }

        [JsonProperty("msgid")]
        public string MsgId { get; set; }
    }
}
