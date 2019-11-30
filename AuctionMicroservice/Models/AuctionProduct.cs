using SharedMicroservice.Models;
using System;

namespace AuctionMicroservice.Models
{
    public class AuctionProduct: AncestorTb
    {
        public string Description { get; set; }
        public string URLImg { get; set; }
        public string URLDescExt { get; set; }
        public DateTime OpeningDate { get; set; }
        public double MinValue { get; set; }
        public double BidAmount { get; set; }
        public double BidValue { get; set; }
        public int StopwatchTime { get; set; }
        public int CompanyId { get; set; }
        public int WinnerAuctionUserId { get; set; }
        public DateTime LastBidDate { get; set; }
        public double CurrentValue { get; set; }
        public string Closed { get; set; }


    }
}
