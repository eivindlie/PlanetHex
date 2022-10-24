using System;
using System.Linq;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using MonoGame.Extended.Entities;

using PlanetHex.Components;
using PlanetHex.PlanetGeneration;
using PlanetHex.PlanetGeneration.Models.Planet;
using PlanetHex.PlanetGeneration.Noise;
using PlanetHex.Setup;
using PlanetHex.Systems;

namespace PlanetHex
{
    public class MainGame : Game
    {
        private GraphicsDeviceManager? _graphics;
        private World _world = null!;
        private Planet _planet = null!;

        private readonly IServiceProvider _serviceProvider;

        public MainGame(IServiceProvider serviceProvider)
        {
            _graphics = new GraphicsDeviceManager(this);
            _serviceProvider = serviceProvider;
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _world = new WorldBuilder()
                .AddSystem(_serviceProvider.GetRequiredService<CameraOrbitSystem>())
                .AddSystem(_serviceProvider.GetRequiredService<RenderSystem>())
                .Build();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            var camera = _world.CreateEntity();
            camera.Attach(new CameraComponent
            {
                Position = new Vector3(0, 0, -350),
                Target = new Vector3(0, 0, 0),
            });

            var planetGenerator = new PlanetGenerator(new PlanetGeneratorSettings());
            _planet = planetGenerator.Generate();

            var colors = PlanetRenderHelper.ColorWheel();
            foreach (var (r, region) in _planet.HexSphere.Regions.Select((region, r) => (r, region)))
            {
                if (r % 3 != 0)
                {
                    continue;
                }
                colors.MoveNext();
                var noise = new SimplexNoiseGenerator();
                var coordFactor = 2f / (_planet.BaseRadius + _planet.HeightLimit);
                foreach (var tile in region.Tiles)
                {
                    var center = tile.Center.AsVector();
                    var noiseIntensity = (noise.CoherentNoise(center.X, center.Y, center.Z));
                    var height = (int)Math.Round(((noiseIntensity + 1) / 2) * _planet.BaseRadius);
                    var (vertices, indices) = PlanetRenderHelper.CreateMeshFromTile(tile, colors.Current, false, height);

                    var entity = _world.CreateEntity();
                    entity.Attach(new MeshRenderableComponent
                    {
                        Vertices = vertices,
                        Indices = indices,
                    });
                }
            }

            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back ==
                ButtonState.Pressed || Keyboard.GetState()
                    .IsKeyDown(
                        Keys.Escape))
                Exit();
            _world.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            _world.Draw(gameTime);
            base.Draw(gameTime);
        }
    }
}
