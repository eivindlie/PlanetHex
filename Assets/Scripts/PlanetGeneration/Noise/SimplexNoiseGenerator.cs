using UnityEngine;

namespace PlanetGeneration.Noise
{
    public class SimplexNoiseGenerator
    {
        private readonly int[] _a = new int[3];
        private float _s, _u, _v, _w;
        private int _i, _j, _k;
        private const float OneThird = 0.333333333f;
        private const float OneSixth = 0.166666667f;
        private readonly int[] _t;

        public SimplexNoiseGenerator()
        {
            if (_t != null) return;
            var rand = new System.Random();
            _t = new int[8];
            for (var q = 0; q < 8; q++)
                _t[q] = rand.Next();
        }

        public SimplexNoiseGenerator(string seed)
        {
            _t = new int[8];
            var seedParts = seed.Split(new char[] { ' ' });

            for (var q = 0; q < 8; q++)
            {
                int b;
                try
                {
                    b = int.Parse(seedParts[q]);
                }
                catch
                {
                    b = 0x0;
                }
                _t[q] = b;
            }
        }

        public SimplexNoiseGenerator(int[] seed)
        {
            // {0x16, 0x38, 0x32, 0x2c, 0x0d, 0x13, 0x07, 0x2a}
            _t = seed;
        }

        public string GetSeed()
        {
            string seed = "";

            for (int q = 0; q < 8; q++)
            {
                seed += _t[q].ToString();
                if (q < 7)
                    seed += " ";
            }

            return seed;
        }

        public float CoherentNoise(float x, float y, float z, int octaves = 1, int multiplier = 25,
            float amplitude = 0.5f, float lacunarity = 2, float persistence = 0.9f)
        {
            Vector3 v3 = new Vector3(x, y, z) / multiplier;
            float val = 0;
            for (int n = 0; n < octaves; n++)
            {
                val += Noise(v3.x, v3.y, v3.z) * amplitude;
                v3 *= lacunarity;
                amplitude *= persistence;
            }
            return val;
        }

        public int GETDensity(Vector3 loc)
        {
            var val = CoherentNoise(loc.x, loc.y, loc.z);
            return (int) Mathf.Lerp(0, 255, val);
        }

        public int GetDensity(Vector3 loc, int min, int max, int octaves = 1, int multiplier = 25,
            float amplitude = 0.5f, float lacunarity = 2, float persistence = 0.9f)
        {
            var val = Mathf.Abs(CoherentNoise(loc.x, loc.y, loc.z, octaves, multiplier, amplitude, lacunarity,
                persistence));
            return (int) Mathf.Lerp(min, max, val);
        }

        public float GetRawDensity(Vector3 loc)
        {
            return CoherentNoise(loc.x, loc.y, loc.z);
        }

        // Simplex Noise Generator
        public float Noise(float x, float y, float z)
        {
            _s = (x + y + z) * OneThird;
            _i = FastFloor(x + _s);
            _j = FastFloor(y + _s);
            _k = FastFloor(z + _s);

            _s = (_i + _j + _k) * OneSixth;
            _u = x - _i + _s;
            _v = y - _j + _s;
            _w = z - _k + _s;

            _a[0] = 0;
            _a[1] = 0;
            _a[2] = 0;

            int hi = _u >= _w ? _u >= _v ? 0 : 1 : _v >= _w ? 1 : 2;
            int lo = _u < _w ? _u < _v ? 0 : 1 : _v < _w ? 1 : 2;

            return Kay(hi) + Kay(3 - hi - lo) + Kay(lo) + Kay(0);
        }

        float Kay(int a)
        {
            _s = (_a[0] + _a[1] + _a[2]) * OneSixth;
            var x = _u - _a[0] + _s;
            var y = _v - _a[1] + _s;
            var z = _w - _a[2] + _s;
            var t = 0.6f - x * x - y * y - z * z;
            var h = Shuffle(_i + _a[0], _j + _a[1], _k + _a[2]);
            _a[a]++;
            if (t < 0) return 0;
            var b5 = h >> 5 & 1;
            var b4 = h >> 4 & 1;
            var b3 = h >> 3 & 1;
            var b2 = h >> 2 & 1;
            var b1 = h & 3;

            var p = b1 switch
            {
                1 => x,
                2 => y,
                _ => z
            };
            var q = b1 switch
            {
                1 => y,
                2 => z,
                _ => x
            };
            var r = b1 switch
            {
                1 => z,
                2 => x,
                _ => y
            };

            p = b5 == b3 ? -p : p;
            q = b5 == b4 ? -q : q;
            r = b5 != (b4 ^ b3) ? -r : r;
            t *= t;
            return 8 * t * t * (p + (b1 == 0 ? q + r : b2 == 0 ? q : r));
        }

        private int Shuffle(int i, int j, int k)
        {
            return B(i, j, k, 0) + B(j, k, i, 1) + B(k, i, j, 2) + B(i, j, k, 3) + B(j, k, i, 4) + B(k, i, j, 5) +
                   B(i, j, k, 6) + B(j, k, i, 7);
        }

        private int B(int i, int j, int k, int B)
        {
            return _t[SimplexNoiseGenerator.B(i, B) << 2 | SimplexNoiseGenerator.B(j, B) << 1 | SimplexNoiseGenerator.B(k, B)];
        }

        private static int B(int n, int b)
        {
            return n >> b & 1;
        }

        private static int FastFloor(float n)
        {
            return n > 0 ? (int) n : (int) n - 1;
        }
    }
}
