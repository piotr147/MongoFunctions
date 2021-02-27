using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using MongoFunctions.Helpers;
using Newtonsoft.Json;
using Shared.Db;
using Shared.Helpers;
using Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MongoFunctions.Functions
{
    public class UsersSetsGetFunctions
    {
        private readonly IDbContext<LegoSet> _setsDbContext;
        private readonly IDbContext<SetOwnership> _ownershipsDbContext;

        public UsersSetsGetFunctions(IDbContext<LegoSet> setDbContext, IDbContext<SetOwnership> ownershipDbContext)
        {
            _setsDbContext = setDbContext;
            _ownershipsDbContext = ownershipDbContext;
        }

        [FunctionName("GetMySets")]
        public async Task<HttpResponseMessage> GetMySets(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            try
            {
                return await TryGetUserSets(req.Query);
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.Message);
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }

        private async Task<HttpResponseMessage> TryGetUserSets(IQueryCollection query)
        {
            Expression<Func<SetOwnership, bool>> ownershipFilters = GetFiltersForOwnershipsFromQuery(query);
            List<SetOwnership> ownerships = await _ownershipsDbContext.FilterBy(ownershipFilters);

            List<string> usersSetsNumbers = ownerships.Select(o => o.SetNumber).ToList();
            Expression<Func<LegoSet, bool>> setFilters = GetFiltersForSetFromQueryAndOwnedSets(query, usersSetsNumbers);
            List<LegoSet> sets = await _setsDbContext.FilterBy(setFilters);

            List<OwnedSet> resultSets = ownerships.Join(sets, o => o.SetNumber, s => s.Number, (s, o) => new OwnedSet(o, s)).ToList();
            string json = JsonConvert.SerializeObject(resultSets, Formatting.Indented);

            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };
        }

        private static Expression<Func<SetOwnership, bool>> GetFiltersForOwnershipsFromQuery(IQueryCollection query)
        {
            if (query["usermail"].Count == 0)
                throw new ArgumentException("User mail is missing");

            bool numbersEmpty = query["numbers"].Count == 0;
            bool dateFromEmpty = query["datefrom"].Count == 0;
            bool dateToEmpty = query["dateto"].Count == 0;
            bool priceFromEmpty = query["pricefrom"].Count == 0;
            bool priceToEmpty = query["priceto"].Count == 0;
            bool wasBuiltEmpty = query["wasbuilt"].Count == 0;
            bool isForSaleEmpty = query["isforsale"].Count == 0;

            DateTime dateFrom = ParseHelper.GetDateTimeValueOrMin(query["datefrom"]);
            DateTime dateTo = ParseHelper.GetDateTimeValueOrMin(query["dateto"]);
            int priceFrom = ParseHelper.GetIntValueOrFalse(query["priceFrom"]); 
            int priceTo = ParseHelper.GetIntValueOrFalse(query["priceTo"]);
            bool wasBuilt = ParseHelper.GetBoolValueOrFalse(query["wasbuilt"]);
            bool isForSale = ParseHelper.GetBoolValueOrFalse(query["isforsale"]);

            string[] numbers = query["numbers"].ToString().Replace(" ", "").Split(',');


            if (query["usermail"].Count == 0)
                throw new ArgumentException("User mail is missing");

            return o =>
                (o.UserMail == query["usermail"])
                && (numbersEmpty || numbers.Contains(o.SetNumber))
                && (dateFromEmpty || o.PurchaseDate >= dateFrom)
                && (dateToEmpty || o.PurchaseDate <= dateTo)
                && (priceFromEmpty || o.TotalPrice >= priceFrom)
                && (priceToEmpty || o.TotalPrice <= priceTo)
                && (wasBuiltEmpty || o.WasBuilt == wasBuilt)
                && (isForSaleEmpty || o.IsForSale == isForSale);
        }

        private static Expression<Func<LegoSet, bool>> GetFiltersForSetFromQueryAndOwnedSets(IQueryCollection query, List<string> userSets)
        {
            bool seriesEmpty = query["series"].Count == 0;
            bool yearEmpty = query["year"].Count == 0;

            return s =>
                userSets.Contains(s.Number)
                && (seriesEmpty || s.Series == query["series"])
                && (yearEmpty || s.Year == query["year"]);
        }
    }
}
