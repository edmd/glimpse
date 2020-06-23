using System;

namespace glimpse.Models.Messaging
{
    public class RabbitSubscriberEventArgs : EventArgs{
        public RabbitSubscriberEventArgs(RequestResponse requestResponse) {
            this.RequestResponse = requestResponse;
        }

        public RequestResponse RequestResponse { get; }
    }
}
