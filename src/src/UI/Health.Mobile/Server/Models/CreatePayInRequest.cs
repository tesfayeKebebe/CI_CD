using System.Runtime.Serialization;

namespace Health.Mobile.Server.Models;

[DataContract]
public class CreatePayInRequest
{
    [DataMember(Name = "appid")]
    public string AppId { get; set; }
    
    [DataMember(Name = "sign")]
    public string Sign { get; set; }

    [DataMember(Name = "ussd")]
    public string Ussd { get; set; }
}
public class CreateCallBackNotificationRequest
{
    [DataMember(Name = "code")]
    public string? code { get; set; }
    
    [DataMember(Name = "msg")]
    public string? message { get; set; }

    [DataMember(Name = "data")]
    public CallBackData data { get; set; }
}

public class CallBackData
{
    [DataMember(Name = "outTradeNo")]
    public string outTradeNo { get; set; }
    
    [DataMember(Name = "tradeNo")]
    public string tradeNo { get; set; } 
}