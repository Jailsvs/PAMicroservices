using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SharedMicroservice.DTO
{
    public class AuctionBidDTO : AncestorDTO
    {
        [Required]
        public int UserId { get; set; }
        public int AuctionProductId { get; set; }
    }
}
