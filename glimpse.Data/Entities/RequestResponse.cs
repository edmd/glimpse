using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;

namespace glimpse.Entities
{
    [JsonObject]
    [JsonConverter(typeof(RequestResponseJsonConverter))]
    public class RequestResponse : IIdentifier
    {
        [Key]
        [JsonProperty]
        public Guid Id { get; set; }

        [JsonProperty]
        public Guid CompanyId { get; set; }

        [JsonProperty]
        public bool IsActive { get; set; }

        /// <summary>
        /// We can use the Interval property to determine if the message on the queue is stale by
        /// comparing the time it was placed on the queue to the current time and if the timespan is
        /// say double the interval, then discard message
        /// </summary>
        [JsonProperty]
        public int Interval { get; set; }

        [JsonProperty]
        public virtual ICollection<Header> Headers { get; set; }

        #region Request...

        [Required]
        [JsonProperty]
        public Uri Url { get; set; }

        public void AddRequestHeader(string key, string value)
        {
            if(Headers == null) { Headers = new List<Header>(); }

            if (key != null && Headers.Count(x => x.Key == key && x.IsRequestHeader == true) == 0)
            {
                Headers.Add(
                    new Header {
                        Id = Guid.NewGuid(),
                        Key = key, 
                        Value = value, 
                        IsRequestHeader = true
                    });
            }
        }

        public void RemoveRequestHeader(string key)
        {
            if (Headers != null)
            {
                var headerToRemove = Headers.First(x => x.Key == key);
                Headers.Remove(headerToRemove);
            }
        }

        [Required]
        [JsonProperty]
        public string Method { get; set; }

        [JsonProperty]
        public string RequestBody { get; set; }

        #endregion

        #region Response...

        [Required]
        [JsonProperty]
        public HttpStatusCode ResponseStatus { get; set; }

        public void AddResponseHeader(string key, string value)
        {
            if (Headers == null) { Headers = new List<Header>(); }

            if (key != null && Headers.Count(x => x.Key == key && x.IsRequestHeader == false) == 0)
            {
                Headers.Add(
                    new Header
                    {
                        Id = Guid.NewGuid(),
                        Key = key,
                        Value = value,
                        IsRequestHeader = false
                    });
            }
        }

        public void RemoveResponseHeader(string key)
        {
            var headerToRemove = Headers.First(x => x.Key == key);
            Headers.Remove(headerToRemove);
        }

        [JsonProperty]
        public string ResponseBody { get; set; }

        [JsonProperty]
        public int AcceptableResponseTimeMs { get; set; }

        #endregion
    }

    public class RequestResponseJsonConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            // TODO: remove empty values from json output
            var requestResponse = (RequestResponse)value;
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