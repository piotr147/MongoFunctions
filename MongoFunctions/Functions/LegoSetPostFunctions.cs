using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Shared.Db;
using Shared.Models;
using MongoDB.Driver.Core.WireProtocol.Messages.Encoders;
using System.Collections.Generic;

namespace MongoFunctions.Functions
{
    public class LegoSetPostFunctions
    {
        private readonly IDbContext<LegoSet> _dbContext;

        public LegoSetPostFunctions(IDbContext<LegoSet> dbContext)
        {
            _dbContext = dbContext;
        }

        [FunctionName("PostSet")]
        public async Task<IActionResult> PostSet(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            string body = await new StreamReader(req.Body).ReadToEndAsync();

            try
            {
                return await TryPostSet(body);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }

        private async Task<IActionResult> TryPostSet(string body)
        {
            LegoSet set = JsonConvert.DeserializeObject<LegoSet>(body);
            await _dbContext.UpsertOneAsync(s => s.Number == set.Number, set);

            return new OkResult();
        }

        [FunctionName("PostSets")]
        public async Task<IActionResult> PostSets(
            [HttpTrigger(AuthorizationLevel.Function, "posts", Route = null)] HttpRequest req,
            ILogger log)
        {
            string body = await new StreamReader(req.Body).ReadToEndAsync();

            try
            {
                return await TryPostSets(body);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }

        private async Task<IActionResult> TryPostSets(string body)
        {
            IEnumerable<LegoSet> sets = JsonConvert.DeserializeObject<IEnumerable<LegoSet>>(body);
            foreach (var set in sets)
            {
                await _dbContext.UpsertOneAsync(s => s.Number == set.Number, set);
            }

            return new OkResult();
        }
    }
}
