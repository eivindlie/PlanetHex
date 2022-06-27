using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using PlanetHex.PlanetGeneration.Models.HexSphere;

namespace PlanetHex.Setup;

internal static class PlanetRenderHelper
{
    public static (VertexPositionColor[] vertices, short[] indices) CreateMeshFromHexSphere(HexSphere hexSphere)
    {
        var color = Color.Blue;
        var vertices = new List<VertexPositionColor>();
        var indices = new List<short>();
        var startIndex = 0;
        var colors = ColorWheel();
        foreach (var tile in hexSphere.Regions.SelectMany(r => r.Tiles))
        {
            var tileColor = colors.Current;
            colors.MoveNext();
            vertices.AddRange(tile.Boundary.Select(point => new VertexPositionColor(point.AsVector(), tileColor)));

            startIndex += tile.Boundary.Count;

            short[] newIndices;
            if (vertices.Count == 5)
            {
                newIndices = new short[]
                {
                    0, 1, 2,
                    2, 3, 4,
                    0, 2, 4,
                };
            }
            else
            {
                newIndices = new short[]
                {
                    0, 1, 2,
                    2, 3, 4,
                    4, 5, 0,
                    0, 2, 4,
                };
            }
            indices.AddRange(newIndices.Select(i => (short)(i + startIndex)));
        }

        return (vertices.ToArray(), indices.ToArray());
    }

    private static IEnumerator<Color> ColorWheel()
    {
        var colors = new[]
        {
            Color.Red, Color.Green, Color.Blue, Color.Yellow, Color.RoyalBlue, Color.Cyan,
        };
        var currentColor = 0;

        while (true)
        {
            yield return colors[currentColor];
            currentColor = (currentColor + 1) % colors.Length;
        }
    }
}
