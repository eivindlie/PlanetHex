using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework.Graphics;

namespace PlanetHex.Components;

public class MeshRenderableComponent
{
    public VertexPositionColor[] Vertices { get; init; } = Array.Empty<VertexPositionColor>();
}
