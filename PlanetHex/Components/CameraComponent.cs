using Microsoft.Xna.Framework;

namespace PlanetHex.Components;

public class CameraComponent
{
    public Vector3 Position { get; init; }
    public Vector3 Target { get; init; }
}
