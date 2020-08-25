using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace glimpse.Entities
{
    public class HttpResponseEvent : IIdentifier
    {
        [Key]
        [JsonProperty]
        public Guid Id { get; set; }

        [JsonProperty]
        public TimeSpan ElapsedTime { get; set; }

        [JsonProperty]
        public DateTime StartDate { get; set; }

        public HttpResponseType ResponseType { get; set;}

        [JsonProperty]
        public virtual RequestResponse RequestResponse { get; set; }

        // Denormalized data
        [JsonProperty]
        public Uri Url { get; set; }
    }
}