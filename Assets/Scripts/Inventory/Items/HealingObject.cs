using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Healing Object", menuName = "Inventory System/Items/Healing")]

public class HealingObject : ItemObject
{
    public int restoreHealth;

    public void Awake()
    {
        type = ItemType.Healing;

    }
}
