using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class DisplayInventory : MonoBehaviour
{
    //private MouseItem mouseItem;
    public InventoryObject inventory;
    public GameObject inventoryPrefab;
    Dictionary<GameObject, InventorySlot> itemsDisplayed = new Dictionary<GameObject, InventorySlot>();
    public GameObject interactPrefab;
    public GameObject canvas;
    private Vector3 offset = new Vector3(100f, -100f);

    void Start()
    {
        CreateSlots();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateSlots();
    }

    public void CreateSlots()
    {
        itemsDisplayed = new Dictionary<GameObject, InventorySlot>();

        for (int i = 0; i < inventory.Container.Items.Length; i++)
        {
            var obj = Instantiate(inventoryPrefab, Vector3.zero, Quaternion.identity, transform);

            AddEvent(obj, EventTriggerType.PointerEnter, delegate { OnEnter(obj); });
            AddEvent(obj, EventTriggerType.PointerExit, delegate { OnExit(obj); });
            AddEvent(obj, EventTriggerType.BeginDrag, delegate { OnDragStart(obj); });
            AddEvent(obj, EventTriggerType.EndDrag, delegate { OnDragEnd(obj); });
            AddEvent(obj, EventTriggerType.Drag, delegate { OnDrag(obj); });
            AddEvent(obj, EventTriggerType.PointerClick, delegate { PointerClick(obj); });

            itemsDisplayed.Add(obj, inventory.Container.Items[i]);
        }
    }

    public void UpdateSlots()
    {
        foreach (KeyValuePair<GameObject, InventorySlot> slot in itemsDisplayed)
        {
            if (slot.Value.ID >= 0)
            {
                slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite =
                    inventory.database.GetItem[slot.Value.item.ID].iconDisplay;
                slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 1);
                slot.Key.GetComponentInChildren<TextMeshProUGUI>().text = slot.Value.amount == 1 ? "" : slot.Value.amount.ToString("n0");
            }
            else
            {
                slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite = null;
                slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 0);
                slot.Key.GetComponentInChildren<TextMeshProUGUI>().text = "";
            }
        }
    }


    private void AddEvent(GameObject obj, EventTriggerType type, UnityAction<BaseEventData> action)
    {
        EventTrigger trigger = obj.GetComponent<EventTrigger>();
        var eventTrigger = new EventTrigger.Entry();
        eventTrigger.eventID = type;
        eventTrigger.callback.AddListener(action);
        trigger.triggers.Add(eventTrigger);
    }

    public void OnEnter(GameObject obj)
    {
        MouseData.slotHoveredOver = obj;
    }
    private void OnExit(GameObject obj)
    {
        MouseData.slotHoveredOver = null;
    }

    private void OnDragStart(GameObject obj)
    {
        var mouseObject = new GameObject();
        var rt = mouseObject.AddComponent<RectTransform>();
        rt.sizeDelta = new Vector2(77, 77);
        mouseObject.transform.SetParent(transform.parent);
        mouseObject.transform.position = Input.mousePosition;

        if (itemsDisplayed[obj].ID >= 0)
        {
            var image = mouseObject.AddComponent<Image>();
            image.sprite = inventory.database.GetItem[itemsDisplayed[obj].ID].iconDisplay;
            image.raycastTarget = false;
        }

        MouseData.tempItemDragged = mouseObject;
    }

    private void OnDrag(GameObject obj)
    {
        if (MouseData.tempItemDragged != null)
            MouseData.tempItemDragged.GetComponent<RectTransform>().position = Input.mousePosition;
    }

    private void OnDragEnd(GameObject obj)
    {
        Destroy(MouseData.tempItemDragged);

        if (MouseData.slotHoveredOver)
        {
            inventory.SwapItems(itemsDisplayed[obj], itemsDisplayed[MouseData.slotHoveredOver]);
        }
    }

    private void PointerClick(GameObject obj)
    {
        if(itemsDisplayed[obj].ID >= 0)
        {
            var newInteractPrefab = Instantiate(interactPrefab, Input.mousePosition + offset, Quaternion.identity, canvas.transform);
            newInteractPrefab.GetComponent<ItemInteract>().InteractWithItem(itemsDisplayed[obj]);
        }
    }

    private void OnApplicationQuit()
    {
        inventory.Clear();
    }
}

public static class MouseData
{
    public static GameObject tempItemDragged;
    public static GameObject slotHoveredOver;

}

