using MongoDB.Driver;
using Shared.Helpers;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Shared.Db
{
    public class DbContext<T> : IDbContext<T>
    {
        private readonly IMongoDatabase _database;
        private readonly IMongoCollection<T> _collection;

        public DbContext(IConfigurationReader confiugrationReader)
        {
            MongoClient client = new MongoClient(confiugrationReader.GetConnectionString());
            _database = client.GetDatabase(confiugrationReader.GetDatabaseName());
            _collection = _database.GetCollection<T>(typeof(T).Name);
        }

        public async Task<T> GetFirstAsync(Expression<Func<T, bool>> pred) =>
            await (await _collection.FindAsync(pred)).FirstOrDefaultAsync();

        public async Task<List<T>> GetAllAsync() =>
            (await _collection.FindAsync(_ => true)).ToList();

        public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>> pred) =>
            (await _collection.FindAsync(pred)).ToList();

        public async Task InsertOneAsync(T item) =>
            await _collection.InsertOneAsync(item);

        public async Task InsertManyAsync(IEnumerable<T> items) =>
            await _collection.InsertManyAsync(items);
    }
}
