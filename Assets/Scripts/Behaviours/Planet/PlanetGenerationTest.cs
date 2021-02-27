using Extensions;

using PlanetGeneration;

using Rendering;

using UnityEngine;

namespace Behaviours.Planet
{
    public class PlanetGenerationTest : MonoBehaviour
    {
        public void Start()
        {
            var settings = new PlanetGeneratorSettings
            {
                HeightLimit = 5,
                BlockHeight = 3,
            };
            
            var planet = new PlanetGenerator(settings).Generate();
            
            foreach(var (region, regionIndex) in planet.Regions.WithIndex())
            foreach (var (chunk, chunkIndex) in region.Chunks.WithIndex())
            {
                ChunkRenderer.Render(planet, regionIndex, chunkIndex);
            }
        }
    }
}
