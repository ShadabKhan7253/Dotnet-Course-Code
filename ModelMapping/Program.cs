using System;
using System.Data;
using System.Text.Json;
using System.Text.RegularExpressions;
using AutoMapper;
using Dapper;
using ModelMapping.Data;
using ModelMapping.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace ModelMapping
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            DataContextDapper dapper = new DataContextDapper(config);

            string computersJson = File.ReadAllText("ComputersSnake.json");

            Mapper mapper = new Mapper(new MapperConfiguration((cfg) => {
                cfg.CreateMap<ComputerSnake, Computer>()
                    .ForMember(destination => destination.ComputerId, options =>
                        options.MapFrom(source => source.computer_id))
                    .ForMember(destination => destination.CPUCores, options =>
                        options.MapFrom(source => source.cpu_cores))
                    .ForMember(destination => destination.HasLTE, options =>
                        options.MapFrom(source => source.has_lte))
                    .ForMember(destination => destination.HasWifi, options =>
                        options.MapFrom(source => source.has_wifi))
                    .ForMember(destination => destination.Motherboard, options =>
                        options.MapFrom(source => source.motherboard))
                    .ForMember(destination => destination.VideoCard, options =>
                        options.MapFrom(source => source.video_card))
                    .ForMember(destination => destination.ReleaseDate, options =>
                        options.MapFrom(source => source.release_date))
                    .ForMember(destination => destination.Price, options =>
                        options.MapFrom(source => source.price));
            }));

            IEnumerable<ComputerSnake>? computersSystem = System.Text.Json.JsonSerializer.Deserialize<IEnumerable<ComputerSnake>>(computersJson);

            if (computersSystem != null)
            {
                IEnumerable<Computer> computerResult = mapper.Map<IEnumerable<Computer>>(computersSystem);
                foreach (Computer computer in computerResult)
                {
                    Console.WriteLine(computer.Motherboard);
                }
            }

        } 

    }
}