using Microsoft.Extensions.DependencyInjection;

namespace PlanetHex.Setup;

public static class DependencyInjection
{
    public static ServiceCollection BuildServiceCollection()
    {
        var services = new ServiceCollection();
        services.AddServices();
        return services;
    }

    private static void AddServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<MainGame>();
    }
}
