using SharedMicroservice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SharedMicroservice.Repository
{
    public interface IRepository<T> where T:AncestorTb
    {
        IQueryable<T> GetAll();
        IEnumerable<T> GetAllList();
        IQueryable<T> Get(Expression<Func<T, bool>> predicate);
        T Find(params object[] key);
        T GetById(int id);
        T First(Expression<Func<T, bool>> predicate);
        void Insert(T entity);
        void Update(T entity);
        void Delete(Func<T, bool> predicate);
        void Delete(int id);
        bool Commit();
        void Dispose();
        int Count();
    }
}
