using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NoiseGenerator
{

    public static float[,] GenerateNoiseMap(int number_maps, int map_width, float scale, int seed, int octaves, float persistance, float lacunarity)
    {
        float[,] noiseMap = new float[number_maps,map_width];

        System.Random rng = new System.Random(seed);
        Vector2[] octaveOffsets = new Vector2[octaves];
        for (int i = 0; i < octaves; i++)
        {
            float offsetX = rng.Next(-10000, 10000);
            float offsetY = rng.Next(-10000, 10000);
            octaveOffsets[i] = new Vector2(offsetX, offsetY);
        }

        if (scale <= 0)
        {
            scale = 0.0001f;
        }

        float maxNoiseHeight = float.MinValue;
        float minNoiseHeight = float.MaxValue;

        float widthHalf = number_maps / 2f;
        float heightHalf = map_width / 2f;

        for (int y = 0; y < map_width; y++)
        {
            for (int x = 0; x < number_maps; x++)
            {
                float amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;

                for (int i = 0; i < octaves; i++)
                {
                    float indexX = (x - widthHalf) / scale * frequency + octaveOffsets[i].x;
                    float indexY = (y - heightHalf) / scale * frequency + octaveOffsets[i].y;

                    float perlinValue = Mathf.PerlinNoise(indexX, indexY) * 2 - 1;

                    noiseHeight += perlinValue * amplitude;

                    amplitude *= persistance;
                    frequency *= lacunarity;
                }

                if (noiseHeight > maxNoiseHeight)
                {
                    maxNoiseHeight = noiseHeight;
                }
                else if (noiseHeight < minNoiseHeight)
                {
                    minNoiseHeight = noiseHeight;
                }

                noiseMap[x, y] = noiseHeight;
            }
        }

        for(int x = 0; x < number_maps; x++)
        {
            for(int y = 0; y < map_width; y++)
            {
                noiseMap[x, y] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseMap[x, y]);
            }
        }

        return noiseMap;
    }
}
