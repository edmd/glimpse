using glimpse.Entities;
using System;

namespace glimpse.Models.Queue
{
    public class RabbitSubscriberEventArgs : EventArgs{
        public RabbitSubscriberEventArgs(RequestResponse requestResponse) {
            this.RequestResponse = requestResponse;
        }

        public RequestResponse RequestResponse { get; set; }
    }
}