namespace Web.UI.Server.Common
{
    public class ApiSetting
    {
        public string BaseUrl { get; set; } = "https://localhost:7241/api";
        public int HttpClientTimeoutMs { get; set; } = 10000;
     

    }
}
