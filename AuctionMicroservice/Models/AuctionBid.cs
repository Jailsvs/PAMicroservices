using SharedMicroservice.Models;
using System;

namespace AuctionMicroservice.Models
{
    public class AuctionBid : AncestorTb
    {
        public DateTime BidDate { get; set; }
        public int UserId { get; set; }
        public int AuctionProductId { get; set; }

    }
}
