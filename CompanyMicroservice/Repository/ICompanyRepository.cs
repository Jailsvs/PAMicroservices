using CompanyMicroservice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using SharedMicroservice.Repository;

namespace CompanyMicroservice.Repository
{
    public interface ICompanyRepository: IRepository<Company>
    {}
}
