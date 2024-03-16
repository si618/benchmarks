namespace Benchmarks.App.Menus.Selections;

internal sealed record AboutSelection : Selection
{
    internal AboutSelection(int order) : base("About", order)
    {
    }

    public override int Execute()
    {
        ConsoleWriter.WriteHeader(clearConsole: true);

        AnsiConsole.WriteLine(
            """
            Performance benchmarks for various things of interest

            Written by Simon McKenna
            """);

        ConsoleWriter.WaitForKeyPress();

        return 0;
    }
}
