// See https://aka.ms/new-console-template for more information

using Sora;
using Sora.Entities;
using Sora.Interfaces;
using Sora.Net.Config;
using Sora.Util;
using Sora.EventArgs.SoraEvent;
using YukariToolBox.LightLog;

Log.LogConfiguration
    .EnableConsoleOutput()
    .SetLogLevel(LogLevel.Debug);

// ISoraService service = SoraServiceFactory.CreateService(new ClientConfig()
// {
//     AccessToken = "123456",
//     Port = 8080
// });

ISoraService service = SoraServiceFactory.CreateService(new ServerConfig()
{
    AccessToken = "123456",
    Port = 9000
});

await service.StartService()
    .RunCatch(e => Log.Error("Sora Service", Log.ErrorLogBuilder(e)));
await Task.Delay(-1);