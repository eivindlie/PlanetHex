using System.Collections.Generic;
using System.Linq;

using Behaviours.Planet.Blocks.Impl;

using Extensions;

using Models.HexSphere;
using Models.Planet;

using UnityEngine;

namespace PlanetGeneration
{
    public class PlanetGenerator
    {
        private readonly PlanetGeneratorSettings _settings;

        public PlanetGenerator(PlanetGeneratorSettings settings)
        {
            _settings = settings;
        }

        public Planet Generate()
        {
            var hexSphere = new HexSphereGenerator(_settings.BaseRadius, _settings.Divisions).Generate();

            var regions = hexSphere.Regions.Select(GenerateRegion).ToArray();

            return new Planet
            {
                BlockHeight = _settings.BlockHeight,
                ChunkHeight = _settings.ChunkHeight,
                BaseRadius = _settings.BaseRadius,
                HeightLimit = _settings.HeightLimit,

                HexSphere = hexSphere,
                Regions = regions
            };
        }

        private Region GenerateRegion(TileRegion tileRegion)
        {
            var region = GenerateEmptyRegion(tileRegion);
            foreach (var (tile, tileIndex) in tileRegion.Tiles.WithIndex())
            {
                var tileStack = GenerateTileStack(tile);

                foreach (var (chunk, chunkIndex) in region.Chunks.WithIndex())
                {
                    foreach (var (layer, layerIndex) in chunk.Layers.WithIndex())
                    {
                        layer.Blocks[tileIndex] = tileStack[chunkIndex * _settings.ChunkHeight + layerIndex];
                    }
                }
            }

            return region;
        }

        private Region GenerateEmptyRegion(TileRegion tileRegion)
        {
            var numChunks = (int) Mathf.Ceil((float) _settings.HeightLimit / _settings.ChunkHeight);
            return new Region
            {
                Chunks = Enumerable.Range(0, numChunks).Select(_ => GenerateEmptyChunk(tileRegion)).ToArray()
            };
        }

        private Chunk GenerateEmptyChunk(TileRegion tileRegion)
        {
            return new Chunk
            {
                Layers = Enumerable.Range(0, _settings.ChunkHeight)
                    .Select(_ => GenerateEmptyLayer(tileRegion))
                    .ToArray()
            };
        }

        private Layer GenerateEmptyLayer(TileRegion tileRegion)
        {
            return new Layer
            {
                Blocks = new int[tileRegion.Tiles.Count]
            };
        }

        private List<int> GenerateTileStack(Tile tile)
        {
            var height = Random.Range(0, _settings.HeightLimit);
            var numChunks = (int) Mathf.Ceil((float) _settings.HeightLimit / _settings.ChunkHeight);
            return Enumerable.Range(0, numChunks * _settings.ChunkHeight)
                .Select((x, i) => i < height ? BlackBlock.BlockId : AirBlock.BlockId)
                .ToList();
        }
    }
}
