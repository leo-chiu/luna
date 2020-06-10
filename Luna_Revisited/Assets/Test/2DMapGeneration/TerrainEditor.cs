using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TerrainGenerator))]
public class TerrainEditor : Editor
{
    public override void OnInspectorGUI()
    {
        TerrainGenerator mapGen = (TerrainGenerator)target;

        if (DrawDefaultInspector())
        {
            if (mapGen.autoUpdate)
            {
                mapGen.resetTerrain();
                mapGen.RedrawMap();
            }
        }


        if (GUILayout.Button("Generate"))
        {
            mapGen.RedrawMap();

            if (GUILayout.Button("Reset"))
            {
                mapGen.resetTerrain();
            }
        }
    }
}
