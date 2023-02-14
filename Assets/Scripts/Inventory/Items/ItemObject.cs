using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Healing,
    Quest
}

public abstract class ItemObject : ScriptableObject
{
    public int ID;
    public string itemName;
    public Sprite iconDisplay;
    public ItemType type;
    [TextArea(5,10)]
    public string description;

    public Item CreateItem()
    {
        Item newItem = new Item(this);
        return newItem;
    }
}

[System.Serializable]
public class Item
{
    public string Name;
    public int ID;
    public string Description;
    public int HealAmount = 0;

    public Item()
    {
        Name = "";
        ID = -1;
    }

    public Item(ItemObject item)
    {
        Name = item.itemName;
        ID = item.ID;
        Description = item.description;

        if(item.type == ItemType.Healing)
        {
            HealingObject healingItem = (HealingObject)item;
            HealAmount = healingItem.restoreHealth;
        }
    }
}

