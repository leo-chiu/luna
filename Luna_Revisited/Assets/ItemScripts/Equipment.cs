using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Equipment", menuName = "Inventory/Equipment")] [System.Serializable]
public class Equipment : Item
{
    public EquipmentSlot slot;

    public int attack_modifier;
    public int armor_modifier;

    public override void Use()
    {
        EquipmentManager.instance.Equip(this);
        Debug.Log("Equipped");
    }

}
public enum EquipmentSlot {Helmet, Top, Bottom, Shoes, Gloves, Tool}