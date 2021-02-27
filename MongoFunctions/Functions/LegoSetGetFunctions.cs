using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net;
using System.Text;
using System;
using Shared.Db;
using Shared.Models;
using System.Linq.Expressions;
using System.Collections.Generic;

namespace MongoFunctions.Functions
{
    public class LegoSetGetFunctions
    {
        private readonly IDbContext<LegoSet> _dbContext;

        public LegoSetGetFunctions(IDbContext<LegoSet> dbContext)
        {
            _dbContext = dbContext;
        }

        [FunctionName("GetSet")]
        public async Task<HttpResponseMessage> GetSet(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            Expression<Func<LegoSet, bool>> filters = GetFiltersForSetFromQuery(req.Query);

            try
            {
                return await TryGetSet(filters);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }

        private async Task<HttpResponseMessage> TryGetSet(Expression<Func<LegoSet, bool>> filters)
        {
            LegoSet set = await _dbContext.GetFirstAsync(filters);
            string json = JsonConvert.SerializeObject(set, Formatting.Indented);

            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };
        }

        [FunctionName("GetSets")]
        public async Task<HttpResponseMessage> GetSets(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            Expression<Func<LegoSet, bool>> filters = GetFiltersForSetFromQuery(req.Query);

            try
            {
                return await TryGetSets(filters);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }

        private async Task<HttpResponseMessage> TryGetSets(Expression<Func<LegoSet, bool>> filters)
        {
            List<LegoSet> sets = await _dbContext.FilterBy(filters);
            string json = JsonConvert.SerializeObject(sets, Formatting.Indented);

            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };
        }

        private static Expression<Func<LegoSet, bool>> GetFiltersForSetFromQuery(IQueryCollection query)
        {
            bool numberEmpty= query["number"].Count == 0;
            bool seriesEmpty= query["series"].Count == 0;
            bool yearEmpty= query["year"].Count == 0;

            return s =>
                (numberEmpty || s.Number == query["number"])
                && (seriesEmpty || s.Series == query["series"])
                && (yearEmpty || s.Year == query["year"]);
        }
    }
}
