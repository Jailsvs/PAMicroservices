using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SharedMicroservice.DTO
{
    public class AuctionProductStopwatchBidDTO : AncestorDTO
    {
        public DateTime OpeningDate { get; set; }
        public int StopwatchTime { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public int AuctionProductId { get; set; }
    }
}
