using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public int mapWidth;
    public int mapHeight;
    public float scale;


    public int octaves;

    public float lacunarity;
    [Range(0,1)]
    public float persistance;

    public int seed;

    public float mesh_height_factor;

    public bool autoUpdate;
    
    public void GenerateMap()
    {
        float[,] noiseMap = Noise.GenerateNoiseMap(mapWidth, mapHeight, scale, octaves , lacunarity, persistance, seed);
        MapDisplay display = FindObjectOfType<MapDisplay>();
        display.DrawNoiseMap(noiseMap);
        display.DrawMesh(MeshGenerator.GenerateMesh(noiseMap, mesh_height_factor));
    }


    void OnValidate()
    {
        if (mapWidth <= 0)
        {
            mapWidth = 1;
        }
        if(mapHeight <= 0)
        {
            mapHeight = 1;
        }
        if(lacunarity < 1)
        {
            lacunarity = 1;
        }
        if(octaves < 0)
        {
            octaves = 0;
        }
    }
}
