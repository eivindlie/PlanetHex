using System;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using MonoGame.Extended.Entities;

using PlanetHex.Components;
using PlanetHex.PlanetGeneration;
using PlanetHex.PlanetGeneration.Models.Planet;
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
            var (vertices, indices) = PlanetRenderHelper.CreateMeshFromHexSphere(_planet.HexSphere);

            var planetEntity = _world.CreateEntity();
            planetEntity.Attach(new MeshRenderableComponent
            {
                Vertices = vertices,
                Indices = indices,
            });
            
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
