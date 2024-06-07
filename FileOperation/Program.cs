
namespace FileOperation
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string str = "I am writing content to he file.";

            File.WriteAllText("log.txt", "\n" + str + "\n");

            using StreamWriter writeFile = new("log.txt", append: true);

            writeFile.WriteLine("\n" + str + "\n");

            writeFile.Close();

            string readFile = File.ReadAllText("log.txt");

            Console.WriteLine(readFile);

        }

    }
}