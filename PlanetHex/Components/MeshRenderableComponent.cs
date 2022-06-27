using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework.Graphics;

namespace PlanetHex.Components;

public class MeshRenderableComponent
{
    public VertexPositionColor[] Vertices { get; set; } = Array.Empty<VertexPositionColor>();
    public short[] Indices { get; set; } = Array.Empty<short>();
}
