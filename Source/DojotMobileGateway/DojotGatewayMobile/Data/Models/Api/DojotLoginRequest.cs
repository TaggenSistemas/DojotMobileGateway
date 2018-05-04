using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DojotGatewayMobile.Data.Models
{
    public class DojotLoginRequest
    {
        [JsonProperty("username")]
        public string Username { get; private set; }
        [JsonProperty("passwd")]
        public string Passwd { get; private set; }

        public DojotLoginRequest(string user, string password)
        {
            Username = user;
            Passwd = password;
        }
    }
}
