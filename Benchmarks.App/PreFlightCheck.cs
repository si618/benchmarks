﻿namespace Benchmarks.App;

internal static class PreFlightCheck
{
    internal static void CanStartTestContainers()
    {
        ConsoleWriter.WriteHeader(clearConsole: false);

        AnsiConsole.MarkupLine("[gray]Running pre-flight check to verify test containers can start[/]");
        AnsiConsole.WriteLine();

        // Would be nice if ContainerBuilder.Validate method was public
        new ContainerBuilder()
            .WithImage("testcontainers/helloworld:latest")
            .WithPortBinding(8080, true)
            .WithWaitStrategy(Wait.ForUnixContainer()
                .UntilHttpRequestIsSucceeded(r => r.ForPort(8080)))
            .Build();

        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine("[gray]Test container pre-flight check passed[/]");

        Thread.Sleep(TimeSpan.FromSeconds(1.66));
    }
}
