using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PlanetHex
{
    public class MainGame : Game
    {
        private GraphicsDeviceManager? _graphics;
        private SpriteBatch? _spriteBatch;

        private Vector3 _camTarget;
        private Vector3 _camPosition;

        private Matrix _projectionMatrix;
        private Matrix _viewMatrix;
        private Matrix _worldMatrix;

        private BasicEffect? _basicEffect;
        private VertexPositionColor[]? _triangleVertices;
        private VertexBuffer? _vertexBuffer;

        //Orbit
        bool orbit = true;
        public MainGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _camTarget = new Vector3(0, 0, 0);
            _camPosition = new Vector3(0, 0, -100);
            _projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45.0f),
                GraphicsDevice.Viewport.AspectRatio, 1f, 1000.0f);
            _viewMatrix = Matrix.CreateLookAt(_camPosition, _camTarget, Vector3.Up);
            _worldMatrix = Matrix.CreateWorld(_camTarget, Vector3.Forward, Vector3.Up);

            _basicEffect = new BasicEffect(GraphicsDevice);
            _basicEffect.Alpha = 1.0f;
            _basicEffect.VertexColorEnabled = true;
            _basicEffect.LightingEnabled = false;

            _triangleVertices = new VertexPositionColor[3];
            _triangleVertices[0] = new VertexPositionColor(new Vector3(0, 20, 0), Color.Red);
            _triangleVertices[1] = new VertexPositionColor(new Vector3(-20, -20, 0), Color.Green);
            _triangleVertices[2] = new VertexPositionColor(new Vector3(20, -20, 0), Color.Blue);

            _vertexBuffer = new VertexBuffer(GraphicsDevice, typeof(VertexPositionColor), _triangleVertices.Length,
                BufferUsage.WriteOnly);
            _vertexBuffer.SetData(_triangleVertices);
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == 
                ButtonState.Pressed || Keyboard.GetState().IsKeyDown(
                    Keys.Escape))
                Exit();

            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                _camPosition.X -= 0.1f;
                _camTarget.X -= 0.1f;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                _camPosition.X += 0.1f;
                _camTarget.X += 0.1f;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                _camPosition.Y -= 0.1f;
                _camTarget.Y -= 0.1f;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                _camPosition.Y += 0.1f;
                _camTarget.Y += 0.1f;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.OemPlus))
            {
                _camPosition.Z += 0.1f;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.OemMinus))
            {
                _camPosition.Z -= 0.1f;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                orbit = !orbit;
            }

            if (orbit)
            {
                Matrix rotationMatrix = Matrix.CreateRotationY(
                    MathHelper.ToRadians(1f));
                _camPosition = Vector3.Transform(_camPosition, 
                    rotationMatrix);
            }
            _viewMatrix = Matrix.CreateLookAt(_camPosition, _camTarget, 
                Vector3.Up);
            base.Update(gameTime);

        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _basicEffect!.Projection = _projectionMatrix;
            _basicEffect!.View = _viewMatrix;
            _basicEffect!.World = _worldMatrix;

            GraphicsDevice.Clear(Color.CornflowerBlue);
            GraphicsDevice.SetVertexBuffer(_vertexBuffer);

            RasterizerState rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.None;
            GraphicsDevice.RasterizerState = rasterizerState;

            foreach (EffectPass pass in _basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, 3);
            }

            base.Draw(gameTime);
        }
    }
}
