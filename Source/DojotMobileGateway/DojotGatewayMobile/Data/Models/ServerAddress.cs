using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DojotGatewayMobile.Data.Models
{
    public class ServerAddress
    {
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }
        public string Address { get; set; }
    }
}
