using KynajEngine;

public class Program
{
    static void Main(string[] args)
    {
        while (true)
        {
            // this is what the engine recieves from gui
            string input = Console.ReadLine();

            string[] tokens = input.Split();

            UciHandler.handle(tokens);
         }
    }
}