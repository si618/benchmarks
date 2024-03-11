namespace Benchmarks.App.Menus;

internal sealed class MainMenu : MenuBase
{
    public MainMenu()
    {
        Choices = new List<Selection>
        {
            new ListSelection(1),
            new RunAllSelection(2),
            new AboutSelection(3),
            new ExitSelection(4) { ExitApp = true }
        };
    }

    public override int Render()
    {
        var selected = Choices.First();

        var prompt = new SelectionPrompt<Selection>()
            .AddChoices(GetChoices())
            .UseConverter(m => m.Name);

        var exitCode = 0;

        while (selected is not ExitSelection { ExitApp: true })
        {
            ConsoleWriter.WriteHeader(clearConsole: true);

            selected = AnsiConsole.Prompt(prompt);

            exitCode = selected.Execute();
        }

        return exitCode;
    }
}
