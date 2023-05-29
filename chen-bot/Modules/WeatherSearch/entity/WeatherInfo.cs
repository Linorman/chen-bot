public class WeatherInfo
{
    public string code { get; set; }
    public DateTimeOffset updateTime { get; set; }
    public string fxLink { get; set; }
    public Now now { get; set; }
    public WeatherRefer refer { get; set; }

    public WeatherInfo(string code, DateTimeOffset updateTime, string fxLink, Now now, WeatherRefer refer)
    {
        this.code = code;
        this.updateTime = updateTime;
        this.fxLink = fxLink;
        this.now = now;
        this.refer = refer;
    }
}

public class Now
{
    public DateTimeOffset obsTime { get; set; }
    public string temp { get; set; }
    public string feelsLike { get; set; }
    public string icon { get; set; }
    public string text { get; set; }
    public string wind360 { get; set; }
    public string windDir { get; set; }
    public string windScale { get; set; }
    public string windSpeed { get; set; }
    public string humidity { get; set; }
    public string precip { get; set; }
    public string pressure { get; set; }
    public string vis { get; set; }
    public string cloud { get; set; }
    public string dew { get; set; }
}

public class WeatherRefer
{
    public List<string> sources { get; set; }
    public List<string> license { get; set; }
}