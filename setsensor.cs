using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.IO;

namespace Company.Function
{
    public static class setsensor
    {
        // Visit https://aka.ms/sqlbindingsoutput to learn how to use this output binding
        [FunctionName("setsensor")]
         public static CreatedResult Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "sensordata")] HttpRequest req,
            [Sql("[dbo].[sensor_data]", ConnectionStringSetting = "SqlConnectionString")] out SensorData output,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger with SQL Output Binding function processed a request.");

            string requestBody = new StreamReader(req.Body).ReadToEnd();

            output = new SensorData();
            SensorDataBody data = JsonConvert.DeserializeObject<SensorDataBody>(requestBody);
            output.vin = data.VIN;
            output.sensor_id = data.SID;
            output.datetime = data.TIME;
            
            if (data.TIRE.Equals("FR")) {
                output.position = 1;
            }else if (data.TIRE.Equals("FL")){
                output.position = 2;
            }else if (data.TIRE.Equals("RR")){
                output.position = 3;
            }else if (data.TIRE.Equals("RL")){
                output.position = 4;
            }else{
                output.position = 0;
            }

            output.latitude = data.LAT;
            output.longitude = data.LON;
            output.pressure = data.PRES;
            output.temperature = data.TEMP;

            return new CreatedResult($"/api/sensordata", output);
        }
    }

    public class SensorData
    {
        public string vin { get; set; }
        public string sensor_id { get; set; }
        public DateTime datetime { get; set; }
        public int position {get;set;}
        public decimal pressure {get;set;}
        public decimal temperature {get;set;}
        public decimal latitude {get;set;}
        public decimal longitude {get;set;}
    }

    public class SensorDataBody
    {
        public string VIN { get; set; }
        public string SID { get; set; }
        public string TIRE { get; set; }
        public decimal PRES { get; set; }
        public decimal TEMP { get; set; }
        public DateTime TIME { get; set; }
        public decimal LAT { get; set; }
        public decimal LON { get; set; }
    }
}
