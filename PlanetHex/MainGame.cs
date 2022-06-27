using System;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using MonoGame.Extended.Entities;

using PlanetHex.Components;
using PlanetHex.Systems;

namespace PlanetHex
{
    public class MainGame : Game
    {
        private GraphicsDeviceManager? _graphics;
        private World _world = null!;

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
                Position = new Vector3(0, 75, -125),
                Target = new Vector3(0, 0, 0),
            });

            var triangle = _world.CreateEntity();
            triangle.Attach(new MeshRenderableComponent
            {
                Vertices = new[]
                {
                    new VertexPositionColor(new Vector3(0, 20, 0), Color.Red),
                    new VertexPositionColor(new Vector3(-20, -20, 0), Color.Green),
                    new VertexPositionColor(new Vector3(20, -20, 0), Color.Blue),
                    new VertexPositionColor(new Vector3(0, -40, 0), Color.Yellow),
                    new VertexPositionColor(new Vector3(0, -20, -20), Color.Coral),
                },
                Indices = new short[]
                {
                    0, 1, 2, 
                    1, 2, 3,
                    1, 2, 4,
                    2, 3, 4,
                    0, 2, 4,
                    0, 3, 4,
                },
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
