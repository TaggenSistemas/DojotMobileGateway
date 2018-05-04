using System;
using System.Collections.Generic;

using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DojotGatewayMobile.Data.Models
{
    public partial class DojotApiResponse
    {
        [JsonProperty("devices")]
        public Device[] Devices { get; set; }

        [JsonProperty("pagination")]
        public Pagination Pagination { get; set; }
    }

    public partial class Device
    {
        [JsonProperty("attrs")]
        public Dictionary<string, List<Attrib>> Attribs { get; set; }

        [JsonProperty("created")]
        public DateTimeOffset Created { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("templates")]
        public long[] Templates { get; set; }

        [JsonProperty("updated")]
        public DateTimeOffset Updated { get; set; }
    }
    

    public partial class Attrib
    {
        [JsonProperty("created")]
        public DateTimeOffset Created { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("static_value")]
        public string StaticValue { get; set; }

        [JsonProperty("template_id")]
        public string TemplateId { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("value_type")]
        public string ValueType { get; set; }
    }

    public partial class Pagination
    {
        [JsonProperty("has_next")]
        public bool HasNext { get; set; }

        [JsonProperty("next_page")]
        public object NextPage { get; set; }

        [JsonProperty("page")]
        public long Page { get; set; }

        [JsonProperty("total")]
        public long Total { get; set; }
    }

    public partial class DojotApiResponse
    {
        public static DojotApiResponse FromJson(string json) => JsonConvert.DeserializeObject<DojotApiResponse>(json, DojotGatewayMobile.Data.Models.Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this DojotApiResponse self) => JsonConvert.SerializeObject(self, DojotGatewayMobile.Data.Models.Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters = {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }
}
