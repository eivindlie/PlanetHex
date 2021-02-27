using System.Collections.Generic;
using System.Linq;

using Models.HexSphere;

using PlanetGeneration;

using UnityEngine;

namespace Meshes
{
    public static class TileToMeshMapper
    {
        public static Mesh Map(Tile tile, float bottomRadius, float height)
        {
            var count = tile.Boundary.Count;
            var offset = count * 2 - 1;

            var vertices = new Vector3[offset * 2];
            var tris = new List<int>();
            var pos = PointHelpers.ProjectToRadius(tile.Center, bottomRadius).AsVector();
            var vertexIndex = 0;
            for (var i = 0; i < tile.Boundary.Count; i++)
            {
                vertices[vertexIndex] = PointHelpers.ProjectToRadius(tile.Boundary[i], bottomRadius).AsVector() - pos;
                vertices[offset + vertexIndex] =
                    PointHelpers.ProjectToRadius(tile.Boundary[i], bottomRadius + height).AsVector() - pos;
                var next = 1;
                if (i != 1 && i != 2)
                {
                    next++;
                    vertices[vertexIndex + 1] =
                        PointHelpers.ProjectToRadius(tile.Boundary[i], bottomRadius).AsVector() - pos;
                    vertices[offset + vertexIndex + 1] =
                        PointHelpers.ProjectToRadius(tile.Boundary[i], bottomRadius + height).AsVector() - pos;
                }
                if (i == 0)
                {
                    next++;
                    vertices[vertexIndex + 2] = PointHelpers.ProjectToRadius(tile.Boundary[i], bottomRadius).AsVector() - pos;
                    vertices[offset + vertexIndex + 2] =
                        PointHelpers.ProjectToRadius(tile.Boundary[i], bottomRadius + height).AsVector() - pos;
                }
                if (i == count - 1)
                {
                    next += 2;
                }

                tris.Add(vertexIndex);
                tris.Add((vertexIndex + next) % offset);
                tris.Add(vertexIndex + offset);
                tris.Add((vertexIndex + next) % offset);
                tris.Add((vertexIndex + next) % offset + offset);
                tris.Add(vertexIndex + offset);
                vertexIndex += next;
            }

            tris.Add(1);
            tris.Add(4);
            tris.Add(3);
            tris.Add(offset + 1);
            tris.Add(offset + 3);
            tris.Add(offset + 4);
            tris.Add(4);
            tris.Add(8);
            tris.Add(6);
            tris.Add(offset + 4);
            tris.Add(offset + 6);
            tris.Add(offset + 8);
            tris.Add(8);
            tris.Add(4);
            tris.Add(1);
            tris.Add(offset + 8);
            tris.Add(offset + 1);
            tris.Add(offset + 4);

            if (tile.Boundary.Count > 5)
            {
                tris.Add(8);
                tris.Add(1);
                tris.Add(10);
                tris.Add(offset + 8);
                tris.Add(offset + 10);
                tris.Add(offset + 1);
            }

            Vector2[] uvs = null;
            if (count == 5)
            {
                uvs = new Vector2[]
                {
                    new Vector2(1, 0.3078f),
                    new Vector2(0.8617f, 0.1176f),
                    new Vector2(0, 0.3078f),
                    new Vector2(0.8f, 0.3078f),
                    new Vector2(0.6f, 0.3078f),
                    new Vector2(0.4f, 0.3078f),
                    new Vector2(0.5382f, 0.1176f),
                    new Vector2(0.2f, 0.3078f),
                    new Vector2(0.6999f, 0),
                    new Vector2(1, 0.4778f),
                    new Vector2(0.8617f, 0.6680f),
                    new Vector2(0, 0.4778f),
                    new Vector2(0.8f, 0.4778f),
                    new Vector2(0.6f, 0.4778f),
                    new Vector2(0.4f, 0.4778f),
                    new Vector2(0.5382f, 0.6680f),
                    new Vector2(0.2f, 0.4778f),
                    new Vector2(0.6999f, 0.7856f),
                };
            }
            else if (count == 6)
            {
                uvs = new Vector2[]
                {
                    new Vector2(1, 0.2887f),
                    new Vector2(0.9166f, 0.1444f),
                    new Vector2(0, 0.2887f),
                    new Vector2(0.8332f, 0.2887f),
                    new Vector2(0.6666f, 0.2887f),
                    new Vector2(0.5f, 0.2887f),
                    new Vector2(0.5833f, 0.1444f),
                    new Vector2(0.3333f, 0.2887f),
                    new Vector2(0.6666f, 0),
                    new Vector2(0.1667f, 0.2887f),
                    new Vector2(0.8332f, 0),
                    new Vector2(1, 0.4553f),
                    new Vector2(0.9166f, 0.5996f),
                    new Vector2(0, 0.4553f),
                    new Vector2(0.8332f, 0.4553f),
                    new Vector2(0.6666f, 0.4553f),
                    new Vector2(0.5f, 0.4553f),
                    new Vector2(0.5833f, 0.5996f),
                    new Vector2(0.3333f, 0.4453f),
                    new Vector2(0.6666f, 0.7439f),
                    new Vector2(0.1667f, 0.4553f),
                    new Vector2(0.8332f, 0.7436f),
                };
            }

            var mesh = new Mesh()
            {
                vertices = vertices,
                triangles = tris.ToArray(),
                uv = uvs
            };
            mesh.RecalculateNormals();
            return mesh;
        }
    }
}
