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
            var planet = new PlanetGenerator(new PlanetGeneratorSettings()).Generate();
            
            foreach(var (region, regionIndex) in planet.Regions.WithIndex())
            foreach (var (chunk, chunkIndex) in region.Chunks.WithIndex())
            {
                ChunkRenderer.Render(planet, regionIndex, chunkIndex);
            }
        }
    }
}
