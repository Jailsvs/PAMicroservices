using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SharedMicroservice.DTO
{
    public class AuctionProductStopwatchTimeDTO : AncestorDTO
    {
        [Required]
        public int StopwatchTimeCounter { get; set; }
        [Required]
        public int AuctionProductId { get; set; }
        public int UserId { get; set; }

    }
}
