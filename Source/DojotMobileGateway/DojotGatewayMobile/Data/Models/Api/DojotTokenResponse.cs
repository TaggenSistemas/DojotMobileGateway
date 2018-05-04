using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DojotGatewayMobile.Data.Models
{
    public class DojotTokenResponse
    {
        [JsonProperty("jwt")]
        public string Jwt { get; private set; }
    }
}
