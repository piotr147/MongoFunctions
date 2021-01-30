using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MongoFunctions.Db
{
    public interface IDbContext<T>
    {
        public Task<T> GetFirstAsync(Expression<Func<T, bool>> pred);
        public Task<List<T>> GetAllAsync();
    }
}
