using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    public int number_maps;
    public int map_width;
    public float scale;
    public int seed;

    public int octaves; 
    public float persistance; 
    public float lacunarity;

    public float[,] noiseMap;

    public int max_amplitude;

    public bool autoUpdate;

    public int min_depth;

    private GameObject generated_terrain;

    public GameObject prefab_terrain;

    public List<GameObject> columns;

    private float prefab_terrain_width;
    private float prefab_terrain_height;

    void Start()
    {
        noiseMap = NoiseGenerator.GenerateNoiseMap(number_maps, map_width, scale, seed, octaves, persistance, lacunarity);
        prefab_terrain_width = prefab_terrain.transform.lossyScale.x * prefab_terrain.GetComponent<BoxCollider2D>().size.x;
        prefab_terrain_height = prefab_terrain.transform.lossyScale.y * prefab_terrain.GetComponent<BoxCollider2D>().size.y;
    }

    public void LoadMap()
    {
        noiseMap = NoiseGenerator.GenerateNoiseMap(number_maps, map_width, scale, seed, octaves, persistance, lacunarity);

        generated_terrain = new GameObject();
        generated_terrain.name = "Generated_Terrain";
        for (int map = 0; map < number_maps; map++)
        {
            for (int column = 0; column < map_width; column++)
            {
                int column_height = (int)(noiseMap[map,column] * max_amplitude) + min_depth;
                GameObject terrain = new GameObject();
                terrain.transform.SetParent(generated_terrain.transform);
                for (int i = 0; i < column_height; i++)
                {
                    Instantiate(prefab_terrain, new Vector3(-40f + column * prefab_terrain_width, -10f + prefab_terrain_height*i, 0), Quaternion.identity, terrain.transform);
                }
                terrain.name = "terrain_column";
                columns.Add(terrain);
            }
        }

    }

    public void resetTerrain()
    {
        Destroy(generated_terrain);
    }

    public void RedrawMap()
    {
        LoadMap();
    }
}
