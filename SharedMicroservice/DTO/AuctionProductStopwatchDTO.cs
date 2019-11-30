using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SharedMicroservice.DTO
{
    public class AuctionProductStopwatchDTO : AncestorDTO
    {
        [Required]
        public DateTime OpeningDate { get; set; }
        [Required]
        public int StopwatchTime { get; set; }
    }
}
