using PlanetHex.Common.Extensions;
using PlanetHex.PlanetGeneration.Models.HexSphere;
using PlanetHex.PlanetGeneration.Models.Planet;
using PlanetHex.PlanetGeneration.Noise;

namespace PlanetHex.PlanetGeneration
{
    public class PlanetGenerator
    {
        private readonly PlanetGeneratorSettings _settings;
        private readonly SimplexNoiseGenerator _noiseGenerator = new();

        public PlanetGenerator(PlanetGeneratorSettings settings)
        {
            _settings = settings;
        }

        public Planet Generate()
        {
            var hexSphere = new HexSphereGenerator(_settings.BaseRadius, _settings.Divisions).Generate();

            //var regions = hexSphere.Regions.Select(GenerateRegion).ToArray();

            return new Planet
            {
                BlockHeight = _settings.BlockHeight,
                ChunkHeight = _settings.ChunkHeight,
                BaseRadius = _settings.BaseRadius,
                HeightLimit = _settings.HeightLimit,

                HexSphere = hexSphere,
                //Regions = regions
            };
        }

        // private Region GenerateRegion(TileRegion tileRegion, int regionNumber)
        // {
        //     var region = GenerateEmptyRegion(tileRegion, regionNumber);
        //     foreach (var (tile, tileIndex) in tileRegion.Tiles.WithIndex())
        //     {
        //         var tileStack = GenerateTileStack(tile);
        //
        //         foreach (var (chunk, chunkIndex) in region.Chunks.WithIndex())
        //         {
        //             foreach (var (layer, layerIndex) in chunk.Layers.WithIndex())
        //             {
        //                 layer.Blocks[tileIndex] = tileStack[chunkIndex * _settings.ChunkHeight + layerIndex];
        //             }
        //         }
        //     }
        //
        //     return region;
        // }

        private Region GenerateEmptyRegion(TileRegion tileRegion, int regionNumber)
        {
            var numChunks = (int) Math.Ceiling((float) _settings.HeightLimit / _settings.ChunkHeight);
            return new Region
            {
                Chunks = Enumerable.Range(0, numChunks)
                    .Select(chunkNumber => GenerateEmptyChunk(tileRegion, chunkNumber))
                    .ToArray(),
                RegionNumber = regionNumber,
                Center = tileRegion.Center,
            };
        }

        private Chunk GenerateEmptyChunk(TileRegion tileRegion, int chunkNumber)
        {
            return new Chunk
            {
                Layers = Enumerable.Range(0, _settings.ChunkHeight)
                    .Select(layerNumber => GenerateEmptyLayer(tileRegion, layerNumber))
                    .ToArray(),
                ChunkNumber = chunkNumber
            };
        }

        private Layer GenerateEmptyLayer(TileRegion tileRegion, int layerNumber)
        {
            return new Layer
            {
                Blocks = new int[tileRegion.Tiles.Count],
                LayerNumber = layerNumber
            };
        }

        // private List<int> GenerateTileStack(Tile tile)
        // {
        //     var height = _noiseGenerator.GetDensity(tile.Center.AsVector(), 1, _settings.HeightLimit, octaves: 3,
        //         multiplier: _settings.BaseRadius / 4, persistence: 0.6f);
        //     var numChunks = (int) Math.Ceiling((float) _settings.HeightLimit / _settings.ChunkHeight);
        //     return Enumerable.Range(0, numChunks * _settings.ChunkHeight)
        //         .Select((x, i) => i < height ? GreenBlock.BlockId : AirBlock.BlockId)
        //         .ToList();
        // }
    }
}
