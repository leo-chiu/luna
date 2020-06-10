using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    public static EquipmentManager instance { get; private set; }

    public delegate void OnEquipmentChanged(Equipment new_item, Equipment old_item);
    public OnEquipmentChanged onEquipmentChanged;

    public Equipment[] equipment;

    public void Awake()
    {
        if (instance == null) instance = this;
    }

    public void Start()
    {
        int number_slots = System.Enum.GetNames(typeof(EquipmentSlot)).Length;
        equipment = new Equipment[number_slots];
    }

    public void Equip(Equipment equip)
    {

        Equipment old_equip = null; 

        if(equipment[(int)equip.slot] != null)
        {
            old_equip = equipment[(int)equip.slot];
        }

        equipment[(int)equip.slot] = equip;

        if(onEquipmentChanged != null)
        {
            onEquipmentChanged.Invoke(equip, old_equip);
        }
    }

    public void Unequip(int slot_index)
    {
        Equipment old_equip = null;
        if(equipment[slot_index] != null)
        {
            old_equip = equipment[slot_index];
            equipment[slot_index] = null;
        }

        if (onEquipmentChanged != null)
        {
            onEquipmentChanged.Invoke(null, old_equip);
        }
    }
}
