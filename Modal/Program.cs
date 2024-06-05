using System;
using System.Text.RegularExpressions;

namespace HelloWorld
{
    public class Computer
    {
        // private string _motherboard;
        // public string Motherboard { get {return _motherboard;} set{_motherboard = value;} }
        // behind the scene C# will automatically create the private field and use get to get the 
        // private field and set to set the private field so it automatically take care of in the 
        // background

        public string Motherboard { get; set; }
        public int CPUCores { get; set; }
        public bool HasWifi { get; set; }
        public bool HasLTE { get; set; }
        public DateTime ReleaseDate { get; set; }
        public decimal Price { get; set; }
        public string VideoCard { get; set; }

        public Computer()
        {
            if (VideoCard == null)
            {
                VideoCard = "";
            }
            if (Motherboard == null)
            {
                Motherboard = "";
            }
        }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            Computer myComputer = new Computer() 
            {
                Motherboard = "Z690",
                HasWifi = true,
                HasLTE = false,
                ReleaseDate = DateTime.Now,
                Price = 943.87m,
                VideoCard = "RTX 2060"
            };
            myComputer.HasWifi = false;
            Console.WriteLine(myComputer.Motherboard);
            Console.WriteLine(myComputer.HasWifi);
            Console.WriteLine(myComputer.ReleaseDate);
            Console.WriteLine(myComputer.VideoCard);
        }

    }
}