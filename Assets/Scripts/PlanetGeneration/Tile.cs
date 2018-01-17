using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace PlanetGeneration
{

    public class Tile
    {
        private float HexSize;
        public Point Center;
        public List<Point> Boundary = new List<Point>();
        public List<Face> Faces;
        public int Region;

        public Tile(Point center, float hexSize = 1.0f)
        {
            this.Center = center;
            this.Region = center.Region;
            this.HexSize = hexSize;
            this.Faces = center.GetOrderedFaces();

            foreach(Face face in Faces)
            {
                this.Boundary.Add(face.GetCentroid().Segment(this.Center, HexSize));
            }


            if (!PointingAwayFromOrigin())
            {
                Boundary.Reverse();
            }
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Tile))
            {
                return false;
            }
            var t = (Tile)obj;
            return t.Center == Center;
        }

        public override int GetHashCode()
        {
            return Center.GetHashCode();
        }

        private Vector3 GetNormal()
        {
            var U = Boundary[1].AsVector() - Boundary[0].AsVector();
            var V = Boundary[2].AsVector() - Boundary[0].AsVector();

            return Vector3.Cross(U, V);
        }

        private bool PointingAwayFromOrigin()
        {
            var normal = GetNormal();
            return (Center.x * normal.x >= 0) && (Center.y * normal.y >= 0) && (Center.z * normal.z >= 0);
        }

    }
}