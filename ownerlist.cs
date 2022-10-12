using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace Company.Function
{
    public static class ownerlist
    {
        // Visit https://aka.ms/sqlbindingsinput to learn how to use this input binding
    [FunctionName("ownerlist")]
         public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            [Sql("SELECT * FROM [dbo].[owners]",
            CommandType = System.Data.CommandType.Text,
            ConnectionStringSetting = "SqlConnectionString")] IEnumerable<Owner> result,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger with SQL Input Binding function processed a request.");

            foreach (Owner r in result){
                //r.trackList.Add(new Object());
            }

            var jresult = new {
                ownerList = result
            };

            return new OkObjectResult(jresult);
        }
    }


    public class Owner
    {
        public int id { get; set; }
        public string vin { get; set; }
        public string name { get; set; }
        public string phone { get; set; }
        public int district_id { get; set; }
        public string detailed_address { get; set; }
        public List<object> trackList { get; set; }
    }
}
