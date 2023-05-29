using Sora.Attributes.Command;
using Sora.Enumeration;
using Sora.EventArgs.SoraEvent;
using YukariToolBox.LightLog;
using System.Net.Http;
using chen_bot.Modules.Translate.entity;
using MatchType = Sora.Enumeration.MatchType;
using System.Text.Json;

namespace chen_bot.Modules.Translate.handler;

[CommandSeries]
public class TranslateHandler
{
    [SoraCommand(
        CommandExpressions = new[] {@"(?i)^(?:#|＃)(汉译英|英译汉)(?:-|——)((\s|\S)+)$"},
        SourceType = SourceFlag.Group,
        MatchType = MatchType.Regex
    )]
    public static async ValueTask Translate(GroupMessageEventArgs eventArgs)
    {
        if (eventArgs.Message.GetText().Length >= 400)
        {
            Log.Debug("MessageHandler", "Message too long");
        }

        if (eventArgs.Message.GetText() == null)
        {
            Log.Debug("MessageHandler", "Message is null");
        }

        // 获取翻译数据
        string text = eventArgs.Message.GetText().Split('-')[1].Trim();
        string mode = eventArgs.Message.GetText().Split('-')[0].Trim().Substring(1);
        Log.Debug("TranslateHandlerInfo", $"text: {text}, mode: {mode}");

        if (text.Length == 0)
        {
            await eventArgs.Reply("宝，你让我翻译个寂寞吗？");
            return;
        }
        // 判断text是否只包含标点符号或者空格
        if (System.Text.RegularExpressions.Regex.IsMatch(text, @"^[\p{P}\s]+$"))
        {
            Log.Debug("SomeHandler", "Text contains invalid characters");
            await eventArgs.Reply("亲，你输入的内容有误哦");
            return;
        }
        
        string from = "";
        string to = "";

        if (mode == "汉译英")
        {
            from = "zh";
            to = "en";
        }
        else if (mode == "英译汉")
        {
            from = "en";
            to = "zh"; 
        }

        RequestInfo requestInfo = new RequestInfo(from, to, text);
        var client = new HttpClient();
        var response = await client.GetAsync(requestInfo.getUrl());
        var result = await response.Content.ReadAsStringAsync();
        TranslationResult? translationResult = null;
        if (!string.IsNullOrEmpty(result))
        {
            translationResult = JsonSerializer.Deserialize<TranslationResult>(result);
        }

        if (translationResult == null)
        {
            Log.Debug("MessageHandler", "TranslationResult is null");
        }

        // 发送翻译结果
        string translation = "诺，你要的翻译结果：\n";
        foreach (var translation1 in translationResult!.TransResult)
        {
            translation += translation1.Dst + "\n";
        }
        
        await eventArgs.Reply(translation);
    }
    
    [SoraCommand(
        CommandExpressions = new[] {@"(?i)^(?:#|＃)(汉译英|英译汉)(?:-|——)$"},
        SourceType = SourceFlag.Group,
        MatchType = MatchType.Regex
    )]
    public static async ValueTask TranslateNull(GroupMessageEventArgs eventArgs)
    {
        await eventArgs.Reply("宝，你让我翻译个寂寞吗？");
    }
    
    [SoraCommand(
        CommandExpressions = new[] {@"(?i)^(?:#|＃)(汉译英|英译汉)$"},
        SourceType = SourceFlag.Private,
        MatchType = MatchType.Regex
    )]
    public static async ValueTask TranslatePrivate(GroupMessageEventArgs eventArgs)
    {
        await eventArgs.Reply("宝，你让我翻译个寂寞吗？");
    }
}