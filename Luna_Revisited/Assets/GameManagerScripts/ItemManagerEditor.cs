using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

[CustomEditor(typeof(ItemManager))]
public class ItemManagerEditor : Editor
{
    private TextAsset file;
    private string[] lines;

    public void CreateItemAssets()
    {

        file = Resources.Load<TextAsset>("CSV Files/ItemSheet");
        if (file == null)
        {
            Debug.Log("ItemSheet not found");
            return;
        }
        lines = file.text.Split(new char[] { '\n' });
        for (int i = 1; i < lines.Length; i++)
        {
            string[] description_split = lines[i].Split('"', '"');

            bool description_has_commas = false;

            if (description_split.Length > 2)
            {
                description_has_commas = true;
            }

            string[] row = description_split[0].Split(',');

            Item item = (Item)ScriptableObject.CreateInstance("Item");

            if(row[0] == "Equipment")
            {
                item = (Equipment)ScriptableObject.CreateInstance("Equipment");
                ((Equipment)item).slot = (EquipmentSlot)System.Enum.Parse(typeof(EquipmentSlot), row[5]);
            }else if(row[0] == "Projectile")
            {
                item = (Projectile)ScriptableObject.CreateInstance("Projectile");
                int.TryParse(row[8], out ((Projectile)item).attack_boost);
            }

            item.name = row[1];

            int.TryParse(row[2], out item.item_id);

            int.TryParse(row[3], out item.stack_capacity);

            bool.TryParse(row[4], out item.is_default_item);

            if(description_has_commas)
                item.description = description_split[1];
            else
                item.description = row[9];

            string item_sprite_file_path = "Sprites/Items/" + item.name;
            item.sprite = Resources.Load<Sprite>(item_sprite_file_path);

            string fileName = item.name + ".asset";
            var pathName = "Assets/Resources/ItemAssets/Items/" + fileName;
            AssetDatabase.CreateAsset(item, pathName);
        }
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if(GUILayout.Button("Load Items from CSV"))
        {
            Debug.Log("Loading Items from CSV");

            CreateItemAssets();
        }
        if (GUILayout.Button("Testing"))  
        {
            AssetDatabase.Refresh(); 
            Debug.Log(AssetDatabase.LoadAssetAtPath<Quest>("Assets/Resources/Quests/newquest2.asset").kill_objectives[0].GetType());
            Debug.Log(AssetDatabase.LoadAssetAtPath<Quest>("Assets/Resources/Quests/newquest2.asset").collect_objectives[0].GetType());
        }
    }
}
