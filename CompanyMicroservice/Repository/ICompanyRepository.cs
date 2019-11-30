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
    {

        /*int Count();
        Task Delete(int id);
        Task<IEnumerable<Company>> GetAll(int tenantId); 
        Task<Company> GetById(int id);
        Task Insert(Company entity);
        IQueryable<Company> Query(Expression<Func<Company, bool>> filter);
        Task<bool> Save();
        void Update(Company entity);*/

    }
}
