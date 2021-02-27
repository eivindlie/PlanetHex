using System.Collections.Generic;
using System.Linq;

using MathHelpers;

using Models.Planet;

using PlanetGeneration;

using Unity.Jobs;

using UnityEngine;

namespace Rendering
{
    public class PlanetController
    {
        private const int RenderDistance = 100;
        private const int UnrenderDistance = 150;

        private readonly Planet _planet;
        private readonly GameObject _player;

        private readonly Dictionary<int, IEnumerable<GameObject>> _renderedRegions =
            new Dictionary<int, IEnumerable<GameObject>>();

        public PlanetController(Planet planet, GameObject player)
        {
            _planet = planet;
            _player = player;
        }

        public PlanetController(PlanetGeneratorSettings generatorSettings, GameObject player)
        {
            _planet = new PlanetGenerator(generatorSettings).Generate();
            _player = player;
        }

        public void UpdateRenderedRegions()
        {
            var regionsToRender = _planet.Regions.Select(r => (r, CalculateRenderPriority(r)))
                .Where(r => r.Item2 > 0)
                .OrderByDescending(r => r.Item2)
                .Select(r => r.Item1)
                .ToList();
            var regionsToUnrender = _planet.Regions.Where(ShouldUnrenderRegion).ToList();

            RenderRegions(regionsToRender);
            UnrenderRegions(regionsToUnrender);
        }

        private float CalculateRenderPriority(Region region)
        {
            if (_renderedRegions.ContainsKey(region.RegionNumber)) return 0;

            var distance = SphereHelpers.DistanceBetweenPoints(region.Center.AsVector(), _player.transform.position,
                _planet.BaseRadius);

            if (distance > RenderDistance) return 0;

            return 1 / distance;
        }

        private bool ShouldUnrenderRegion(Region region)
        {
            if (!_renderedRegions.ContainsKey(region.RegionNumber)) return false;

            var distance = SphereHelpers.DistanceBetweenPoints(region.Center.AsVector(), _player.transform.position,
                _planet.BaseRadius);

            return distance > UnrenderDistance;
        }

        private void RenderRegions(IEnumerable<Region> regions)
        {
            foreach (var region in regions)
            {
                var gameObjects = new List<GameObject>();
                foreach (var chunk in region.Chunks)
                {
                    gameObjects.AddRange(ChunkRenderer.Render(_planet, region.RegionNumber, chunk.ChunkNumber));
                }

                _renderedRegions.Add(region.RegionNumber, gameObjects);
            }
        }

        private void UnrenderRegions(IEnumerable<Region> regions)
        {
            foreach (var region in regions)
            {
                var gameObjects = _renderedRegions[region.RegionNumber];
                foreach (var go in gameObjects)
                {
                    Object.Destroy(go);
                }

                _renderedRegions.Remove(region.RegionNumber);
            }
        }
    }
}
