using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using PlanetHex.PlanetGeneration;
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

    public static (VertexPositionColor[] vertices, short[] indices) CreateMeshFromTile(Tile tile, Color color,
        bool normalize = true, int height = 0)
    {
        var radius = (tile.Center.AsVector() - Vector3.Zero).Length() + height;
        var vertexVectors = tile.Boundary.Select(point => point.AsVector()).ToList();
        vertexVectors.AddRange(
            tile.Boundary.Select(point => PointHelpers.ProjectToRadius(point, radius + 1).AsVector()));

        short[] indices;
        if (tile.Boundary.Count == 5)
        {
            indices = new short[]
            {
                0, 2, 1,
                2, 4, 3,
                0, 4, 2,

                5, 6, 7,
                7, 8, 9,
                5, 7, 9,

                0, 1, 6,
                0, 6, 5,

                1, 2, 7,
                1, 7, 6,

                2, 3, 8,
                2, 8, 7,

                3, 4, 9,
                3, 9, 8,

                4, 0, 5,
                4, 5, 9,
            };
        }
        else
        {
            indices = new short[]
            {
                0, 2, 1,
                2, 4, 3,
                4, 0, 5,
                0, 4, 2,

                6, 7, 8,
                8, 9, 10,
                10, 11, 6,
                6, 8, 10,

                0, 1, 7,
                0, 7, 6,

                1, 2, 8,
                1, 8, 7,

                2, 3, 9,
                2, 9, 8,

                3, 4, 10,
                3, 10, 9,

                4, 5, 11,
                4, 11, 10,
                5, 0, 6,
                5, 6, 11,
            };
        }

        if (normalize)
        {
            var centerVector = tile.Center.AsVector();
            vertexVectors = vertexVectors.Select(v => v - centerVector).ToList();
        }

        var vertices = vertexVectors.Select(v => new VertexPositionColor(v, color)).ToArray();

        return (vertices.ToArray(), indices);
    }

    public static IEnumerator<Color> ColorWheel()
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
