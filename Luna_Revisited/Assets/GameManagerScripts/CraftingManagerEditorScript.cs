using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CraftingManager))]
public class CraftingManagerEditorScript : Editor
{
    private TextAsset file;
    private string[] lines;

    public void CreateRecipeAssets()
    {
        file = Resources.Load<TextAsset>("CSV Files/RecipeSheet");
        if (file == null)
        {
            Debug.Log("RecipeSheet not found");
            return;
        }
        lines = file.text.Split(new char[] { '\n' });
        for (int i = 1; i < lines.Length - 1; i++)
        {
            string[] row = lines[i].Split(new char[] { ',' });

            string item_path = "ItemAssets/Items/" + row[0];
            Item item = Resources.Load<Item>(item_path);

            if (item == null)
            {
                Debug.Log(row[0] + " item does not exist");
                continue;
            }

            ItemRecipe recipe = (ItemRecipe)ScriptableObject.CreateInstance("ItemRecipe");
            recipe.item = item;
            recipe.recipe = new List<KeyValuePair<Item, int>>();

            string[] ingredient_items = row[1].Split(new char[] { ' ' });
            for (int b = 0; b < ingredient_items.Length; b++)
            {
                if (ingredient_items[b].Length > 1)
                {
                    string[] ingredient = (ingredient_items[b].Split('(', ')'));
                    int.TryParse(ingredient[1], out int count);
                    Debug.Log(count + " of " + ingredient[0]);
                    string ingredient_name = ingredient[0];

                    string ingredient_path = "ItemAssets/Items/" + ingredient_name;
                    Item ingredient_item = Resources.Load<Item>(ingredient_path);
                    if (ingredient_item == null)
                    {
                        Debug.Log("Ingredient item " + ingredient_name + " not found, recipe aborted");
                    }
                    recipe.recipe.Add(new KeyValuePair<Item, int>(ingredient_item, count));
                }
            }
            AssetDatabase.CreateAsset(recipe, "Assets/Resources/ItemAssets/Recipes/" + row[0] + "_Recipe.asset");
        }
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("Generate Crafting Recipes from CSV"))
        {
            CreateRecipeAssets();
        }

    }
}
