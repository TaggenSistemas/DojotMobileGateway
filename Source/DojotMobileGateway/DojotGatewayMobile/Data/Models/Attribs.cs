using System;
using System.Collections.Generic;
using System.Text;

namespace DojotGatewayMobile.Data.Models
{
    public class AmbulanceAttribs
    {
        public AmbulanceAttribs(string loc, string stretcher)
        {
            HasStretcher = stretcher;
            Location = loc;
        }

        public string HasStretcher { get; set; }
        public string Location { get; set; }
    }

    public class StretcherAttribs
    {
        public StretcherAttribs(string loc, string ambulance, string sent)
        {
            IsInAmbulance = ambulance;
            Location = loc;
            ReceivedOn = sent;
        }

        public string IsInAmbulance { get; set; }
        public string Location { get; set; }
        public string ReceivedOn { get; set; }
    }
}
