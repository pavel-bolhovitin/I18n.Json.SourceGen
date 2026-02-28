using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System.Collections.Generic;
using System.Text;

namespace I18n.Json.SourceGen;

internal class Logger
{

    public static void Error(SourceProductionContext context, string message)
    {
        var descriptor = new DiagnosticDescriptor(
            "LOG001",
            "Generator Log",
            "{0}",
            "I18n.Json.SourceGen",
            DiagnosticSeverity.Error,
            isEnabledByDefault: true);

        context.ReportDiagnostic(Diagnostic.Create(descriptor, Location.None, message));
    }

    public static void Warning(SourceProductionContext context, string message)
    {
        var descriptor = new DiagnosticDescriptor(
            "LOG001",
            "Generator Log",
            "{0}",
            "I18n.Json.SourceGen",
            DiagnosticSeverity.Warning,
            isEnabledByDefault: true);

        context.ReportDiagnostic(Diagnostic.Create(descriptor, Location.None, message));
    }

    //private static readonly string LogPath = Path.Combine(Path.GetTempPath(), "_RoslynLog.txt");

    //private static readonly object _lock = new();

    //public static void Log(string msg)
    //{
    //    lock (_lock)
    //    {
    //        File.AppendAllText(LogPath, $"[{Process.GetCurrentProcess().Id}] {msg}{Environment.NewLine}");
    //    }
    //}

    //public static List<string> Logs { get; } = new();

    //public static void Print(string msg) => Logs.Add("//\t" + msg);

    //public static void FlushLogs(GeneratorExecutionContext context)
    //{
    //    context.AddSource($"logs.g.cs", SourceText.From(string.Join("\n", Logs), Encoding.UTF8));
    //}


    private static readonly List<string> Logs = new();
    private static readonly object _lock = new();

    public static void Print(string msg)
    {
        lock (_lock)
        {
            Logs.Add("//\t" + msg);
        }
    }

    public static void FlushLogs(SourceProductionContext context)
    {
        lock (_lock)
        {
            if (Logs.Count == 0)
                return;

            var content = string.Join("\n", Logs);
            context.AddSource($"logs.g.cs", SourceText.From(content, Encoding.UTF8));

            // Очищаем список после записи
            Logs.Clear();
        }
    }
}
