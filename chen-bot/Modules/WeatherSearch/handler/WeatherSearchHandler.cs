using System.IO.Compression;
using Sora.Attributes.Command;
using Sora.Enumeration;
using Sora.EventArgs.SoraEvent;
using YukariToolBox.LightLog;
using JsonSerializer = System.Text.Json.JsonSerializer;
using MatchType = Sora.Enumeration.MatchType;

namespace DefaultNamespace;

[CommandSeries]
public class WeatherSearchHandler
{
    [SoraCommand(
        CommandExpressions = new[] {@"(?i)^(?:#|＃)(天气|天氣|weather)(?:-|——)(\S+)$"},
        SourceType = SourceFlag.Group,
        MatchType = MatchType.Regex)]
    public static async ValueTask WeatherHandler(GroupMessageEventArgs eventArgs)
    {
        Log.Debug("WeatherSearchHandler", "In WeatherHandler");
        if (eventArgs.Message.GetText().Length >= 400)
        {
            Log.Debug("MessageHandler", "Message too long");
        }

        if (eventArgs.Message.GetText() == null)
        {
            Log.Debug("MessageHandler", "Message is null");
        }
        
        // 获取城市
        string city = eventArgs.Message.GetText().Split('-')[1].Trim();
        Log.Debug("WeatherSearchHandler", $"city: {city}");
        CityInfo? cityInfo = await GetCityInfo(city);
        if (cityInfo.code != "200")
        {
            Log.Debug("WeatherSearchHandler", "CityInfo is null");
            await eventArgs.Reply("宝，你要不看看你发的是什么");
            return;
        }
        WeatherInfo weatherInfo = await GetWeatherInfo(cityInfo.location[0].id);
        
        // 构造回复消息
        if (weatherInfo.code != "200")
        {
            Log.Debug("WeatherSearchHandler", $"Error code: {weatherInfo.code}");
        }
        string replyMessage = $"城市：{cityInfo.location[0].name}\n" +
                              $"天气：{weatherInfo.now.text}\n" +
                              $"温度：{weatherInfo.now.temp}℃\n" +
                              $"体感温度：{weatherInfo.now.feelsLike}℃\n" +
                              $"风向：{weatherInfo.now.windDir}\n" +
                              $"风速：{weatherInfo.now.windSpeed}km/h\n" +
                              $"湿度：{weatherInfo.now.humidity}%\n" +
                              $"能见度：{weatherInfo.now.vis}km\n" +
                              $"气压：{weatherInfo.now.pressure}mb\n" +
                              $"更新时间：{weatherInfo.now.obsTime}";
        await eventArgs.Reply(replyMessage);
    }

    public static async ValueTask<CityInfo> GetCityInfo(string city)
    {
        // 发送请求
        HttpClient httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Add("Accept-Encoding", "gzip");
        string url = ApiConstant.CITY_REQUEST_URL_HEADER + city;
        var response = await httpClient.GetAsync(url);
        
        // 解压 Gzip 数据
        Stream responseStream = await response.Content.ReadAsStreamAsync();
        if (response.Content.Headers.ContentEncoding.Contains("gzip"))
        {
            responseStream = new GZipStream(responseStream, CompressionMode.Decompress);
        }

        // 将结果反序列化为 CityInfo 对象
        try
        {
            var cityInfo = await JsonSerializer.DeserializeAsync<CityInfo>(responseStream);
            return cityInfo;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }
    }
    
    public static async ValueTask<WeatherInfo> GetWeatherInfo(string cityId)
    {
        // 发送请求
        HttpClient httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Add("Accept-Encoding", "gzip");
        string url = ApiConstant.WEATHER_REQUEST_URL_HEADER + cityId;
        var response = await httpClient.GetAsync(url);
        
        // 解压 Gzip 数据
        Stream responseStream = await response.Content.ReadAsStreamAsync();
        if (response.Content.Headers.ContentEncoding.Contains("gzip"))
        {
            responseStream = new GZipStream(responseStream, CompressionMode.Decompress);
        }

        // 将结果反序列化为 WeatherInfo 对象
        var weatherInfo = await JsonSerializer.DeserializeAsync<WeatherInfo>(responseStream);
        
        return weatherInfo;
    }
}