using Microsoft.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace I18n.Json.SourceGen;

internal class ConfigReader
{
    private const string ConfigName = "i18nJsonSourceGenConfig.json";
    private const string ConfigFileProperty = "build_property.I18nJsonSourceGenConfig";

    public static IncrementalValueProvider<Config> Create(IncrementalGeneratorInitializationContext context)
    {
        var configFileName = context.AnalyzerConfigOptionsProvider
            .Select((options, _) =>
            {
                options.GlobalOptions.TryGetValue(ConfigFileProperty, out var fileName);

                if (fileName != null)
                {
                    Logger.Print($"Provided custom config filename '{fileName}'");
                    return fileName;
                }
                else
                {
                    Logger.Print($"Using default config filename");
                    return ConfigName;
                }
            });

        return context.AdditionalTextsProvider
            .Combine(configFileName)
            .Where(pair => Path.GetFileName(pair.Left.Path) == pair.Right)
            .Select((pair, ct) => Parse(pair.Left.GetText(ct)?.ToString()))
            .Collect()
            .Select((configs, _) =>
            {
                if (configs.Length > 1)
                {
                    Logger.Print($"Multiple config files found! Using the first one.");
                    return configs[0];
                }
                else if (configs.Length == 1)
                {
                    Logger.Print($"Config file found.");
                    return configs[0];
                }
                else
                {
                    Logger.Print($"Config file not found!. Using default config.");
                    return new Config();
                }
            });
    }

    private static Config Parse(string? json)
    {
        if (string.IsNullOrWhiteSpace(json))
        {
            return new Config();
        }

        try
        {
            return JsonSerializer.Deserialize<Config>(json!) ?? new Config();
        }
        catch
        {
            return new Config();
        }
    }
}
