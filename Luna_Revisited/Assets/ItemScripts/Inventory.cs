using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Inventory : MonoBehaviour
{
    #region Singleton
    public static Inventory instance { get; set; }

    public void Awake()
    {
        if(instance!= null)
        {
            return;
        }
        instance = this;
    }
    #endregion

    public List<ItemStack> items;
    public Dictionary<int, int> item_store;

    public int money;
    public int max_money;

    public int capacity;

    public event Action onItemCallback;

    public void Start()
    {
        items = new List<ItemStack>();
        item_store = new Dictionary<int, int>();

        foreach(KeyValuePair<int, Item> itemPair in ItemManager.instance.all_items)
        {
            if (!item_store.ContainsKey(itemPair.Key)) {
                item_store.Add(itemPair.Value.item_id, 0);
            }
        }
    }

    public bool canPickup(ItemStack item_stack)
    {
        Item item = item_stack.item;
        int count = item_stack.count;

        // in the case that there are no more open indices
        if (items.Count >= capacity)
        {
            // if count will take up at least one slot, then we know that we can't add it to the inventory, as there are no more slots available
            if (count / item.stack_capacity >= 1) return false;
            // check if the player has this item in their inventory
            if (item_store.ContainsKey(item.item_id))
            {
                // if they do, then check their smallest stack
                int remainder = item_store[item.item_id] % item.stack_capacity;
                if (remainder != 0)
                {
                    // if there is a non-full stack, check how full it is
                    int space_in_stack = item.stack_capacity - remainder;
                    // return whether or not the space in the stack is larger than or equal to the space that will be taken up
                    if(space_in_stack >= count)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    // if there are only full stacks in the inventory, we know that the player can't fit any more of that item into the inventory
                    return false;
                }
            }
        }
        // in the case that there are still open indices
        else
        {
            // if we were to add all the items in, they would fill up at least the ceil of the division, (i.e. 32/3 = 11 slots)
            int additional_slots = (int)(Mathf.Ceil(count / item.stack_capacity));
            // if the capacity is exceeded after adding these additional slots, then we have to check for existing stacks
            if (additional_slots + items.Count >= capacity)
            {
                // if the excess is at most 1 slot
                if (additional_slots + items.Count - capacity <= 1)
                {
                    int remainder = item_store[item.item_id] % item.stack_capacity;
                    if (remainder != 0)
                    {
                        // if there is a non-full stack, check how full it is
                        int space_in_stack = item.stack_capacity - remainder;
                        // return whether or not the space in the stack is larger than or equal to the space that will be taken up
                        if (space_in_stack >= count)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        // if there are only full stacks in the inventory, we know that the player can't fit any more of that item into the inventory
                        return false;
                    }
                }
                // if the excess is more than 1 slot 
                else
                {
                    return false;
                }
            }
            // if the capacity isn't exceeded even after adding all the items
            else
            {
                // then we know for certain that the inventory can fit the additional items entirely
                return true;
            }
        }
        return false;
    }

    public bool canPickupItems(List<ItemStack> items_to_pickup)
    {
        bool able = true;
        foreach (ItemStack s in items_to_pickup)
        {
            if(!canPickup(new ItemStack(s.item, s.count)))
            {
                able = false;
            }
        }
        return able;
    }

    public int get_instances_of_item(int item_id)
    {
        if (item_store.ContainsKey(item_id))
            return item_store[item_id];
        return 0;
    }

    private int fillUpExisting(int item_id, int count)
    {
        // this list will contain the subset of all items currently in the inventory that match the id of the drop
        List<KeyValuePair<ItemStack, int>> subset = new List<KeyValuePair<ItemStack, int>>();

        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].item_id == item_id)
            {
                subset.Add(new KeyValuePair<ItemStack, int>(items[i], i));
            }
        }

        // if there are no existing stacks of that item
        if (subset.Count == 0) 
        {
            // you have to loot from scratch
            return count;
        }

        int left_to_loot = count;

        // otherwise we need to sort this list from greatest to least and insert until either there is no more to loot or we have run out of existing stacks 
        subset.Sort((x, y) => x.Key.count.CompareTo(y.Key.count));

        for(int i = 0; i < subset.Count; i++)
        {
            // if the stack's current count is less than that of the stack capacity, it means there's space to insert some items
            if(subset[i].Key.count < subset[i].Key.item.stack_capacity)
            {
                int slot_index = subset[i].Value;
                // the difference between that item's capacity and its current count is the amount of space that can be filled
                int available_space = subset[i].Key.item.stack_capacity - subset[i].Key.count;
                // if there is more to loot than is in this available space
                if (left_to_loot > available_space)
                {
                    // there will be less to loot after using some of it to fill up this stack
                    left_to_loot -= available_space;
                    // the considered stack will increase to full capacity
                    items[slot_index].count += available_space;
                    item_store[item_id] += available_space;
                }
                // if this existing stack has enough space in it to loot the rest of what's required
                else
                {
                    // there is nothing left to loot, as this stack has looted the rest
                    left_to_loot = 0;
                    // the considered stack will fill with what's left
                    items[slot_index].count += left_to_loot;
                    item_store[item_id] += left_to_loot;
                }
            }
        }

        // after we've exhausted all existing stacks 
        return left_to_loot;
    }

    public int fillUpNonExisting(Item item, int left_to_loot)
    {
        // this function is not intended to add to the counts of stacks that already exist in the inventory, rather it should just keep checking the capacity while inserting new item stacks
        int stack_capacity = item.stack_capacity;
        // this loop will end either if we have looted everything or we have reached a full inventory
        while (left_to_loot > 0 && items.Count < capacity)
        {
            // if what's left to loot is greater than what can be fit into one stack
            if (left_to_loot > stack_capacity)
            {
                // add this full stack into the inventory
                items.Add(new ItemStack(item, stack_capacity));
                // record this in the item store 
                item_store[item.item_id] += stack_capacity;
                // subtract one stack's count from what's left to loot
                left_to_loot -= stack_capacity;
            }
            // if what's left to loot is less than or equal to one stack
            else if (left_to_loot <= stack_capacity)
            {
                // add this possible full/partial stack to the inventory
                items.Add(new ItemStack(item, left_to_loot));
                // record this in the item store
                item_store[item.item_id] += left_to_loot;
                // there is nothing left to loot
                left_to_loot = 0;
            }
        }

        return left_to_loot;
    }

    public void recieveItem(ItemStack item)
    {
        int left_to_loot = fillUpExisting(item.item_id, item.count);
        fillUpNonExisting(item.item, left_to_loot);
    }

    public void Loot(ItemPickup drop)
    {
        Item item = drop.item;
        int item_id = item.item_id;
        int count = drop.count;

        int left_to_loot = count;

        left_to_loot = fillUpExisting(item_id, count);

        left_to_loot = fillUpNonExisting(item, left_to_loot);

        drop.count = left_to_loot;

        onItemCallback();
    }

    public bool RemoveItem(int item, int count)
    {
        // if there isn't at least a count number of items of that id in the inventory, we can't remove it
        if (get_instances_of_item(item) < count) return false;
        // otherwise look for the smallest stack of the item, to do so we will first sort a subset of all itemstacks that share that item's id
        List<KeyValuePair<ItemStack, int>> subset = new List<KeyValuePair<ItemStack, int>>();
        for (int i = 0; i < items.Count; i++)
        {
            // if this slot's item id matches that of removal
            if(items[i].item_id == item)
            {
                // add it to the subset of the inventory containing only that item
                subset.Add(new KeyValuePair<ItemStack, int>(items[i], i));
            }
        }
        // sort the list of all stacks of matching item id by their count
        subset.Sort((x, y) => y.Key.count.CompareTo(x.Key.count));
        // by this point we will start by deducting from index 0 and work our way up the list until we have removed a count number of items
        int left_to_remove = count;
        int subset_index = 0;
        // while there're still items needing to be removed
        while(left_to_remove > 0)
        {
            int stack_count = subset[subset_index].Key.count;
            int item_id = subset[subset_index].Key.item_id;
            int slot_index = subset[subset_index].Value;

            // if the stack contains exactly what's needed to remove what's required
            if (left_to_remove == subset[subset_index].Key.count)
            {
                // first deduct it from the item store
                item_store[item_id] -= stack_count;
                // for completion
                left_to_remove = 0;
                // because we saved the slot index we can use it to remove precisely that slot from the inventory
                items.RemoveAt(slot_index);
                ItemCallback();
                return true;
            }
            // if there's more things in the stack than needs to be removed
            else if(left_to_remove < stack_count)
            {
                // the difference in the stack's current count and whatever is needed to be removed will become the new count value which that stack assumes
                int difference = stack_count - left_to_remove;
                // deduct from the item store
                item_store[item_id] -= left_to_remove;

                // for completion
                left_to_remove = 0;
                // we can use the saved slot index to reassign the count of that stack
                items[item_id].count = difference;
                ItemCallback();
                return true;
            }
            // if there's more things to be removed than exists in the stack
            else if(left_to_remove > stack_count)
            {
                /*  we will need to index the subset index and allow the loop to iterate, as there's not enough from this stack alone to fill the removal quota
                    nonetheless we will have to remove this stack in its entirety and count it towards the quota
                    first deduct it from the item store
                */
                item_store[item_id] -= stack_count;
                // what's left to be removed is the difference between what's currently left and the stack count
                left_to_remove -= stack_count;
                // because we saved the slot index we can use it to remove precisely that slot from the inventory
                items.RemoveAt(slot_index);
                // allow for iteration
                subset_index++;
            }
        }
        ItemCallback();
        return left_to_remove == 0;
    }

    public void RemoveAtSlot(int slot_index)
    {
        // first deduct from the item store the amount you will be taking out of the inventory
        item_store[items[slot_index].item_id] -= items[slot_index].count;
        // then remove it from the inventory;
        items.RemoveAt(slot_index);
    }

    public void ItemCallback()
    {
        if(onItemCallback != null)
            onItemCallback.Invoke();
    }

    public void SwapSlots(int a_index, int b_index)
    {
        // a and b have to be indices contained in the list of inventory items
        if (a_index < 0 || a_index >= items.Count) return;
        if (b_index < 0 || b_index >= items.Count) return;

        ItemStack a = items[a_index];
        items[a_index] = items[b_index];
        items[b_index] = a;

        ItemCallback();
    }
}
