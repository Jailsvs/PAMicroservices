using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SharedMicroservice.DTO
{
    public class CompanyDTO: AncestorDTO
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string CNPJ { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Whats { get; set; }
    }
}
