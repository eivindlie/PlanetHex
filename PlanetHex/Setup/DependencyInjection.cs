using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using PlanetHex.Systems;

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
        serviceCollection.AddSystems();
        serviceCollection.AddSingleton<MainGame>();

        serviceCollection.AddSingleton(provider =>
            provider
                .GetRequiredService<MainGame>()
                .Services
                .GetService<IGraphicsDeviceManager>());

        serviceCollection.AddSingleton(provider =>
            provider
                .GetRequiredService<MainGame>()
                .Services
                .GetService<IGraphicsDeviceService>());

        serviceCollection.AddSingleton(provider =>
            provider
                .GetRequiredService<MainGame>()
                .GraphicsDevice);
    }

    private static void AddSystems(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<RenderSystem>();
    }
}
