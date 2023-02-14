using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemInteract : MonoBehaviour
{
    public InventoryObject inventory;
    private InventorySlot itemInteracted;
    [SerializeField] private TextMeshProUGUI itemDescription;

    Ray ray;
    RaycastHit hit;

    private void CheckItemInteracted()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out hit);
        if (!hit.collider.gameObject.CompareTag("ItemMenu"))
        {
            Destroy(gameObject);
        }
    }

    public void InteractWithItem(InventorySlot slot)
    {
        itemInteracted = slot;
        itemDescription.text = slot.item.Description;
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))
        {
            CheckItemInteracted();
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Destroy(gameObject);
        }
    }

    public void UseItem()
    {
        if(itemInteracted.item.HealAmount > 0)
        {
            Player.instance.Heal(itemInteracted.item.HealAmount);
            inventory.RemoveItem(itemInteracted.item);
            Destroy(gameObject);
        }
        else Destroy(gameObject);
    }

    public void RemoveItem()
    {
        inventory.RemoveItem(itemInteracted.item);
        Destroy(gameObject);
    }
}
