using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using Utilities;

namespace PlanetGeneration
{


    public class PlanetGenerator
    {
        public Material[] Materials;

        public static Region[] Generate(int radius, int terrainHeight, int? heightLimit0 = null)
        {
            int numDivisions = (int)Mathf.Sqrt((Mathf.PI * Mathf.Pow(radius, 2)) / 5);

            int heightLimit = (heightLimit0 == null) ? terrainHeight + 20 : (int)heightLimit0;

            Hexasphere hexasphere = new Hexasphere(radius, numDivisions);
            Region[] regions = hexasphere.Regions;

            var noiseGenerator = new SimplexNoiseGenerator();

            foreach (var region in regions)
            {
                region.GenerateTerrain(noiseGenerator, terrainHeight, heightLimit, radius);
            }

            return regions;
        }
    }
}

