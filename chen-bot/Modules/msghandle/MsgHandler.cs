using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sora.Attributes.Command;
using Sora.Enumeration;
using Sora.EventArgs.SoraEvent;
using System.Security.AccessControl;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Emit;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Sora.Entities;
using Sora.Entities.Segment;
using YukariToolBox.LightLog;
using Microsoft.EntityFrameworkCore;
using chen_bot.Modules.msghandle;
using YukariToolBox.LightLog;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace chenbot.Modules.msghandle
{
    [CommandSeries]
    public class MsgHandler
    {
        public static int count = 0;

        //保存消息记录
        [SoraCommand(
                CommandExpressions = new[] { @"[\s\S]" },
                SourceType = SourceFlag.Private,
                MatchType = Sora.Enumeration.MatchType.Regex
            )]
        public static async ValueTask getMsg(PrivateMessageEventArgs eventArgs)
        {
            using var db = new MsgContext();
            //eventArgs.IsContinueEventChain = false;
            db.Add(new Msg
            {
                //id = ++count,
                userQQ = eventArgs.SenderInfo.UserId.ToString(),
                time = Convert.ToInt32(DateTimeOffset.Now.ToUnixTimeSeconds()),
                context = eventArgs.Message.GetText()
            });
            db.SaveChanges();
        }

        //消息记录查询
        [SoraCommand(
                CommandExpressions = new[] { @"#查询\s[\S]+" },
                SourceType = SourceFlag.Private,
                MatchType =Sora.Enumeration.MatchType.Regex,
                Priority = 10
            )]
        public static async ValueTask queryMsg(PrivateMessageEventArgs eventArgs)
        {
            using var db = new MsgContext();
            eventArgs.IsContinueEventChain = false;
            string queryStr = eventArgs.Message.GetText().Substring(4);
            List<Msg> msgs = db.Messages.Where(m => m.context.Contains(queryStr)).OrderBy(m => m.time).ToList();
            //构建消息段
            MessageBody messageBody = new MessageBody();
            foreach (Msg msg in msgs)
            {
                string str = DateTimeOffset.FromUnixTimeSeconds(msg.time).DateTime.ToLocalTime().ToString();
                str += " " + msg.userQQ;
                str += " " + msg.context;
                str += '\n';
                messageBody += SoraSegment.Text(str);
            }
            await eventArgs.Reply(messageBody);
        }
    }
}
