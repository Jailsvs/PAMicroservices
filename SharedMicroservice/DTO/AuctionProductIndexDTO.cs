using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SharedMicroservice.DTO
{
    public class AuctionProductIndexDTO : AncestorDTO
    {
        [Required]
        public string Description { get; set; }
        public string URLImg { get; set; }
        public string URLDescExt { get; set; }
        [Required]
        public DateTime OpeningDate { get; set; }
        public double MinValue { get; set; }
        public double BidAmount { get; set; }
        public double BidValue { get; set; }
        [Required]
        public int StopwatchTime { get; set; }
        [Required]
        public int CompanyId { get; set; }
     }
}
