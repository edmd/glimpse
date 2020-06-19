using System;

namespace glimpse_data.Models.Messaging
{
    public class RabbitSubscriberEventArgs : EventArgs{
        public RabbitSubscriberEventArgs(RequestResponse requestResponse) {
            this.RequestResponse = requestResponse;
        }

        public RequestResponse RequestResponse { get; }
    }
}
