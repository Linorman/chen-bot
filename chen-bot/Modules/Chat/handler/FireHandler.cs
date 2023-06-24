using Sora.Attributes.Command;
using Sora.Enumeration;
using Sora.EventArgs.SoraEvent;
using YukariToolBox.LightLog;
using MatchType = Sora.Enumeration.MatchType;

namespace chen_bot.Modules.Chat.handler;

[CommandSeries]
public class FireHandler
{
    [SoraCommand(
        CommandExpressions = new[] {"早", "早哦", "早捏", "早！", "早哦！", "早捏！","早!" , "早哦!", "早捏!", "早。", "早哦。", "早捏。", " "},
        SourceType = SourceFlag.Private,
        MatchType = MatchType.Full
    )]
    public static async ValueTask Fire(PrivateMessageEventArgs eventArgs)
    {
        // 获取qq号
        long qq = eventArgs.Sender.Id;
        Log.Debug("FireHandlerInfo", $"qq: {qq}");
        if (qq == 3137596853 || qq == 2834875564)
        {
            await eventArgs.Reply("早捏");
        } else
        {
            return;
        }
    }
}