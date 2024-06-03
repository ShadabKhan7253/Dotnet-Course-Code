// See https://aka.ms/new-console-template for more information

using System;

namespace DataStructure
{
    internal class Program
    {
        static void Main(string[] args)
        {   
            Console.WriteLine("========== Array ==========");
            string[] myFoodArray = new string[2];
            myFoodArray[0] = "Apple";
            Console.WriteLine(myFoodArray[0]);
            Console.WriteLine(myFoodArray[1]);
            // Console.WriteLine(myFoodArray[2]); // error IndexOutOfBound

            string[] mySecondFoodArray = {"Apple","Banana","Mango"};
            Console.WriteLine(mySecondFoodArray[0]);
            Console.WriteLine(mySecondFoodArray[1]);
            Console.WriteLine(mySecondFoodArray[2]);

            Console.WriteLine("========== List ==========");

            List<string> myFoodList = new List<string>() {"Grape","Orange"};
            Console.WriteLine(myFoodList[0]);
            Console.WriteLine(myFoodList[1]);
            
            // Console.WriteLine(myFoodList[2]);

            myFoodList.Add("Pine Apple");
            Console.WriteLine(myFoodList[2]);

            Console.WriteLine("========== IEnumerable ==========");
            IEnumerable<string> myFoodIEnumerable = myFoodList;
            // Console.WriteLine(myFoodIEnumerable[0]); // Error : indexing can't be used here
            Console.WriteLine(myFoodIEnumerable.First());

            Console.WriteLine("========== Two Dimension Array ==========");
            string [,] myTwoDimensionArray = new string[,] {
                {"Apple","Banana"},
                {"Grape","Orange"}
            };

            Console.WriteLine(myTwoDimensionArray[0,1]);

            Console.WriteLine("========== Dictionery ==========");
            Dictionary<string,string> myFoodDistionary = new Dictionary<string, string>{
                {"Food1" , "Papaya" }
            };
            Console.WriteLine(myFoodDistionary["Food1"]);

            Dictionary<string,string[]> myFoodDistionaryArray = new Dictionary<string,string[]>(){
                {"Food1" , new string[]{"Papaya","Mango"} }
            };
            Console.WriteLine(myFoodDistionaryArray["Food1"][1]);



        }
    }
}
