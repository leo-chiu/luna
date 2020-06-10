using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingManager : MonoBehaviour
{
    public static CraftingManager instance { get; set; }

    [SerializeField]
    public Dictionary<Item, ItemRecipe> recipe_list;

    public void Awake()
    {
        if (instance == null) instance = this;
    }

    public void Start()
    {
        ItemRecipe[] recipes = Resources.LoadAll<ItemRecipe>("ItemAssets/Recipes");
        recipe_list = new Dictionary<Item, ItemRecipe>();
        for (int i = 0; i < recipes.Length; i++)
        {
            recipe_list.Add(recipes[i].item, recipes[i]);
        }
    }

    public bool canCraft(Item to_craft)
    {
        // if there's no recipe found for this item, they cannot craft it
        if (!recipe_list.ContainsKey(to_craft)) return false;
        // otherwise there is a recipe, so pull it out
        ItemRecipe recipe = recipe_list[to_craft];
        // extract from it the list of components needed for crafting
        List<KeyValuePair<Item, int>> grocery_list = recipe.recipe;
        // for each of these components
        foreach (KeyValuePair<Item, int> ingredient in grocery_list)
        {
            // if the player is short on even one of them
            if (Inventory.instance.item_store[ingredient.Key.item_id] < ingredient.Value)
            { 
                // the player cannot craft this item
                return false;
            }
            }
        // we've reached this point only if the player has all the necessary ingredients

        // if the player lacks the space in their inventory, they cannot craft this item
        if (!Inventory.instance.canPickup(new ItemStack(to_craft, recipe.production_count))) return false;

        // reaching this point of the code, we are guaranteed that the player has both the necessary ingredients and sufficient space in their inventory
        return true;
    }
        
    public void Craft(Item to_craft, ItemRecipe recipe)
    {
        int production = recipe.production_count;
        if (!canCraft(to_craft))
        {
            List<KeyValuePair<Item, int>> grocery_list = recipe_list[to_craft].recipe;
            foreach (KeyValuePair<Item, int> ingredient in grocery_list)
            {
                Inventory.instance.RemoveItem(ingredient.Key.item_id, ingredient.Value);
            }

            Inventory.instance.recieveItem(new ItemStack(to_craft, production));
        }
    }
}
