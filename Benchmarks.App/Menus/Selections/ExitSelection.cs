namespace Benchmarks.App.Menus.Selections;

internal sealed record ExitSelection : Selection
{
    internal ExitSelection(int order) : base("Exit", order)
    {
    }

    public override int Execute()
    {
        if (ExitApp)
        {
            ConsoleWriter.AnimateHeader();
        }

        return 0;
    }

    public bool ExitApp { get; init; }
}