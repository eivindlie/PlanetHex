using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;

using PlanetHex.Components;

namespace PlanetHex.Systems;

public class RenderSystem : EntityDrawSystem
{
    private ComponentMapper<MeshRenderableComponent> _meshRenderableMapper = null!;

    private Vector3 _camTarget;
    private Vector3 _camPosition;

    private Matrix _projectionMatrix;
    private Matrix _viewMatrix;
    private Matrix _worldMatrix;
    private BasicEffect? _basicEffect;

    private readonly IGraphicsDeviceManager _graphicsDeviceManager;
    private readonly GraphicsDevice _graphicsDevice;

    public RenderSystem(IGraphicsDeviceManager graphicsDeviceManager, GraphicsDevice graphicsDevice) : base(
        Aspect.All(typeof(MeshRenderableComponent)))
    {
        _graphicsDeviceManager = graphicsDeviceManager;
        _graphicsDevice = graphicsDevice;
    }

    public override void Initialize(IComponentMapperService mapperService)
    {
        _camTarget = new Vector3(0, 0, 0);
        _camPosition = new Vector3(0, 0, -100);
        _projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45.0f),
            _graphicsDevice.Viewport.AspectRatio, 1f, 1000.0f);
        _viewMatrix = Matrix.CreateLookAt(_camPosition, _camTarget, Vector3.Up);
        _worldMatrix = Matrix.CreateWorld(_camTarget, Vector3.Forward, Vector3.Up);

        _basicEffect = new BasicEffect(_graphicsDevice);
        _basicEffect.Alpha = 1.0f;
        _basicEffect.VertexColorEnabled = true;
        _basicEffect.LightingEnabled = false;

        _meshRenderableMapper = mapperService.GetMapper<MeshRenderableComponent>();
    }

    public override void Draw(GameTime gameTime)
    {
        _graphicsDevice.Clear(Color.CornflowerBlue);
        _basicEffect!.Projection = _projectionMatrix;
        _basicEffect!.View = _viewMatrix;
        _basicEffect!.World = _worldMatrix;

        var rasterizerState = new RasterizerState
        {
            CullMode = CullMode.None,
        };
        _graphicsDevice.RasterizerState = rasterizerState;

        foreach (var entity in ActiveEntities)
        {
            var meshRenderableComponent = _meshRenderableMapper.Get(entity);
            var vertexBuffer = new VertexBuffer(_graphicsDevice, typeof(VertexPositionColor),
                meshRenderableComponent.Vertices.Length, BufferUsage.WriteOnly);
            vertexBuffer.SetData(meshRenderableComponent.Vertices);
            _graphicsDevice.SetVertexBuffer(vertexBuffer);
            foreach (var pass in _basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                _graphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, 3);
            }
        }
    }
}
