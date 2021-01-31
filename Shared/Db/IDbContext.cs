using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Shared.Db
{
    public interface IDbContext<T>
    {
        public Task<T> GetFirstAsync(Expression<Func<T, bool>> pred);

        public Task<List<T>> GetAllAsync();

        public  Task<List<T>> GetAllAsync(Expression<Func<T, bool>> pred);

        public  Task InsertOneAsync(T item);

        public  Task InsertManyAsync(IEnumerable<T> items);
    }
}
