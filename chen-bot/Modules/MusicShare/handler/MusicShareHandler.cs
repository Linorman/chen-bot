using System.Text;
using GenericMusicClient;
using GenericMusicClient.Model;
using Sora.Attributes.Command;
using Sora.Enumeration;
using Sora.EventArgs.SoraEvent;
using YukariToolBox.LightLog;
using MatchType = Sora.Enumeration.MatchType;
namespace chen_bot.Modules.MusicShare.handler;

[CommandSeries]
public class MusicShareHandler
{
    [SoraCommand(
        CommandExpressions = new[] {@"(?i)^(?:#|＃)(搜歌曲|搜歌手)(?:-|——)((\s|\S)+)$"},
        SourceType = SourceFlag.Group,
        MatchType = MatchType.Regex
    )]
    public static async ValueTask MusicShare(GroupMessageEventArgs eventArgs)
    {
        if (eventArgs.Message.GetText().Length >= 400)
        {
            Log.Debug("MessageHandler", "Message too long");
        }

        if (eventArgs.Message.GetText() == null)
        {
            Log.Debug("MessageHandler", "Message is null");
        }

        // 获取歌曲/歌手
        string content = "";
        try
        {
            content = eventArgs.Message.GetText().Split('-')[1].Trim();
        }
        catch (System.Exception)
        {
            await eventArgs.Reply("宝，你让我搜个寂寞吗？");
            return;
        }
        string mode = eventArgs.Message.GetText().Split('-')[0].Trim().Substring(1);
        Log.Debug("MusicShareHandlerInfo", $"content: {content}, mode: {mode}");
        string textReply = "喏，你要的音乐在这里啦:\n";

        if (content.Length == 0)
        {
            await eventArgs.Reply("宝，你让我搜个寂寞吗？");
            return;
        }
        MusicClient musicClient = new (PlatformType.QQ);
        var list = await musicClient.GetByName(content);
        if (list.Count == 0)
        {
            await eventArgs.Reply("没有搜到相关的歌曲捏");
            return;
        }
        var music = list[0];
        textReply += music.Name +"\n";
        // await eventArgs.Reply(textReply);
        string? url = music.CoverUrl;
        if (url == null)
        {
            await eventArgs.Reply("没有找到歌曲");
            return;
        }
        // var client = new HttpClient();
        // var response = await client.GetAsync(url);
        // var result = await response.Content.ReadAsStringAsync();
        // var image = Convert.ToBase64String(Encoding.UTF8.GetBytes(result));
        // 转换成CQ码
        textReply += $"[CQ:image,file={url}]";
        await eventArgs.Reply(textReply);
    }
    
    [SoraCommand(
        CommandExpressions = new[] {@"(?i)^(?:#|＃)(搜歌曲|搜歌手)(?:-|——)$"},
        SourceType = SourceFlag.Private,
        MatchType = MatchType.Regex
    )]
    public static async ValueTask TranslateNull(GroupMessageEventArgs eventArgs)
    {
        await eventArgs.Reply("宝，你让我搜索个寂寞吗？");
    }
    
    [SoraCommand(
        CommandExpressions = new[] {@"(?i)^(?:#|＃)(搜歌曲|搜歌手)$"},
        SourceType = SourceFlag.Private,
        MatchType = MatchType.Regex
    )]
    public static async ValueTask TranslateNull2(GroupMessageEventArgs eventArgs)
    {
        await eventArgs.Reply("宝，你让我搜索个寂寞吗？");
    }
}