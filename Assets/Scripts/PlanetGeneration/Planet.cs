using UnityEngine;
using System.Collections.Generic;

namespace PlanetGeneration 
{
    public class Planet 
    {
        public int Radius;
        public int Mass;
        public int TerrainHeight;
        private float Density = 1.0f;

        public Region[] Regions;
        
        public Planet(int radius, int terrainHeight) 
        {
            Radius = radius;
            Mass = (int)(Density * 4 * Mathf.PI * Mathf.Pow(Radius, 3)) / 3;
            TerrainHeight = terrainHeight;
        }

        public static void Load(string path) 
        {

        }
    }
}
