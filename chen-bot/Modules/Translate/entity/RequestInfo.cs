using System.Security.Cryptography;
using System.Text;

namespace chen_bot.Modules.Translate.entity;

public class RequestInfo
{
    public string urlHeader = "https://fanyi-api.baidu.com/api/trans/vip/translate?";
    public string from { get; set; }
    public string to { get; set; }
    public string q { get; set; }
    public string appid { get; set; }
    public string salt { get; set; }
    public string sign { get; set; }
    
    public string getUrl()
    {
        string url = urlHeader + "q=" + q + "&from=" + from + "&to=" + to + "&appid=" + appid + "&salt=" + salt + "&sign=" + sign;
        byte[] utf8Bytes = Encoding.UTF8.GetBytes(url);
        url = Encoding.UTF8.GetString(utf8Bytes);
        return url;
    }
    
    public RequestInfo(string from, string to, string q)
    {
        this.from = from;
        this.to = to;
        this.q = q;
        this.appid = "20230518001682427";
        
        // 生成随机数
        Random saltRandom = new Random();
        int saltInt = saltRandom.Next(1000000000);
        this.salt = saltInt.ToString();
        
        // 生成签名
        string signStr = appid + q + salt + "lThemPwyNdvejmlXR2Y3";
        MD5 md5 = MD5.Create();
        byte[] signBytes = md5.ComputeHash(Encoding.UTF8.GetBytes(signStr));
        StringBuilder signSb = new StringBuilder();
        foreach (byte signByte in signBytes)
        {
            signSb.Append(signByte.ToString("x2"));
        }
        this.sign = signSb.ToString();
    }
}