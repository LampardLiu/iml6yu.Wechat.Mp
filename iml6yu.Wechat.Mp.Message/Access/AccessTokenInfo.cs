using iml6yu.Wechat.Mp.Message.TemplateMessage;
using Newtonsoft.Json;

namespace iml6yu.Wechat.Mp.Message
{
    /// <summary>
    /// 
    /// </summary>
    public class AccessTokenInfo: TemplateMessageResult
    {
        /// <summary>
        /// token
        /// </summary>
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        /// <summary>
        /// 相对过期时间
        /// </summary>
        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }
    }
}
