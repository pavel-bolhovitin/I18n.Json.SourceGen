
using System.Collections.Generic;

namespace I18n.Json.SourceGen;

internal class Config
{
    public string Namespace { get; set; } = "I18n";
    /// <summary>
    /// Paths to locale files or/and folers with locale-files.
    /// Example: ["locale.json", "locales/"] - in this case,
    /// generator will try to find all .json files in "locales/"
    /// folder and parse them as locale files.
    /// </summary>
    public List<string> LocalePaths { get; set; } = ["locale.json"];
}
