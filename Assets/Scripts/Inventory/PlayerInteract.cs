using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] private InventoryObject inventory;

    private void OnTriggerEnter(Collider other)
    {
        var item = other.GetComponentInChildren<ItemDrop>();
        if (item)
        {
            switch (item.randomItem)
            {
                case 0: case 1: case 2:
                    break;

                case 3: case 4: case 5:
                    inventory.AddItem(new Item(item.item[0]), 1);
                    break;

                case 6: case 7:
                    inventory.AddItem(new Item(item.item[1]), 1);
                    break;

                case 8: case 9:
                    inventory.AddItem(new Item(item.item[2]), 1);
                    break;

                case 10: case 11:
                    inventory.AddItem(new Item(item.item[3]), 1);
                    break;

                case 12:
                    inventory.AddItem(new Item(item.item[4]), 1);
                    break;

                case 13:
                    inventory.AddItem(new Item(item.item[5]), 1);
                    break;

                default:
                    break;
            }
        }
    }

    private void OnApplicationQuit()
    {
        inventory.Container.Items = new InventorySlot[15];
    }
}
