using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharedMicroservice.Models
{
    public class AncestorTb
    {
        public int Id { get; set; }
        public int TenantId { get; set; }
        public bool IsDeleted { get; set; }
    }
}
