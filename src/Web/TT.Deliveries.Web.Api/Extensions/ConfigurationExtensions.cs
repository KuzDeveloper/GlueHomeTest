using Microsoft.Extensions.Configuration;
using System;
using TT.Deliveries.Web.Api.Enums;

namespace TT.Deliveries.Web.Api.Extensions
{
    public static class ConfigurationExtensions
    {
        public const string WorkEnvironment = "ASPNETCORE_ENVIRONMENT";

        public static TConfig Get<TConfig>(this IConfiguration configuration, string sectionName)
            where TConfig : class, new()
        {
            var result = new TConfig();

            configuration.GetSection(sectionName).Bind(result);

            return result;
        }

        public static string GetWorkEnvironmentTypeName(this IConfiguration configuration)
        {
            return configuration.GetValue<string>(WorkEnvironment);
        }

        public static EnvironmentType GetWorkEnvironmentType(this IConfiguration configuration)
        {
            var environmentTypeName = configuration.GetWorkEnvironmentTypeName();

            if (!Enum.TryParse<EnvironmentType>(environmentTypeName, out var environmentType))
            {
                throw new InvalidOperationException($"Environment type for name ({environmentTypeName}) can't be defined");
            }

            return environmentType;
        }
    }
}
