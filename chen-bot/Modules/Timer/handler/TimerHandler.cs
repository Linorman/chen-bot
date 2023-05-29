using Sora.Attributes.Command;
using Sora.Enumeration;
using Sora.EventArgs.SoraEvent;
using YukariToolBox.LightLog;
using MatchType = Sora.Enumeration.MatchType;

namespace chen_bot.Modules.Timer.handler;


[CommandSeries]
public class TimerHandler
{
    [SoraCommand(
        CommandExpressions = new[] {@"(?i)^(?:#|＃)(timer|定时)[-——](\d+)\s*分钟$"},
        SourceType = SourceFlag.Group,
        MatchType = MatchType.Regex
    )]
    public static async ValueTask Timer(GroupMessageEventArgs eventArgs)
    {
        // 获取定时时间
        string time = eventArgs.Message.GetText().Split('-')[1].Trim();
        time = time.Substring(0, time.Length - 2);
        Log.Debug("TimerHandlerInfo", $"time: {time}");
        
        // 获取qq号
        long qq = eventArgs.Sender.Id;
        Log.Debug("TimerHandlerInfo", $"qq: {qq}");
        
        // 设置定时器
        var timer = new System.Timers.Timer(int.Parse(time) * 60 * 1000);
        timer.Elapsed += async (sender, e) => {await eventArgs.Reply($"[CQ:at,qq={qq}]"+"时间到了"); };
        timer.AutoReset = false;
        timer.Enabled = true;
        
        // 回复
        await eventArgs.Reply("定时器已设置");
    }
}