using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace DojotGatewayMobile.Data.Models
{
    public class LoginInformation
    {
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }
        public string Address { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
