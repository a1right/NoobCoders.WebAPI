using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nest;
using NoobCoders.Domain;

namespace NoobCodersFullTextSearchRepository
{
    public static class DependencyInject
    {
        public static void AddElasticSearch(
            this IServiceCollection services, IConfiguration configuration)
        {
            var user = configuration["ELKConfiguration:User"];
            var password = configuration["ELKConfiguration:Password"];
            var url = configuration["ELKConfiguration:Url"];
            var certificate = configuration["ELKConfiguration:Certificate"];
            var defaultIndex = configuration["ELKConfiguration:Index"];

            var settings = new ConnectionSettings(new Uri(url))
                .CertificateFingerprint(certificate)
                .BasicAuthentication(user, password)
                .PrettyJson()
                .DefaultIndex(defaultIndex);

            AddDefaultMappings(settings);

            var client = new ElasticClient(settings);

            services.AddSingleton<IElasticClient>(client);
        }

        private static void AddDefaultMappings(ConnectionSettings settings)
        {
            settings.DefaultMappingFor<Post>(post => post
                    .Ignore(p => p.PostRubrics)
                    .Ignore(p => p.CreatedDate)
                );
        }
        
    }
}
