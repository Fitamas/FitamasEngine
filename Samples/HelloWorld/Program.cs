namespace Fitamas.Samples.HelloWorld
{
    static class Program
    {
        static void Main(string[] args)
        {
            using var game = new HelloWorldGame();
            game.Run();

        }
    }
}