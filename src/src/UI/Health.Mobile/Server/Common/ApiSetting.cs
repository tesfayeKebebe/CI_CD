namespace Health.Mobile.Server.Common
{
    public class ApiSetting
    {
      public string BaseUrl { get; set; } = "https://localhost:7241/api";
        //public  string BaseUrl = " https://32be-197-156-95-59.eu.ngrok.io/api";
        public int HttpClientTimeoutMs { get; set; } = 10000;
    }
}
