using System;
using System.Text.RegularExpressions;

namespace HelloWorld
{
    internal class Program
    {
        static int a = 5;
        static int AccessibleInt = 7;
        static void Main(string[] args)
        {
            int b = 15;
            int accessibleInt = 5;
            Console.WriteLine(accessibleInt);
            Console.WriteLine(AccessibleInt);
            Console.WriteLine(Add(a,b));
        }

        static private int Add(int a, int b)
        {
            return a + b;
        }
    }
}