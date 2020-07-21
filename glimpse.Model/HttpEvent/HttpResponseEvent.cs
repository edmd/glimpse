using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace glimpse.Models.HttpEvent
{
    public class HttpResponseEvent
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
        public Guid RequestResponseId { get; set; }
    }
}