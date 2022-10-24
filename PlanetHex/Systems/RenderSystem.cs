using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;

using PlanetHex.Components;

namespace PlanetHex.Systems;

public class RenderSystem : EntityDrawSystem
{
    private ComponentMapper<MeshRenderableComponent> _meshRenderableMapper = null!;
    private ComponentMapper<CameraComponent> _cameraMapper = null!;

    private Matrix _projectionMatrix;
    private BasicEffect? _basicEffect;

    private readonly GraphicsDevice _graphicsDevice;

    public RenderSystem(GraphicsDevice graphicsDevice) :
        base(Aspect.All(typeof(MeshRenderableComponent)))
    {
        _graphicsDevice = graphicsDevice;
    }

    public override void Initialize(IComponentMapperService mapperService)
    {
        _projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45.0f),
            _graphicsDevice.Viewport.AspectRatio, 1f, 5000.0f);

        _basicEffect = new BasicEffect(_graphicsDevice);
        _basicEffect.Alpha = 1.0f;
        _basicEffect.VertexColorEnabled = true;

        _basicEffect.LightingEnabled = true;
        _basicEffect.AmbientLightColor = new Vector3(0.2f, 0.2f, 0.2f);
        _basicEffect.SpecularColor = Vector3.One;
        _basicEffect.SpecularPower = 255f;
        
        _basicEffect.DirectionalLight0.Enabled = true;
        _basicEffect.DirectionalLight0.DiffuseColor = new Vector3(1, 0, 0 );
        _basicEffect.DirectionalLight0.Direction = new Vector3(-1, 0, 0);
        _basicEffect.DirectionalLight0.SpecularColor = Vector3.One;

        _basicEffect.FogEnabled = true;
        _basicEffect.FogColor = Color.CornflowerBlue.ToVector3();
        _basicEffect.FogStart = 200f;
        _basicEffect.FogEnd = 1000f;

        _meshRenderableMapper = mapperService.GetMapper<MeshRenderableComponent>();
        _cameraMapper = mapperService.GetMapper<CameraComponent>();
    }

    public override void Draw(GameTime gameTime)
    {
        _graphicsDevice.Clear(Color.CornflowerBlue);
        var camera = _cameraMapper.Components.Last();
        var viewMatrix = Matrix.CreateLookAt(camera.Position, camera.Target, Vector3.Up);
        var worldMatrix = Matrix.CreateWorld(camera.Target, Vector3.Forward, Vector3.Up);

        _basicEffect!.Projection = _projectionMatrix;
        _basicEffect!.View = viewMatrix;
        _basicEffect!.World = worldMatrix;

        var rasterizerState = new RasterizerState
        {
            CullMode = CullMode.CullClockwiseFace,
        };
        _graphicsDevice.RasterizerState = rasterizerState;

        foreach (var entity in ActiveEntities)
        {
            var meshRenderableComponent = _meshRenderableMapper.Get(entity);
            var vertexBuffer = new VertexBuffer(_graphicsDevice, typeof(VertexPositionColor),
                meshRenderableComponent.Vertices.Length, BufferUsage.WriteOnly);
            vertexBuffer.SetData(meshRenderableComponent.Vertices);
            var indexBuffer = new IndexBuffer(_graphicsDevice, IndexElementSize.SixteenBits,
                meshRenderableComponent.Indices.Length, BufferUsage.WriteOnly);
            indexBuffer.SetData(meshRenderableComponent.Indices);

            _graphicsDevice.SetVertexBuffer(vertexBuffer);
            _graphicsDevice.Indices = indexBuffer;

            foreach (var pass in _basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                _graphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, indexBuffer.IndexCount / 3);
            }
        }
    }
}
