namespace Benchmarks.App;

internal static class ConsoleWriter
{
    private const string Header =
        """
        ______  _______ __    _ _______ _     _ _______ _______ _______ _     _ ________
        |_____| |______ | \\  | |       |_____| |  |  | |_____| |_____/ |____/  |______
        |_____| |______ |  \\_| |______ |     | |  |  | |     | |    \\ |    \\ _______|

        """;
    private static readonly Rainbow Lolcat = new(new RainbowStyle(EscapeSequence.Spectre));
    private static readonly AnimationStyle AnimationStyle = new(
        Duration: TimeSpan.FromSeconds(.42),
        PadToWindowSize: false,
        Speed: 42,
        StopOnResize: true);

    public static void WriteHeader(bool clearConsole = false)
    {
        if (clearConsole)
        {
            AnsiConsole.Clear();
        }

        Lolcat.WriteLineWithMarkup(Header);
    }

    public static void AnimateHeader()
    {
        AnsiConsole.Clear();

        Lolcat.Animate(Header, AnimationStyle);

        AnsiConsole.WriteLine();
    }

    public static void WaitForKeyPress()
    {
        AnsiConsole.WriteLine();

        AnsiConsole.MarkupLine("[gray](Press any key to return to main menu)[/]");

        AnsiConsole.Cursor.Hide();

        Console.ReadKey();

        AnsiConsole.Cursor.Show();
    }
}
