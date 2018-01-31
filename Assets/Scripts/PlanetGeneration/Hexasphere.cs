using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

namespace PlanetGeneration
{

    public class Hexasphere
    {
        private Dictionary<Point, Point> Points;
        private int NumDivisions;
        private float HexSize;
        private float Radius;
        public Region[] Regions;

        public Hexasphere(float radius, int numDivisions, float hexSize = 1.0f)
        {
            this.Radius = radius;
            this.NumDivisions = numDivisions;
            this.HexSize = hexSize;
            this.Regions = new Region[20];
            Points = new Dictionary<Point, Point>();

            Create();
        }

        private void Create()
        {
            float tao = 1.61803399f;
            Point[] corners =
            {
                new Point(1000, tao * 1000, 0),
                new Point(-1000, tao * 1000, 0),
                new Point(1000, -tao * 1000, 0),
                new Point(-1000, -tao * 1000, 0),
                new Point(0, 1000, tao * 1000),
                new Point(0, -1000, tao * 1000),
                new Point(0, 1000, -tao * 1000),
                new Point(0, -1000, -tao * 1000),
                new Point(tao * 1000, 0, 1000),
                new Point(-tao * 1000, 0, 1000),
                new Point(tao * 1000, 0, -1000),
                new Point(-tao * 1000, 0, -1000)
            };

            foreach(Point point in corners)
            {
                Points[point] = point;
            }

            List<Face> faces = new List<Face>() {
                new Face(corners[0], corners[1], corners[4], false),
                new Face(corners[1], corners[9], corners[4], false),
                new Face(corners[4], corners[9], corners[5], false),
                new Face(corners[5], corners[9], corners[3], false),
                new Face(corners[2], corners[3], corners[7], false),
                new Face(corners[3], corners[2], corners[5], false),
                new Face(corners[7], corners[10], corners[2], false),
                new Face(corners[0], corners[8], corners[10], false),
                new Face(corners[0], corners[4], corners[8], false),
                new Face(corners[8], corners[2], corners[10], false),
                new Face(corners[8], corners[4], corners[5], false),
                new Face(corners[8], corners[5], corners[2], false),
                new Face(corners[1], corners[0], corners[6], false),
                new Face(corners[11], corners[1], corners[6], false),
                new Face(corners[3], corners[9], corners[11], false),
                new Face(corners[6], corners[10], corners[7], false),
                new Face(corners[3], corners[11], corners[7], false),
                new Face(corners[11], corners[6], corners[7], false),
                new Face(corners[6], corners[0], corners[10], false),
                new Face(corners[9], corners[1], corners[11], false)
            };

            var newFaces = new List<Face>();

            var regionId = 0;
            foreach (Face face in faces)
            {
                Regions[regionId] = new Region(regionId);
                List<Point> prev = null;
                var bottom = new List<Point>() { face.points[0] };
                var left = face.points[0].Subdivide(face.points[1], this.NumDivisions, this);
                var right = face.points[0].Subdivide(face.points[2], this.NumDivisions, this);

                for (var i = 1; i <= NumDivisions; i++)
                {
                    prev = bottom;
                    bottom = left[i].Subdivide(right[i], i, this);
                    for (var j = 0; j < i; j++)
                    {
                        if (prev[j].Region == -1)
                        {
                            prev[j].Region = regionId;
                        }
                        var nf = new Face(prev[j], bottom[j], bottom[j + 1]);
                        newFaces.Add(nf);

                        if (j > 0)
                        {
                            nf = new Face(prev[j - 1], prev[j], bottom[j]);
                            newFaces.Add(nf);
                        }
                    }
                }
                foreach(var p in bottom)
                {
                    if (p.Region == -1) p.Region = regionId;
                }
                regionId++;
            }

            faces = newFaces;

            var newPoints = new Dictionary<Point, Point>();
            foreach (var p in Points.Values)
            {
                var np = p.Project(Radius);
                newPoints[np] = np;
            }

            Points = newPoints;

            foreach (var p in Points.Values)
            {
                var newTile = new Tile(p, HexSize);
                this.Regions[p.Region].AddTile(new Tile(p, HexSize));
            }

            foreach(Region region in Regions)
            {
                region.CalculateCenter();
            }
        }

        public Point GetOrAddPoint(Point point)
        {
            if (Points.ContainsKey(point))
            {
                var p = Points[point];
                return p;
            }
            else
            {
                Points[point] = point;
                return point;
            }
        }

        public Mesh GetMesh()
        {

            var verts = new List<Vector3>();
            var tris = new List<int>();
            var i = 0;
            foreach(Region region in Regions)
            {
                foreach(Tile tile in region.GetTiles())
                {
                    verts.AddRange(tile.Boundary.Select(p => p.AsVector()));

                    tris.Add(i); tris.Add(i + 1); tris.Add(i + 2);
                    tris.Add(i + 2); tris.Add(i + 3); tris.Add(i + 4);
                    tris.Add(i + 4); tris.Add(i); tris.Add(i + 2);

                    if (tile.Boundary.Count > 5)
                    {
                        tris.Add(i + 4); tris.Add(i + 5); tris.Add(i);
                    }

                    i += tile.Boundary.Count;
                }
            }

            var mesh = new Mesh()
            {
                vertices = verts.ToArray(),
                triangles = tris.ToArray()
            };
            mesh.RecalculateNormals();
            return mesh;
        }
    }

}

