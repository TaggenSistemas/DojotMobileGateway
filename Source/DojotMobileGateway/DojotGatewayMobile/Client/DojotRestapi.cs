using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using DojotGatewayMobile.Data.Models;

namespace DojotGatewayMobile.Client
{
    public class DojotRestapi
    {
        private string _dojotAddr;
        private HttpClient client;

        public DojotRestapi(string dojotAddr)
        {
            _dojotAddr = dojotAddr;
            client = new HttpClient
            {
                Timeout = TimeSpan.FromMinutes(1)
            };
        }

        public async Task<DojotApiResponse> GetDevices(string accessToken)
        {
            DojotApiResponse result = null;
            try
            {
                var uri = new Uri($"https://{ _dojotAddr }/device");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                var response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var contentResult = await response.Content.ReadAsStringAsync();
                    var devices = DojotApiResponse.FromJson(contentResult);
                    result = devices;
                }
            }
            catch (Exception)
            {
                // TODO: Tratar erro
            }

            return result;
        }

        public async Task<DojotTokenResponse> LoginAsync(string user, string password)
        {
            DojotTokenResponse result = null;
            var request = new DojotLoginRequest(user, password);
            try
            {
                var uri = new Uri($"https://{ _dojotAddr }/auth");
                var json = JsonConvert.SerializeObject(request, Newtonsoft.Json.Formatting.None,
                            new JsonSerializerSettings
                            {
                                NullValueHandling = NullValueHandling.Ignore
                            });
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(uri, content);
                if (response.IsSuccessStatusCode)
                {
                    var contentResult = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<DojotTokenResponse>(contentResult);
                }
            }
            catch (Exception)
            {
                // TODO: Tratar erro
            }
            return result;
        }
    }
}
