namespace iml6yu.Wechat.Mp.Message
{
    /// <summary>
    /// 访问参数配置
    /// </summary>
    public class WechatAccessOption : IWechatOption
    {
        /// <summary>
        /// 
        /// </summary>
        public WechatAccessOption()
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="appid"></param>
        /// <param name="appsecret"></param>
        public WechatAccessOption(string appid, string appsecret)
        {
            AppId = appid;
            AppSecret = appsecret;
        }
        /// <summary>
        /// appid
        /// </summary>
        public string AppId { get; set; }

        /// <summary>
        /// 密钥
        /// </summary>
        public string AppSecret { get; set; } 
        
    }
}
