﻿﻿using System.Globalization;
using Database.Data;
using Database.Models;
using Microsoft.Extensions.Configuration;

namespace Database
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // IConfiguration look for the ConnectionString in the json file we specify so it is importance 
            // to spicifical name the key as ConnectionString
            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build(); // it will return the configuration
            DataContextDapper dapper = new DataContextDapper(config);
            DataContextEF entityFramework = new DataContextEF(config);

            DateTime rightNow = dapper.LoadDataSingle<DateTime>("SELECT GETDATE()");

            // Console.WriteLine(rightNow.ToString());
            
            Computer myComputer = new Computer() 
            {
                ComputerId = 0,
                Motherboard = "Z690",
                HasWifi = true,
                HasLTE = false,
                ReleaseDate = DateTime.Now,
                Price = 943.87m,
                VideoCard = "RTX 2060"
            };

            Console.WriteLine(myComputer.ComputerId);

            entityFramework.Add(myComputer);
            entityFramework.SaveChanges();

            string sql = @"INSERT INTO TutorialAppSchema.Computer (
                Motherboard,
                HasWifi,
                HasLTE,
                ReleaseDate,
                Price,
                VideoCard
            ) VALUES ('" + myComputer.Motherboard 
                    + "','" + myComputer.HasWifi
                    + "','" + myComputer.HasLTE
                    + "','" + myComputer.ReleaseDate.ToString("yyyy-MM-dd")
                    + "','" + myComputer.Price.ToString("0.00", CultureInfo.InvariantCulture)
                    + "','" + myComputer.VideoCard
            + "')";

            // Console.WriteLine(sql);

            // int result = dapper.ExecuteSqlWithRowCount(sql);
            bool result = dapper.ExecuteSql(sql);

            // Console.WriteLine(result);

            string sqlSelect = @"
            SELECT 
                Computer.ComputerId,
                Computer.Motherboard,
                Computer.HasWifi,
                Computer.HasLTE,
                Computer.ReleaseDate,
                Computer.Price,
                Computer.VideoCard
             FROM TutorialAppSchema.Computer";

            IEnumerable<Computer> computers = dapper.LoadData<Computer>(sqlSelect);

            Console.WriteLine("'ComputerId','Motherboard','HasWifi','HasLTE','ReleaseDate'" 
                + ",'Price','VideoCard'");
            foreach(Computer singleComputer in computers)
            {
                Console.WriteLine("'" + singleComputer.ComputerId 
                    + "','" + singleComputer.Motherboard
                    + "','" + singleComputer.HasWifi
                    + "','" + singleComputer.HasLTE
                    + "','" + singleComputer.ReleaseDate.ToString("yyyy-MM-dd")
                    + "','" + singleComputer.Price.ToString("0.00", CultureInfo.InvariantCulture)
                    + "','" + singleComputer.VideoCard + "'");
            }

            IEnumerable<Computer>? computersEf = entityFramework.Computer?.ToList<Computer>();

            if (computersEf != null)
            {
                Console.WriteLine("'ComputerId','Motherboard','HasWifi','HasLTE','ReleaseDate'" 
                    + ",'Price','VideoCard'");
                foreach(Computer singleComputer in computersEf)
                {
                    Console.WriteLine("'" + singleComputer.ComputerId 
                        + "','" + singleComputer.Motherboard
                        + "','" + singleComputer.HasWifi
                        + "','" + singleComputer.HasLTE
                        + "','" + singleComputer.ReleaseDate.ToString("yyyy-MM-dd")
                        + "','" + singleComputer.Price.ToString("0.00", CultureInfo.InvariantCulture)
                        + "','" + singleComputer.VideoCard + "'");
                }
            }

            // myComputer.HasWifi = false;
            // Console.WriteLine(myComputer.Motherboard);
            // Console.WriteLine(myComputer.HasWifi);
            // Console.WriteLine(myComputer.ReleaseDate);
            // Console.WriteLine(myComputer.VideoCard);
        }

    }
}