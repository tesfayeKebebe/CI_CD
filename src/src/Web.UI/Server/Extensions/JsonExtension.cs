using System.Text.Json.Serialization;
using System.Text.Json;

namespace Web.UI.Server.Extensions
{
    public static class JsonExtension
    {
      
            private static readonly JsonSerializerOptions _options = new()
            {
                IncludeFields = false,
                IgnoreReadOnlyProperties = false,
                WriteIndented = false,
                PropertyNameCaseInsensitive = false,
                NumberHandling = JsonNumberHandling.Strict,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                MaxDepth = 10
            };

            public static JsonSerializerOptions    GetOptions() => _options;     

    }
}
