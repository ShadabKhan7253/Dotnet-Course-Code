using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Numerics;
using Database.Models;
using Database.Data;
using Microsoft.Data.SqlClient;
using Dapper;


namespace DataStructure
{
    internal class Program
    {
        static void Main(string[] args)
        {   
            
            DataContextDapper dapper = new DataContextDapper();

            string sqlCommand = "SELECT GETDATE()";

            DateTime rightNow = dapper.LoadDataSingle<DateTime>(sqlCommand);

            Console.WriteLine(rightNow);

            Computer myComputer = new Computer() 
            {
                Motherboard = "Z690",
                HasWifi = true,
                HasLTE = false,
                ReleaseDate = DateTime.Now,
                Price = 943.87m,
                VideoCard = "RTX 2060"
            };

            string sql = @"INSERT INTO TutorialAppSchema.Computer (
                Motherboard,
                HasWifi,
                HasLTE,
                ReleaseDate,
                Price,
                VideoCard
            ) VALUES ('" + myComputer.Motherboard
             + "', '" + myComputer.HasWifi 
             + "', '" + myComputer.HasLTE 
             + "', '" + myComputer.ReleaseDate 
             + "', '" + myComputer.Price 
             + "', '" + myComputer.VideoCard 
             + "')";

             Console.WriteLine(sql);
             int result = dapper.ExecuteSqlWithRowCount(sql);

             Console.WriteLine(result);

             string sqlSelect = @"
             SELECT 
                Computer.Motherboard,
                Computer.HasWifi,
                Computer.HasLTE,
                Computer.ReleaseDate,
                Computer.Price,
                Computer.VideoCard
             FROM TutorialAppSchema.Computer";

             IEnumerable<Computer> computers = dapper.LoadData<Computer>(sqlSelect);

             foreach(Computer singleComputer in computers) 
             {
                Console.WriteLine("'" + myComputer.Motherboard
                    + "', '" + myComputer.HasWifi 
                    + "', '" + myComputer.HasLTE 
                    + "', '" + myComputer.ReleaseDate 
                    + "', '" + myComputer.Price 
                    + "', '" + myComputer.VideoCard 
                    + "'");
             } 
        }
    }
}
