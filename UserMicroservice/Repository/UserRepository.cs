using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using UserMicroservice.DBContexts;
using UserMicroservice.Models;
using System.Linq.Expressions;
using SharedMicroservice.Models;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace UserMicroservice.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly UserContext _dbContext;

        public UserRepository(UserContext dbContext)
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
            return _dbContext.Tb_User.Count();
        }

        public void Delete(Func<User, bool> predicate)
        {
            _dbContext.Set<User>()
            .Where(predicate).ToList()
            .ForEach(del => _dbContext.Set<User>().Remove(del));
        }

        public void Delete(int id)
        {
            var user = _dbContext.Tb_User.Find(id);
            if (user == null)
                throw new Exception("User not found");
            _dbContext.Tb_User.Remove(user);
        }

        public void Dispose()
        {
            if (_dbContext != null)
            {
                _dbContext.Dispose();
            }
            GC.SuppressFinalize(this);
        }


        public  User Find(params object[] key)
        {
            return _dbContext.Set<User>().Find(key);
        }

        public User First(Expression<Func<User, bool>> predicate)
        {
            return _dbContext.Set<User>().Where(predicate).FirstOrDefault();
        }

        public IQueryable<User> Get(Expression<Func<User, bool>> predicate)
        {
            return _dbContext.Set<User>().Where(predicate);
        }

        public IQueryable<User> GetAll()
        {
            return _dbContext.Set<User>();
        }

        public IEnumerable<User> GetAllList()
        {
            return _dbContext.Tb_User.ToList();
        }

        public User GetById(int id)
        {
            return _dbContext.Tb_User.Find(id);
        }

        public void IncreaseDecreaseBids(int userId, int bidsQuantity)
        {
            //var commandText = "UPDATE TB_USER SET AvailableBids = AvailableBids + "+ bidsQuantity + " WHERE ID = "+ userId;
            //_dbContext.Database.ExecuteSqlCommand(commandText);
            User user = _dbContext.Tb_User.Find(userId);
            if (user == null)
                throw new Exception("User not found");
            if (user != null)
            {
                user.AvailableBids += bidsQuantity;
                _dbContext.Entry(user).Property("AvailableBids").IsModified = true;
            }

            Commit();
        }

        public void Insert(User entity)
        {
            _dbContext.Add(entity);
        }


        public void Update(User entity)
        {
            var user = _dbContext.Tb_User.Find(entity.Id);
            if (user == null)
                throw new Exception("User not found");

            if ((entity.Name != null) &&
               (entity.Password != null) &&
               (entity.Whats != null))
                _dbContext.Entry(entity).State = EntityState.Modified;
            else
            {
                if (entity.Name != null)
                    _dbContext.Entry(entity).Property("Name").IsModified = true;

                if (entity.Password != null)
                    _dbContext.Entry(entity).Property("Password").IsModified = true;

                if (entity.Email != null)
                    _dbContext.Entry(entity).Property("Email").IsModified = true;

                if (entity.Whats != null)
                    _dbContext.Entry(entity).Property("Whats").IsModified = true;
            }
    }

    }

}
