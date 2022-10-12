using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace Company.Function
{
    public static class healthylist
    {
        // Visit https://aka.ms/sqlbindingsinput to learn how to use this input binding
    [FunctionName("healthylist")]
         public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            [Sql("select vin, avg(pressure) as pressure, avg(temperature) as temperature, avg(latitude) as latitude, avg(longitude) as longitude "
            +"from [dbo].[sensor_data] "
            +"where datetime in (select max(datetime) from [dbo].[sensor_data] group by vin) "
            +"group by vin",
            CommandType = System.Data.CommandType.Text,
            ConnectionStringSetting = "SqlConnectionString")] IEnumerable<VinData> result,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger with SQL Input Binding function processed a request.");

            var tempHealthyCount = 0;
            var tempUnhealthyCount = 0;
            var pressHealthyCount = 0;
            var pressUnHealthyCount = 0;

            foreach (var item in result){
                if (item.pressure >= 220 && item.pressure < 270){
                    pressHealthyCount++;
                }else { 
                    pressUnHealthyCount++;
                }

                if (item.temperature >= 30 && item.temperature < 80){
                    tempHealthyCount++;
                }else { 
                    tempUnhealthyCount++;
                }
            }

            var jresult = new {
                summary = new[]{
                    new {
                        type = "Pressure",
                        healthy = pressHealthyCount,
                        unhealthy = pressUnHealthyCount,
                        nodata = 0
                    },
                    new {
                        type = "Temperature",
                        healthy = tempHealthyCount,
                        unhealthy = tempUnhealthyCount,
                        nodata = 0
                    }
                }
            };

            return new OkObjectResult(jresult);
        }
    }

}
