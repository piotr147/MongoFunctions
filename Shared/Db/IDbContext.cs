using MongoDB.Driver;
using Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Shared.Db
{
    public interface IDbContext<TDocument>
        where TDocument : IDocument
    {
        public Task<List<TDocument>> FilterBy(Expression<Func<TDocument, bool>> pred);

        public Task<TDocument> GetFirstAsync(Expression<Func<TDocument, bool>> pred);

        public Task<List<TDocument>> GetAllAsync();

        public  Task<List<TDocument>> GetAllAsync(Expression<Func<TDocument, bool>> pred);

        public  Task InsertOneAsync(TDocument item);

        public  Task InsertManyAsync(IEnumerable<TDocument> items);

        public Task UpsertOneAsync(Expression<Func<TDocument, bool>> pred, TDocument item);
    }
}
