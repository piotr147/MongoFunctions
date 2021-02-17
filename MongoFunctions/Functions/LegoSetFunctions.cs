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

namespace MongoFunctions.Functions
{
    public class LegoSetFunctions
    {
        private readonly IDbContext<LegoSet> _dbContext;

        public LegoSetFunctions(IDbContext<LegoSet> dbContext)
        {
            _dbContext = dbContext;
        }

        [FunctionName("GetSet")]
        public async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            Func<LegoSet, bool> filters = GetFiltersFromQuery(req.Query);

            try
            {
                LegoSet set = await _dbContext.GetFirstAsync(s => filters(s));
                string json = JsonConvert.SerializeObject(set, Formatting.Indented);

                return new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(json, Encoding.UTF8, "application/json")
                };
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }

        private static Func<LegoSet, bool> GetFiltersFromQuery(IQueryCollection query)
        {
            Func<LegoSet, bool> filters = s => true;

            filters = string.IsNullOrWhiteSpace(query["number"])
                ? filters
                : s => filters(s) && s.Number == query["number"];

            filters = string.IsNullOrWhiteSpace(query["series"])
                ? filters
                : s => filters(s) && s.Number == query["series"];

            filters = string.IsNullOrWhiteSpace(query["year"])
                ? filters
                : s => filters(s) && s.Number == query["year"];

            filters = string.IsNullOrWhiteSpace(query["name"])
                ? filters
                : s => filters(s) && s.Number == query["name"];

            return s => filters(s);
        }
    }
}
