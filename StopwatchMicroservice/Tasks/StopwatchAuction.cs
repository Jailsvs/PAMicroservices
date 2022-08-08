using SharedMicroservice.Constants;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Timers;
using System.Text;
using Newtonsoft.Json;
using System.Collections.Concurrent;
using Microsoft.AspNetCore.SignalR;
using System.Diagnostics;

namespace StopwatchMicroservice.Tasks
{
    public class StopwatchAuction: Timer
    {
        private int _auctionId;
        private int _time;
        private DateTime _openingDate;
        private int _count;

        private readonly ConcurrentDictionary<int, StopwatchAuction> _owner;
        private HttpClient _httpStopWSrv = new HttpClient();

    public StopwatchAuction(int auctionId, int time, DateTime openingDate, ConcurrentDictionary<int, StopwatchAuction> owner)
        {
            _auctionId = auctionId;
            _time = time;
            _openingDate = openingDate;
            _count   = time;
            _owner   = owner;

            
            base.Elapsed += new ElapsedEventHandler(TimerCallback);
            base.Interval = 1000;
            base.Start();
        }

        private void TimerCallback(Object o, ElapsedEventArgs e)
        {
            StopwatchAuction s = (StopwatchAuction)o;
            s.Stop();
            try
            {
                if (s._openingDate <= DateTime.Now)
                {
                    s._count -= 1;
                    if (s._httpStopWSrv.BaseAddress == null)
                    {
                        s._httpStopWSrv.BaseAddress = new Uri(ServiceConstants.STOPWATCHSERVICEAPI_URL);
                        s._httpStopWSrv.DefaultRequestHeaders.Accept.Clear();
                        s._httpStopWSrv.DefaultRequestHeaders.Accept.Add(
                            new MediaTypeWithQualityHeaderValue("application/json"));
                    }
                    object data = new
                    {
                        StopwatchTimeCounter = s._count,
                        AuctionProductId = s._auctionId
                    };
                    
                    var httpContent = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
                    var message = s._httpStopWSrv.PostAsync("api/Stopwatch/" + s._auctionId + "/SendMessage", httpContent).Result;
                    if (!message.IsSuccessStatusCode)
                    {
                        throw new Exception("Error Start Auction Stopwatch" + message.Content.ToString());
                    }
                   
                    if (s._count <= 0)
                    {
                        HttpClient _httpClientAuctionSrv = new HttpClient();
                        _httpClientAuctionSrv.BaseAddress = new Uri(ServiceConstants.AUCTIONSERVICEAPI_URL);
                        _httpClientAuctionSrv.DefaultRequestHeaders.Accept.Clear();
                        _httpClientAuctionSrv.DefaultRequestHeaders.Accept.Add(
                            new MediaTypeWithQualityHeaderValue("application/json"));

                        data = new
                        {
                            Id = s._auctionId
                        };

                        httpContent = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
                        message = _httpClientAuctionSrv.PostAsync("api/Auction/" + s._auctionId + "/Close", httpContent).Result;
                        if (!message.IsSuccessStatusCode)
                        {
                            throw new Exception("Error Closing Auction");
                        }
                        StopwatchAuction ss;
                        _owner.TryRemove(this._auctionId, out ss);
                    }

                }
            }
            finally
            {
                if (s._count > 0)
                    s.Start();
            }
            GC.Collect();
        }

        public void Restart()
        {
            this._count = this._time;
        }

    }
}
