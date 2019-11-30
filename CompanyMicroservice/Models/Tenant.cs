using SharedMicroservice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyMicroservice.Models
{
    public class Tenant : AncestorTb
    {
        public string Name { get; set; }
        public string Host { get; set; }
    }
}
