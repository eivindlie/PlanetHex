using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace PlanetGeneration
{
    public class Point
    {
        public float x;
        public float y;
        public float z;
        public List<Face> faces;

        public Point(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;

            faces = new List<Face>();
        }

        public List<Point> Subdivide(Point point, int count, Hexasphere hexasphere)
        {
            var segments = new List<Point>();
            segments.Add(this);

            for (var i = 1; i < count; i++)
            {
                float ratio = (float)i / count;
                float x = this.x * (1 - ratio) + point.x * ratio;
                float y = this.y * (1 - ratio) + point.y * ratio;
                float z = this.z * (1 - ratio) + point.z * ratio;
                var np = new Point(x, y, z);
                np = hexasphere.GetOrAddPoint(np);
                segments.Add(np);
            }
            segments.Add(point);
            return segments;
        }

        public Point Segment(Point point, float ratio)
        {
            ratio = Mathf.Max(0.01f, Mathf.Min(1, ratio));
            var x = point.x * (1 - ratio) + this.x * ratio;
            var y = point.y * (1 - ratio) + this.y * ratio;
            var z = point.z * (1 - ratio) + this.z * ratio;

            return new Point(x, y, z);
        }

        public Point Midpoint(Point point)
        {
            return Segment(point, 0.5f);
        }


        public override bool Equals(object obj)
        {
            if (!(obj is Point))
            {
                return false;
            }

            var p = (Point)obj;

            return Mathf.Abs(this.x - p.x) < 0.01
                    && Mathf.Abs(this.y - p.y) < 0.01
                    && Mathf.Abs(this.z - p.z) < 0.01;
        }

        public override int GetHashCode()
        {
            return (x.ToString("0.0") + y.ToString("0.0") + z.ToString("0.0")).GetHashCode();
        }

        public Point Project(float radius, float scale = 1.0f)
        {
            scale = Mathf.Max(0, Mathf.Min(1, scale));

            var mag = Mathf.Sqrt(Mathf.Pow(this.x, 2) + Mathf.Pow(this.y, 2) + Mathf.Pow(this.z, 2));
            var ratio = radius / mag;
            this.x = this.x * ratio * scale;
            this.y = this.y * ratio * scale;
            this.z = this.z * ratio * scale;
            return this;
        }

        public void RegisterFace(Face face)
        {
            this.faces.Add(face);
        } 

        public List<Face> GetOrderedFaces()
        {
            var workingList = new List<Face>(faces);
            var ret = new List<Face>();

            var i = 0;
            while (i < faces.Count)
            {
                if (i == 0)
                {
                    ret.Add(workingList[i]);
                    workingList.RemoveAt(i);
                }
                else
                {
                    for(var j = 0; j < workingList.Count; j++)
                    {
                        if(i <= ret.Count && workingList[j].IsAdjacentTo(ret[i - 1]))
                        {
                            ret.Add(workingList[j]);
                            workingList.RemoveAt(j);
                            break;
                        }
                    }
                }
                i++;
            }
            return ret;
        }

        public Vector3 AsVector()
        {
            return new Vector3(x, y, z);
        }

    }
}