using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net;
using System.Net.Http;

namespace glimpse.Models
{
    public class RequestResponse : RequestResponseJsonConverter
    {
        [Key]
        [JsonProperty]
        public Guid Id { get; set; }

        [JsonProperty]
        //[ForeignKey("RequestHeader"), Column(Order = 0)]
        public Guid? RequestHeaderGroupId { get; set; }

        [JsonProperty]
        //[ForeignKey("ResponseHeader"), Column(Order = 1)]
        public Guid? ResponseHeaderGroupId { get; set; }

        [JsonProperty]
        public bool IsActive { get; set; }

        /// <summary>
        /// We can use the Interval property to determine if the message on the queue is stale by
        /// comparing the time it was placed on the queue to the curent time and if the timespan is
        /// say double the interval discard message
        /// </summary>
        [JsonProperty]
        public int Interval { get; set; }

        #region Request...

        [Required]
        [JsonProperty]
        public Uri Url { get; set; }

        [JsonProperty]
        [ForeignKey("RequestHeaderGroupId")]
        public virtual ICollection<Header> RequestHeaders { get;
            // Set not being added here
        }

        public void AddRequestHeader(string key, string value)
        {
            if (key != null && RequestHeaders.Count(x => x.Key == key) == 0)
            {
                RequestHeaders.Add(new Header { Key = key, Value = value });
            }
        }

        public void RemoveRequestHeader(string key)
        {
            var headerToRemove = RequestHeaders.First(x => x.Key == key);
            RequestHeaders.Remove(headerToRemove);
        }

        [Required]
        [JsonProperty]
        public HttpMethod Method { get; set; }

        [JsonProperty]
        public string RequestBody { get; set; }

        #endregion

        #region Response...

        [Required]
        [JsonProperty]
        public HttpStatusCode ResponseStatus { get; set; }

        [JsonProperty]
        [ForeignKey("ResponseHeaderGroupId")]
        public virtual ICollection<Header> ResponseHeaders { get;
            // Set not being added here
        }

        public void AddResponseHeader(string key, string value)
        {
            if (key != null && ResponseHeaders.Count(x => x.Key == key) == 0)
            {
                ResponseHeaders.Add(new Header { Key = key, Value = value });
            }
        }

        public void RemoveResponseHeader(string key)
        {
            var headerToRemove = ResponseHeaders.First(x => x.Key == key);
            ResponseHeaders.Remove(headerToRemove);
        }

        [JsonProperty]
        public string ResponseBody { get; set; }

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