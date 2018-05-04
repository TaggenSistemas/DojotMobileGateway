using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace DojotGatewayMobile.Data.Models
{
    public class DeviceRead
    {
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }
        public string Instance { get; set; }
        public int RSSI { get; set; }
        public DateTime LastReadOn { get; set; }

    }
}
