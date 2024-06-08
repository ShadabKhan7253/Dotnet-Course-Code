

using Microsoft.Extensions.Configuration;
using System.Text.Json;
using JSON.Models;
using JSON.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Globalization;

namespace JSON
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            DataContextDapper dapper = new DataContextDapper(config);

            string computersJson = File.ReadAllText("Computers.json");
            /*Console.WriteLine(computersJson);*/


            // ****************** DESERAILIZATION ******************//

            /* Deserialization reconstructs an object from the serialized form .*/

         
            // This is the setting which is use to convert it into camelCase
            JsonSerializerOptions options = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            IEnumerable<Computer>? computersSystem = System.Text.Json.JsonSerializer.Deserialize<IEnumerable<Computer>>(computersJson, options);

            IEnumerable<Computer>? computersNewtsoft = JsonConvert.DeserializeObject<IEnumerable<Computer>>(computersJson);

            if (computersNewtsoft != null)
            {
                foreach (Computer computer in computersNewtsoft)
                {
                    /*Console.WriteLine(computer.Motherboard);*/
                    string sql = @"INSERT INTO TutorialAppSchema.Computer (
                        Motherboard,
                        HasWifi,
                        HasLTE,
                        ReleaseDate,
                        Price,
                        VideoCard
                    ) VALUES ('" + EscapeSingleQuote(computer.Motherboard)
                            + "','" + computer.HasWifi
                            + "','" + computer.HasLTE
                            + "','" + computer.ReleaseDate?.ToString("yyyy-MM-dd")
                            + "','" + computer.Price.ToString("0.00", CultureInfo.InvariantCulture)
                            + "','" + EscapeSingleQuote(computer.VideoCard)
                    + "')";

                    dapper.ExecuteSQL(sql);
                }
            
            }

            // ****************** SERAILIZATION ******************//

            /*Serialization is the process of converting the state of an object, that is, the values of its 
            properties, into a form that can be stored or transmitted.*/

            JsonSerializerSettings settings = new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()            
            };

            string computerCopyNewtonsoft = JsonConvert.SerializeObject(computersNewtsoft,settings);

            File.WriteAllText("computerCopyNewtonsoft.txt",computerCopyNewtonsoft);

            string computerCopySystem = System.Text.Json.JsonSerializer.Serialize(computersSystem, options);

            File.WriteAllText("computerCopySystem.txt", computerCopySystem);
        }
        static string EscapeSingleQuote(string input)
        {
            string output = input.Replace("'", "''");

            return output;
        }

    }
}