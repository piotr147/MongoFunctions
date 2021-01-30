using MongoDB.Driver;
using MongoFunctions.Helpers;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MongoFunctions.Db
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
    }
}
