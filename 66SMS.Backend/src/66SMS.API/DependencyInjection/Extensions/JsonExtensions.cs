using System.Text.Json.Serialization;

namespace _66SMS.API.DependencyInjection.Extensions
{
    public static class JsonExtensions
    {
        public static IMvcBuilder AddJsonConfig(this IMvcBuilder builder)
        {
            builder.AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.DefaultIgnoreCondition =
                    JsonIgnoreCondition.WhenWritingNull;

                options.JsonSerializerOptions.Converters
                    .Add(new JsonStringEnumConverter());
            });

            return builder;
        }
    }
}
