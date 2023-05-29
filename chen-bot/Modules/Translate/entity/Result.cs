using System.Text.Json.Serialization;

namespace chen_bot.Modules.Translate.entity;

public class TranslationResult
{
    [JsonPropertyName("from")]
    public string From { get; set; }
    
    [JsonPropertyName("to")]
    public string To { get; set; }
    
    [JsonPropertyName("trans_result")]
    public List<Translation> TransResult { get; set; }
}

public class Translation
{
    [JsonPropertyName("src")]
    public string Src { get; set; }
    
    [JsonPropertyName("dst")]
    public string Dst { get; set; }
}