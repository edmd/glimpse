using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace glimpse.Entities
{
    [JsonObject]
    [JsonConverter(typeof(HeaderJsonConverter))]
    public class Header : IIdentifier
    {
        [Key]
        [JsonProperty]
        public Guid Id { get; set; }

        [JsonProperty]
        public string Key { get; set; }

        [JsonProperty]
        public string Value { get; set; }

        [JsonProperty]
        public bool IsRequestHeader { get; set; }

        [JsonIgnore]
        public virtual RequestResponse RequestResponse { get; set; }
    }

    public class HeaderJsonConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            // TODO: remove empty values from json output
            var header = (Header)value;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override bool CanRead
        {
            get { return false; }
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Header);
        }
    }
}
