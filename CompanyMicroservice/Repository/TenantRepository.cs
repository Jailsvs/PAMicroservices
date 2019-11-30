using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CompanyMicroservice.DBContexts;
using CompanyMicroservice.Models;
using System.Linq.Expressions;
using System.Collections;
using System.Transactions;

namespace CompanyMicroservice.Repository
{
    public class TenantRepository : ITenantRepository
    {
        private readonly TenantContext _dbContext;

        public TenantRepository(TenantContext dbContext)
        {
            _dbContext = dbContext;
        }

        public bool Commit()
        {
            int changeCount = _dbContext.SaveChanges();
            return changeCount >= 0;
        }

        public int Count()
        {
            return _dbContext.Tb_Tenant.Count();
        }

        public void Delete(Func<Tenant, bool> predicate)
        {
            _dbContext.Set<Tenant>()
            .Where(predicate).ToList()
            .ForEach(del => _dbContext.Set<Tenant>().Remove(del));
        }

        public void Delete(int id)
        {
            var tenant = _dbContext.Tb_Tenant.Find(id);
            if (tenant == null)
                throw new Exception("Tenant not found");
            _dbContext.Tb_Tenant.Remove(tenant);
        }
        
        public void Dispose()
        {
            if (_dbContext != null)
            {
                _dbContext.Dispose();
            }
            GC.SuppressFinalize(this);
        }


        public Tenant Find(params object[] key)
        {
            return _dbContext.Set<Tenant>().Find(key);
        }

        public Tenant First(Expression<Func<Tenant, bool>> predicate)
        {
            return _dbContext.Set<Tenant>().Where(predicate).FirstOrDefault();
        }

        public IQueryable<Tenant> Get(Expression<Func<Tenant, bool>> predicate)
        {
            return _dbContext.Set<Tenant>().Where(predicate);
        }

        public IQueryable<Tenant> GetAll()
        {
            return _dbContext.Set<Tenant>();
        }

        public IEnumerable<Tenant> GetAllList()
        {
            return _dbContext.Tb_Tenant.ToList();
        }

        public Tenant GetById(int id)
        {
            return _dbContext.Tb_Tenant.Find(id);
        }

        public void Insert(Tenant entity)
        {
            _dbContext.Add(entity);
        }

        public void Update(Tenant entity)
        {
            var tenant = _dbContext.Tb_Tenant.Find(entity.Id);
            if (tenant == null)
                throw new Exception("Tenant not found");

            if ((entity.Name != null) &&
               (entity.Host != null))
                _dbContext.Entry(entity).State = EntityState.Modified;
            else
            {
                if (entity.Name != null)
                    _dbContext.Entry(entity).Property("Name").IsModified = true;

                if (entity.Host != null)
                    _dbContext.Entry(entity).Property("Host").IsModified = true;

            }
        }

    }

}