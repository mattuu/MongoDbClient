using Microsoft.Extensions.DependencyInjection;

namespace MongoDbClient.Caching.Infrastructure
{
    public static class DependencyConfiguration
    {
        public static IServiceCollection AddCaching(this IServiceCollection serviceCollection)
        {
            return serviceCollection.AddTransient<ICustomerIndexEngine, CustomerIndexEngine>();
        }
    }
}