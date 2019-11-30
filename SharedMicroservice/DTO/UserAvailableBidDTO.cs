using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SharedMicroservice.DTO
{
    public class UserAvailableBidDTO : AncestorDTO
    {
        [Required]
        public int AvailableBids { get; set; }
    }
}
