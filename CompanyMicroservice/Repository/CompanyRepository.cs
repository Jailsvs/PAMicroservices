using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using CompanyMicroservice.DBContexts;
using CompanyMicroservice.Models;
using System.Linq.Expressions;

namespace CompanyMicroservice.Repository
{
    public class CompanyRepository: ICompanyRepository
    {
        private readonly CompanyContext _dbContext;

        public CompanyRepository(CompanyContext dbContext)
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
            return _dbContext.Tb_Company.Count();
        }

        public void Delete(Func<Company, bool> predicate)
        {
            _dbContext.Set<Company>()
            .Where(predicate).ToList()
            .ForEach(del => _dbContext.Set<Company>().Remove(del));
        }

        public void Delete(int id)
        {
            var company = _dbContext.Tb_Company.Find(id);
            if (company == null)
                throw new Exception("Company not found");
            _dbContext.Tb_Company.Remove(company);
        }

        public void Dispose()
        {
            if (_dbContext != null)
            {
                _dbContext.Dispose();
            }
            GC.SuppressFinalize(this);
        }


        public Company Find(params object[] key)
        {
            return _dbContext.Set<Company>().Find(key);
        }

        public Company First(Expression<Func<Company, bool>> predicate)
        {
            return _dbContext.Set<Company>().Where(predicate).FirstOrDefault();
        }

        public IQueryable<Company> Get(Expression<Func<Company, bool>> predicate)
        {
            return _dbContext.Set<Company>().Where(predicate);
        }

        public IQueryable<Company> GetAll()
        {
            return _dbContext.Set<Company>();
        }

        public IEnumerable<Company> GetAllList()
        {
            return _dbContext.Tb_Company.ToList();
        }

        public Company GetById(int id)
        {
            return _dbContext.Tb_Company.Find(id);
        }

        public void Insert(Company entity)
        {
            _dbContext.Add(entity);
        }

        public void Update(Company entity)
        {
            var company = _dbContext.Tb_Company.Find(entity.Id);
            if (company == null)
                throw new Exception("Company not found");

            if ((entity.Name != null) &&
               (entity.CNPJ != null) &&
               (entity.Email != null) &&
               (entity.Whats != null))
                _dbContext.Entry(entity).State = EntityState.Modified;
            else
            {
                if (entity.Name != null)
                    _dbContext.Entry(entity).Property("Name").IsModified = true;

                if (entity.CNPJ != null)
                    _dbContext.Entry(entity).Property("CNPJ").IsModified = true;

                if (entity.Email != null)
                    _dbContext.Entry(entity).Property("Email").IsModified = true;

                if (entity.Whats != null)
                    _dbContext.Entry(entity).Property("Whats").IsModified = true;
            }
    }

    }

}
