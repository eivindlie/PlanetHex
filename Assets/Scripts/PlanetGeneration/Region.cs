using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace PlanetGeneration
{
    public class Region
    {
        public Vector3 Center;
        private List<Tile> Tiles;
        public int ID;

        public Region(int id)
        {
            this.ID = id;
            this.Tiles = new List<Tile>();
        }

        public void CalculateCenter()
        {
            var x = Tiles.Select(t => t.Center.x).Average();
            var y = Tiles.Select(t => t.Center.y).Average();
            var z = Tiles.Select(t => t.Center.z).Average();

            Center = new Vector3(x, y, z);
        }

        public void AddTile(Tile tile)
        {
            Tiles.Add(tile);
        }

        public List<Tile> GetTiles()
        {
            return Tiles;
        }
    }
}
