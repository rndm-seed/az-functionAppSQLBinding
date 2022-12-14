using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace Company.Function
{
    public static class getprovinces
    {
        // Visit https://aka.ms/sqlbindingsinput to learn how to use this input binding
    [FunctionName("getprovinces")]
         public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "provinces")] HttpRequest req,
            [Sql("SELECT * FROM [dbo].[master_provinces]",
            CommandType = System.Data.CommandType.Text,
            ConnectionStringSetting = "SqlConnectionString")] IEnumerable<Object> result,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger with SQL Input Binding function processed a request.");

            var jresult = new {
                provinces = result
            };

            return new OkObjectResult(jresult);
        }
    }
}
