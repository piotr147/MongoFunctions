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

namespace MongoFunctions.Functions
{
    public class GetSetFunction
    {
        private readonly IDbContext<LegoSet> _dbContext;

        public GetSetFunction(IDbContext<LegoSet> dbContext)
        {
            _dbContext = dbContext;
        }

        [FunctionName("GetSetFunction")]
        public async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string number = req.Query["number"];

            try
            {
                //LegoSet set = await _dbContext.GetFirstAsync(s => s.Number == number);
                var set = await _dbContext.GetAllAsync();
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
    }
}
