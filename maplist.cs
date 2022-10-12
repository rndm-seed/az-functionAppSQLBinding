using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace Company.Function
{
    public static class maplist
    {
        // Visit https://aka.ms/sqlbindingsinput to learn how to use this input binding
    [FunctionName("maplist")]
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

            var resultList = new List<VinDataReturn>();  
            foreach (var item in result){
                var obj = new VinDataReturn();
                obj.vin = item.vin;
                obj.latitude = item.latitude;
                obj.longitude = item.longitude;
                if (item.pressure >= 220 && item.pressure < 270){
                    obj.pressure = "healthy";
                }else { 
                    obj.pressure = "unhealthy";
                }

                if (item.temperature >= 30 && item.temperature < 80){
                    obj.temperature = "healthy";
                }else { 
                    obj.temperature = "unhealthy";
                }
                resultList.Add(obj);
            }

            var jresult = new {
                carList = resultList
            };

            return new OkObjectResult(jresult);
        }
    }

    public class VinData
    {
        public string vin { get; set; }
        public double pressure { get; set; }
        public double temperature { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
    }

    public class VinDataReturn
    {
        public string vin { get; set; }
        public string pressure { get; set; }
        public string temperature { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
    }
}
