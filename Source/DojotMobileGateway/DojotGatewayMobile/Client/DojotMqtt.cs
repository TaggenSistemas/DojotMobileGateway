using Newtonsoft.Json;
using OpenNETCF.MQTT;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DojotGatewayMobile.Client
{
    public class DojotMqtt
    {
        private MQTTClient client;
        private int timeout; // Timeout in seconds

        public DojotMqtt(string address, int port, int timeoutSecs)
        {
            client = new MQTTClient(address, port);
            SetTimeout(timeoutSecs);
        }


        public async Task MQTTPublish(string deviceId, object attribs, string user)
        {
            var topic = $"/{ user }/{deviceId}/attrs";
            var content = JsonConvert.SerializeObject(attribs);

            client.Connect(GenerateRandomKey());
            var i = 0;
            while (!client.IsConnected)
            {
                await Task.Delay(1000);

                if (i++ > timeout) throw new TimeoutException();
            }


            client.Publish(topic, content, QoS.FireAndForget, false);

        }

        public void SetTimeout(int timeoutinSecs)
        {
            timeout = timeoutinSecs;
        }

        private string GenerateRandomKey()
        {
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            var key = new byte[10];
            rng.GetBytes(key);

            var sb = new StringBuilder();
            foreach (var item in key)
            {
                sb.Append(item.ToString("x2"));
            }

            return sb.ToString();
        }
    }
}
