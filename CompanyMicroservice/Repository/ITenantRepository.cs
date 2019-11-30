using  Microsoft.AspNetCore.Mvc;  
using CompanyMicroservice.Models;  
using CompanyMicroservice.Repository;  
using  System;  
using  System.Collections.Generic;  
using  System.Transactions;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using SharedMicroservice.Repository;

namespace CompanyMicroservice.Repository
{
    public interface ITenantRepository : IRepository<Tenant>
    {
    }
}
