using Newtonsoft.Json;

namespace AuthService.BE
{
    public class KNDAuthInfo
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }

        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }

        public string Scope { get; set; }

        public dynamic Authorities { get; set; }

        public string UserName { get; set; }
    }
}
