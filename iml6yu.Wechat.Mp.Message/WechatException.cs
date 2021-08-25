using System;
using System.Collections.Generic;
using System.Text;

namespace iml6yu.Wechat.Mp.Message
{
    /// <summary>
    /// 微信异常类基础类
    /// </summary>
    public class WechatException : Exception
    {
        public WechatException(string msg) : base(msg) { }
    }

    /// <summary>
    /// 密钥null异常
    /// </summary>
    public class WechatSecretNullException : WechatException
    {
        public WechatSecretNullException(string msg) : base(msg) { }
    }

    /// <summary>
    /// Appidnull异常
    /// </summary>
    public class WechatAppIdNullException : WechatException
    {
        public WechatAppIdNullException(string msg) : base(msg) { }
    }

    /// <summary>
    /// 微信配置异常
    /// </summary>
    public class WechatConfigException : WechatException
    {
        public WechatConfigException(string msg) : base(msg) { }
    }

    /// <summary>
    /// 微信支付异常
    /// </summary>

    public class WechatPayException : WechatException
    {
        public WechatPayException(string msg) : base(msg) { }
    }

    /// <summary>
    /// 模板消息异常
    /// </summary>
    public class WechatTemplateMessageException : WechatException
    {
        public WechatTemplateMessageException(string msg) : base(msg) { }
    }
}
