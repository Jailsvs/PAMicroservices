using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SharedMicroservice.DTO
{
    public class UserUpdateDTO : AncestorDTO
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        public string Whats { get; set; }
        public string UserType { get; set; }
        public int AvailableBids { get; set; }
    }
}
