using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using AuthService.BE;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace AuthService
{
    public class AASKNDService : IAuthService
    {
        private readonly HttpClient _httpClient;

        private string _mainUrl;
        private string _authUrl;
        private string _userInfoUrl;

        public AASKNDService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _mainUrl = configuration["AASKND:MainURI"];
            _authUrl = _mainUrl + configuration["AASKND:AuthPath"];
            _userInfoUrl = _mainUrl + configuration["AASKND:UserInfoPath"];
        }

        public async Task<KNDAuthInfo> Auth(string login, string password)
        {
            try
            {
                var content = new Dictionary<string, string>
                {
                    {"grant_type", "password"},
                    {"username", login},
                    {"password", password},
                };

                using (var response = await _httpClient.PostAsync(_authUrl, new FormUrlEncodedContent(content)))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        return JsonConvert.DeserializeObject<KNDAuthInfo>(await response.Content.ReadAsStringAsync(), new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                    }
                    var errorMessage = response.Content.ReadAsStringAsync().Result;
                    throw new Exception(errorMessage);
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<KndUserInfo> GetUserInfo(string token, string tokenType)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenType, token);
                    using (var response = await client.GetAsync(_userInfoUrl))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            var userInfoResponse = JsonConvert.DeserializeObject<KndUserInfoResponse>(await response.Content.ReadAsStringAsync(), new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                            return KndUserInfo.MapToUserInfo(userInfoResponse);
                        }
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }        
    }
}
