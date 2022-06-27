using Microsoft.Xna.Framework;

using PlanetHex.Common.Extensions;
using PlanetHex.PlanetGeneration.Models.HexSphere;

using Point = PlanetHex.PlanetGeneration.Models.Shared.Point;

namespace PlanetHex.PlanetGeneration
{
    public class HexSphereGenerator
    {
        private readonly float _radius;
        private readonly int _numDivisions;
        private readonly float _hexSize;

        private readonly int _regionDivisions;

        private const int TilesPerRegion = 256;

        public HexSphereGenerator(float radius, int numDivisions, float hexSize = 1.0f)
        {
            _radius = radius;
            _numDivisions = numDivisions;
            _hexSize = hexSize;

            _regionDivisions =
                (int)Math.Ceiling(Math.Sqrt(Math.Pow(_numDivisions, 2) / TilesPerRegion));
        }

        public HexSphere Generate()
        {
            var polyhedron = CreateInitialPolyhedron();
            polyhedron = SubdividePolyhedron(polyhedron, _regionDivisions, false, false);
            polyhedron = SubdividePolyhedron(polyhedron, _numDivisions / _regionDivisions);
            var hexasphere = MapPolyhedronToHexasphere(polyhedron, _radius, _hexSize);

            return hexasphere;
        }

        private Polyhedron CreateInitialPolyhedron()
        {
            var tao = 1.61803399f;
            var corners = new[]
            {
                new HexSpherePoint(1000, tao * 1000, 0),
                new HexSpherePoint(-1000, tao * 1000, 0),
                new HexSpherePoint(1000, -tao * 1000, 0),
                new HexSpherePoint(-1000, -tao * 1000, 0),
                new HexSpherePoint(0, 1000, tao * 1000),
                new HexSpherePoint(0, -1000, tao * 1000),
                new HexSpherePoint(0, 1000, -tao * 1000),
                new HexSpherePoint(0, -1000, -tao * 1000),
                new HexSpherePoint(tao * 1000, 0, 1000),
                new HexSpherePoint(-tao * 1000, 0, 1000),
                new HexSpherePoint(tao * 1000, 0, -1000),
                new HexSpherePoint(-tao * 1000, 0, -1000)
            };

            var faces = new[]
            {
                new Face(corners[0], corners[1], corners[4]),
                new Face(corners[1], corners[9], corners[4]),
                new Face(corners[4], corners[9], corners[5]),
                new Face(corners[5], corners[9], corners[3]),
                new Face(corners[2], corners[3], corners[7]),
                new Face(corners[3], corners[2], corners[5]),
                new Face(corners[7], corners[10], corners[2]),
                new Face(corners[0], corners[8], corners[10]),
                new Face(corners[0], corners[4], corners[8]),
                new Face(corners[8], corners[2], corners[10]),
                new Face(corners[8], corners[4], corners[5]),
                new Face(corners[8], corners[5], corners[2]),
                new Face(corners[1], corners[0], corners[6]),
                new Face(corners[11], corners[1], corners[6]),
                new Face(corners[3], corners[9], corners[11]),
                new Face(corners[6], corners[10], corners[7]),
                new Face(corners[3], corners[11], corners[7]),
                new Face(corners[11], corners[6], corners[7]),
                new Face(corners[6], corners[0], corners[10]),
                new Face(corners[9], corners[1], corners[11])
            };

            return new Polyhedron
            {
                Corners = corners.ToList(),
                Faces = faces.ToList()
            };
        }

        private Polyhedron SubdividePolyhedron(Polyhedron initialPolyhedron, int divisions, bool registerFaces = true,
            bool registerRegions = true)
        {
            var addPoint = CreatePointAdder(initialPolyhedron.Corners);

            var newFaces = new List<Face>();

            foreach (var (face, index) in initialPolyhedron.Faces.WithIndex())
            {
                var dividedFaces = registerRegions
                    ? SubdivideFace(face, divisions, addPoint, registerFaces, index)
                    : SubdivideFace(face, divisions, addPoint, registerFaces);
                newFaces.AddRange(dividedFaces);
            }

            var newPoints = newFaces.SelectMany(f => f.Points).Distinct().ToList();

            return new Polyhedron
            {
                Corners = newPoints, Faces = newFaces
            };
        }

        /// <summary>
        /// Creates a closure that creates new points while ensuring the same point object is returned for the
        /// same coordinates each time.
        /// </summary>
        /// <param name="points">An optional initial list of points.</param>
        /// <returns></returns>
        private Func<HexSpherePoint, HexSpherePoint> CreatePointAdder(IEnumerable<HexSpherePoint>? points = null)
        {
            var pointsMap = new Dictionary<HexSpherePoint, HexSpherePoint>();
            if (points != null)
            {
                foreach (var p in points)
                {
                    pointsMap[p] = p;
                }
            }

            HexSpherePoint AddPoint(HexSpherePoint p)
            {
                if (!pointsMap.ContainsKey(p))
                {
                    pointsMap[p] = p;
                }
                return pointsMap[p];
            }

            return AddPoint;
        }

