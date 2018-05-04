using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace DojotGatewayMobile.Data.Models
{
    public class DojotDevice
    {
        public DojotDevice()
        {
        }

        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }
        public string Instance { get; set; }
        public string DojotId { get; set; }
        public string Placa { get; set; }
        public bool isAmbulance { get; set; }
        public bool isSelected { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string IsInAmbulance { get; set; }
        public string HasStretcher { get; set; }
        public string Label { get; set; }
    }
}
