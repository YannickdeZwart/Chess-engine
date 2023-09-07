using KynajEngine;

public class Program
{
    static void Main(string[] args)
    {
        UciHandler uciHandler = new UciHandler();

        while (true)
        {
            // this is what the engine recieves from gui
            string input = Console.ReadLine();

            string[] tokens = input.Split();

            uciHandler.handle(tokens);
         }
    }
}