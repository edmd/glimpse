using glimpse_data.Models.Messaging;
using RabbitMQ.Client;
using System;
using System.Text;
using System.Text.Json;
using System.Threading;

namespace glimpse_data.Models
{
    /// <summary>
    /// An HttpEventPublisher is instantiated for each instance of RequestResponse, it calls the 
    /// </summary>
    public class HttpEventPublisher
    {
        RabbitPublisher _publisher;

        private int _interval = 0;
        private Timer _timer;
        private RequestResponse _requestResponse;

        public HttpEventPublisher(RequestResponse requestResponse, RabbitPublisher publisher)
        {
            _requestResponse = requestResponse;
            _interval = requestResponse.Interval > 0 ? requestResponse.Interval * 1000 : 5000; // default to 5s
            _timer = new Timer(RaiseHttpEvent, null, _interval, 0);

            _publisher = publisher;
        }

        private void RaiseHttpEvent(Object state)
        {
            _publisher.Publish(_requestResponse); // Pop message onto the queue

            _timer.Change(_interval, Timeout.Infinite);
        }
    }
}