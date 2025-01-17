using Fitamas.Main;

internal class Program
{
    private static void Main(string[] args)
    {
        using var game = new GameMain(new GameEntryPoint());
        game.Run();
    }
}