using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SharedMicroservice.DTO
{
    public class AuctionProductClosedDTO : AncestorDTO
    {
        public int WinnerAuctionUserId { get; set; }
        public DateTime LastBidDate { get; set; }
        public string Closed { get; set; }
    }
}
