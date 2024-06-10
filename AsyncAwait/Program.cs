namespace AsyncAwait
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Task firstTask = new Task(() =>
            {
                Thread.Sleep(100);
                Console.WriteLine("Task 1");
            });
            firstTask.Start();

            Task secondTask = ConsoleAfterDelayAsync("Task 2", 150);

            ConsoleAfterDelay("Delay", 75);

            Task thirdTask = ConsoleAfterDelayAsync("Task 3", 50);

            await firstTask;
            Console.WriteLine("After task was created");

            await secondTask;
            await thirdTask;

        }
        static void ConsoleAfterDelay(string text, int delayTime)
        {
            Thread.Sleep(delayTime);
            Console.WriteLine(text);
        }

        static async Task ConsoleAfterDelayAsync(string text, int delayTime)
        {
            /* It will also wait but it is not asynchronous so the methd will throw a warning that async method lack await */
            /*Thread.Sleep(delayTime);*/

            /* Task.Delay() is same thing as Thread.Sleep() but it is async so that we have ability to await  */

            await Task.Delay(delayTime);
            Console.WriteLine(text);
        }
    }
}