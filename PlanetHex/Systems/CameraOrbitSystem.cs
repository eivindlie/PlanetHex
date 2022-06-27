using Microsoft.Xna.Framework;

using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;

using PlanetHex.Components;

namespace PlanetHex.Systems;

public class CameraOrbitSystem : EntityProcessingSystem
{
    public CameraOrbitSystem() : base(Aspect.All(typeof(CameraComponent))) { }

    private ComponentMapper<CameraComponent> _cameraMapper = null!;
    private Matrix _rotationMatrix = new();
    
    public override void Initialize(IComponentMapperService mapperService)
    {
        _cameraMapper = mapperService.GetMapper<CameraComponent>();
        _rotationMatrix = Matrix.CreateRotationY(MathHelper.ToRadians(1f));
    }

    public override void Process(GameTime gameTime, int entityId)
    {
        var camera = _cameraMapper.Get(entityId);
        camera.Position = Vector3.Transform(camera.Position, _rotationMatrix);
    }
}
