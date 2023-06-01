using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Web.UI.Server.Common
{
    public static class ConfigureSetting
    {
        public static TConfig TryAddConfiguration<TConfig>(this IServiceCollection services, IConfiguration configuration)
      where TConfig : class, new()
        {
            var config = new TConfig();

            var configSection = configuration.GetSection(typeof(TConfig).Name);
            configSection.Bind(config);
            services.TryAddSingleton(config);
            return config;
        }


    }
}
