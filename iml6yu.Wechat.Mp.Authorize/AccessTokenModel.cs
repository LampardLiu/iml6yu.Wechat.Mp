using Newtonsoft.Json;

namespace iml6yu.Wechat.Mp.Authorization
{
    public class AccessTokenModel
    {
 
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }

        [JsonProperty("openid")]
        public string OpenId { get; set; }

        [JsonProperty("scope")]
        public string Sscope { get; set; }

        [JsonProperty("errcode")]
        public string ErrCode { get; set; }

        [JsonProperty("errmsg")]
        public string ErrMsg { get; set; }

    }
}
