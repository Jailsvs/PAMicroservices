using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using SharedMicroservice.Models;
using SharedMicroservice.Repository;
using UserMicroservice.Models;

namespace UserMicroservice.Repository
{
    public interface IUserRepository : IRepository<User>
    {
        void IncreaseDecreaseBids(int userId, int bidsQuantity);

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
