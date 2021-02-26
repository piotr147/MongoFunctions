using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Shared.Helpers;
using Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Shared.Db
{
    public class DbContext<TDocument> : IDbContext<TDocument>
        where TDocument : IDocument
    {
        private readonly IMongoDatabase _database;
        private readonly IMongoCollection<TDocument> _collection;

        public DbContext(IConfigurationReader confiugrationReader)
        {
            MongoClient client = new MongoClient(confiugrationReader.GetConnectionString());
            _database = client.GetDatabase(confiugrationReader.GetDatabaseName());
            _collection = _database.GetCollection<TDocument>(GetCollectionName(typeof(TDocument)));
        }

        private string GetCollectionName(Type documentType) =>
            ((BsonCollectionAttribute)documentType.GetCustomAttributes(
                    typeof(BsonCollectionAttribute),
                    true)
                .FirstOrDefault())?.CollectionName;

        public async Task<List<TDocument>> FilterBy(Expression<Func<TDocument, bool>> pred) =>
            await (await _collection.FindAsync(pred)).ToListAsync();

        public async Task<TDocument> GetFirstAsync(Expression<Func<TDocument, bool>> pred) =>
            await (await _collection.FindAsync(pred)).FirstOrDefaultAsync();

        public async Task<List<TDocument>> GetAllAsync() =>
            (await _collection.FindAsync(_ => true)).ToList();

        public async Task<List<TDocument>> GetAllAsync(Expression<Func<TDocument, bool>> pred) =>
            (await _collection.FindAsync(pred)).ToList();

        public async Task InsertOneAsync(TDocument item) =>
            await _collection.InsertOneAsync(item);

        public async Task InsertManyAsync(IEnumerable<TDocument> items) =>
            await _collection.InsertManyAsync(items);

        public async Task UpsertOneAsync(Expression<Func<TDocument, bool>> pred, TDocument item)
        {
            await _collection.DeleteOneAsync(pred);
            await _collection.InsertOneAsync(item);
        }

        //public async Task UpsertManyAsync(Func<T, T, bool> pred, IEnumerable<T> items)
        //{
        //    foreach (var item in items)
        //    {
        //        _collection.AsQueryable().Where(t => pred(t, item))

        //    }
        //    await _collection.InsertManyAsync(items);
        //}
    }
}
