using System;

using Microsoft.Extensions.DependencyInjection;

using PlanetHex.Setup;

namespace PlanetHex
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            var serviceCollection = DependencyInjection.BuildServiceCollection();
            var serviceProvider = serviceCollection.BuildServiceProvider();
            using var game = serviceProvider.GetRequiredService<MainGame>();
            game.Run();
        }
    }
}
