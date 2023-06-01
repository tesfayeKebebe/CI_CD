namespace RazorShared.Server.Common
{
    public class ApiSetting
    {

       //public string BaseUrl { get; set; } = "https://localhost:7241/api";
       public  string BaseUrl = "https://b6bf-197-156-77-102.eu.ngrok.io/api";

        public int HttpClientTimeoutMs { get; set; } = 10000;
     

    }
}
