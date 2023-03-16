using Game.Utility;
using UnityEngine;
using Random = System.Random;

namespace Utility {
    public static class Noise {
        public static float[,,] GenerateNoiseMap(int width, int height, int seed, float scale, int octaves,
            float persistence, float lacunarity, int depth, Vector3 offset) 
        {
            var prng = new Random(seed);
            var octaveOffsets = new Vector3[octaves];
            for (var i = 0; i < octaves; i++) 
            {
                var offsetX = prng.Next(-100000, 100000) + offset.x;
                var offsetY = prng.Next(-100000, 100000) + offset.y;
                var offsetZ = prng.Next(-100000, 100000) + offset.z;
                octaveOffsets[i] = new Vector3(offsetX, offsetY, offsetZ);
            }

            var noiseMap = new float[width, height, depth];
            if (scale <= 0) 
            {
                scale = 0.0001f;
            }

            var maxNoise = float.MinValue;
            var minNoise = float.MaxValue;
            for (var x = 0; x < width; x++) 
            {
                for (var y = 0; y < height; y++) 
                {
                    for (var z = 0; z < depth; z++) 
                    {
                        float amplitude = 1;
                        float frequency = 1;
                        float noiseHeight = 0;
                        for (var i = 0; i < octaves; i++) 
                        {
                            var sampleX = x / scale * frequency + octaveOffsets[i].x;
                            var sampleY = y / (scale * 5f) * frequency + octaveOffsets[i].y;
                            var sampleZ = z / scale * frequency + octaveOffsets[i].z;

                            var noise = SimplexNoise.noise(sampleX, sampleY, sampleZ);
//                    float noise = Mathf.PerlinNoise(sampleX, sampleY);
                            noiseHeight += noise * amplitude;
                            amplitude *= persistence;
                            frequency *= lacunarity;
                        }

                        if (noiseHeight > maxNoise)
                            maxNoise = noiseHeight;
                        if (noiseHeight < minNoise)
                            minNoise = noiseHeight;
                        noiseMap[x, y, z] = noiseHeight;
                    }
                }
            }

            for (var x = 0; x < width; x++) 
            {
                for (var y = 0; y < height; y++) 
                {
                    for (var z = 0; z < depth; z++) 
                    {
                        noiseMap[x, y, z] = Mathf.InverseLerp(minNoise, maxNoise, noiseMap[x, y, z]);
                    }
                }
            }

            return noiseMap;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="seed"></param>
        /// <param name="scale"></param>
        /// <param name="octaves"></param>
        /// <param name="persistence"></param>
        /// <param name="lacunarity"></param>
        /// <param name="offset"></param>
        /// <complexity>O(145n²)</complexity>
        /// <returns></returns>
        public static float[,] GenerateNoiseMap(int width, int height, int seed, float scale, int octaves,
            float persistence, float lacunarity, Vector3 offset) 
        {
            var prng = new Random(seed);
            var octaveOffsets = new Vector3[octaves];
            for (var i = 0; i < octaves; i++) //O(4)
            {
                var offsetX = prng.Next(-100000, 100000) + offset.x;
                var offsetY = prng.Next(-100000, 100000) + offset.y;
                var offsetZ = prng.Next(-100000, 100000) + offset.z;
                octaveOffsets[i] = new Vector3(offsetX, offsetY, offsetZ);
            }

            var noiseMap = new float[width, height];
            if (scale <= 0)
            {
                scale = 0.0001f;
            }

            var maxNoise = float.MinValue;
            var minNoise = float.MaxValue;
            for (var x = 0; x < width; x++) //O(144*n²)
            {
                for (var z = 0; z < height; z++) //O(n*12)
                {
                    float amplitude = 1;
                    float frequency = 1;
                    float noiseHeight = 0;
                    for (var i = 0; i < octaves; i++)  //O(6)
                    {
                        var sampleX = x / scale * frequency + octaveOffsets[i].x;
                        var sampleZ = z / scale * frequency + octaveOffsets[i].z;

                        var noise = Mathf.PerlinNoise(sampleX, sampleZ);
                        noiseHeight += noise * amplitude;
                        amplitude *= persistence;
                        frequency *= lacunarity;
                    }

                    if (noiseHeight > maxNoise)
                        maxNoise = noiseHeight;
                    if (noiseHeight < minNoise)
                        minNoise = noiseHeight;
                    noiseMap[x, z] = noiseHeight;
                }
            }

            for (var x = 0; x < width; x++) //O(n²)
            {
                for (var z = 0; z < height; z++) //O(n)
                {
                    noiseMap[x, z] = Mathf.InverseLerp(minNoise, maxNoise, noiseMap[x, z]);
                }
            }

            return noiseMap;
        }
    }
}