        /// <summary>
        /// Divides a triangular face into a number of smaller faces based on the provided number of divisions.
        /// Can be used both for division into large triangular regions, and for division into smaller triangles that
        /// will then be merged into the final hexagons of the sphere.
        /// </summary>
        /// <param name="face">The triangular face that will be subdivided.</param>
        /// <param name="divisions">The number of divisions along each edge of the face.</param>
        /// <param name="addPoint">A function that takes the coordinates of a point, and either returns an existing
        /// point matchin the coordinates, or returns a new point if none exist.</param>
        /// <param name="registerFaces"></param>
        /// <param name="regionId"></param>
        /// <returns></returns>
        private List<Face> SubdivideFace(Face face, int divisions, Func<HexSpherePoint, HexSpherePoint> addPoint,
            bool registerFaces = true, int? regionId = null)
        {
            var bottomEdge = new List<HexSpherePoint> { face.Points[0] };
            var leftEdge = SubdivideEdgeBetweenPoints(face.Points[0], face.Points[1], divisions, addPoint, regionId);
            var rightEdge = SubdivideEdgeBetweenPoints(face.Points[0], face.Points[2], divisions, addPoint, regionId);

            var currentRow = bottomEdge;

            var newFaces = new List<Face>();

            for (var i = 1; i <= divisions; i++)
            {
                var previousRow = currentRow;
                currentRow = SubdivideEdgeBetweenPoints(leftEdge[i], rightEdge[i], i, addPoint, regionId);

                for (var j = 0; j < i; j++)
                {
                    var newFace = new Face(previousRow[j], currentRow[j], currentRow[j + 1], registerFaces);
                    newFaces.Add(newFace);

                    if (j <= 0) continue;

                    newFace = new Face(previousRow[j - 1], previousRow[j], currentRow[j], registerFaces);
                    newFaces.Add(newFace);
                }
            }

            return newFaces;
        }

        /// <summary>
        /// Divides the edge between two points into a number of subdivisions, returning a list of the new points
        /// ordered along the adge from p1 to p2.
        /// </summary>
        /// <returns></returns>
        private List<HexSpherePoint> SubdivideEdgeBetweenPoints(HexSpherePoint p1, HexSpherePoint p2, int divisions,
            Func<HexSpherePoint, HexSpherePoint> addPoint, int? regionId = null)
        {
            p1.Region = regionId;
            p2.Region = regionId;

            var points = new List<HexSpherePoint> { p1 };

            for (var i = 1; i < divisions; i++)
            {
                var ratio = (float)i / divisions;
                var newPoint = addPoint(Segment(p1, p2, ratio));
                newPoint.Region = regionId;
                points.Add(newPoint);
            }
            points.Add(p2);

            return points;
        }

        private HexSphere MapPolyhedronToHexasphere(Polyhedron polyhedron, float radius, float hexSize)
        {
            var numRegions = polyhedron.Corners.Select(p => p.Region ?? 0).Max() + 1;
            var regions = Enumerable.Range(0, numRegions)
                .Select(_ => new TileRegion
                {
                    Tiles = new List<Tile>()
                })
                .ToList();

            foreach (var p in polyhedron.Corners)
            {
                var center = PointHelpers.ProjectToRadius(p, radius);
                var faces = PointHelpers.GetOrderedFaces(center);
                var boundaryPoints = faces
                    .Select(f => Segment(center, f.Centroid, hexSize))
                    .Select(p => PointHelpers.ProjectToRadius(p, radius))
                    .ToList();

                var tile = new Tile
                {
                    Center = center,
                    Boundary = boundaryPoints.Select(p2 => p2 as Point).ToList()
                };

                if (!IsPoitingAwayFromOrigin(tile))
                {
                    tile.Boundary.Reverse();
                }

                var regionId = p.Region ?? 0;
                regions[regionId].Tiles.Add(tile);
            }

            return new HexSphere
            {
                Regions = regions
            };
        }

        private bool IsPoitingAwayFromOrigin(Tile tile)
        {
            var U = tile.Boundary[1].AsVector() - tile.Boundary[0].AsVector();
            var V = tile.Boundary[2].AsVector() - tile.Boundary[0].AsVector();

            var normal = Vector3.Cross(U, V);
            return (tile.Center.X * normal.X >= 0) && (tile.Center.Y * normal.Y >= 0) &&
                   (tile.Center.Z * normal.Z >= 0);
        }

        private HexSpherePoint Segment(HexSpherePoint p1, HexSpherePoint p2, float ratio)
        {
            var x = p1.X * (1 - ratio) + p2.X * ratio;
            var y = p1.Y * (1 - ratio) + p2.Y * ratio;
            var z = p1.Z * (1 - ratio) + p2.Z * ratio;
            return new HexSpherePoint(x, y, z);
        }
    }
}
