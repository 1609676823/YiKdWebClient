using System.Text;

namespace ConsoleTestNet80;

internal static class Program
{
    private static int Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;

        if (args.Length == 0 || IsHelpCommand(args[0]))
        {
            ExampleCatalog.PrintHelp();
            return 0;
        }

        if (!ExampleCatalog.TryGet(args[0], out ExampleDefinition? example) || example == null)
        {
            Console.Error.WriteLine($"未知示例命令：{args[0]}");
            Console.Error.WriteLine();
            ExampleCatalog.PrintHelp();
            return 1;
        }

        ConsoleReport.PrintExampleHeader(example);

        try
        {
            example.Run();
            ConsoleReport.PrintExampleFooter(processCompleted: true);
            return 0;
        }
        catch (Exception exception)
        {
            ConsoleReport.PrintUnhandledException(exception);
            ConsoleReport.PrintExampleFooter(processCompleted: false);
            return 2;
        }
    }

    private static bool IsHelpCommand(string value)
    {
        return value.Equals("help", StringComparison.OrdinalIgnoreCase)
            || value.Equals("--help", StringComparison.OrdinalIgnoreCase)
            || value.Equals("-h", StringComparison.OrdinalIgnoreCase);
    }
}
