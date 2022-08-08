using SharedMicroservice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyMicroservice.Models
{
    public class Company: AncestorTb
    {
        public string Name { get; set; }
        public string CNPJ { get; set; }
        public string Email { get; set; }
        public string Whats { get; set; }
    }
    //Add-Migration InitialCreate
    //Update-Database
}
