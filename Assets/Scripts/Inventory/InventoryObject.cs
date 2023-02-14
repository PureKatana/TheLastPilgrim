using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Runtime.Serialization;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
public class InventoryObject : ScriptableObject
{
    public ItemDatabaseObject database;
    public Inventory Container;
    private const string savePath = "\\InventorySaveFile.json";

    public void AddItem(Item it, int am)
    {
        for(int i = 0; i < Container.Items.Length; i++)
        {
            if (Container.Items[i].ID == it.ID)
            {
                Container.Items[i].AddAmount(am);
                return;
            }
        }
        SetFirstEmptySlot(it, am);
    }

    public InventorySlot SetFirstEmptySlot(Item it, int am)
    {
        for (int i = 0; i < Container.Items.Length; i++)
        {
            if (Container.Items[i].ID <= -1)
            {
                Container.Items[i].UpdateSlot(it.ID, it, am);
                return Container.Items[i];
            }
        }
        return null;
    }

    public void SwapItems(InventorySlot item1, InventorySlot item2)
    {
        InventorySlot temp = new InventorySlot(item2.ID, item2.item, item2.amount);
        item2.UpdateSlot(item1.ID, item1.item, item1.amount);
        item1.UpdateSlot(temp.ID, temp.item, temp.amount);
    }

    public void RemoveItem(Item it)
    {
        for (int i = 0; i < Container.Items.Length; i++)
        {
            if (Container.Items[i].item == it)
            {
                if (Container.Items[i].amount > 1)
                    Container.Items[i].amount--;
                else
                    Container.Items[i].UpdateSlot(-1, null, 0);
            }
        }
    }

    public void Save()
    {
        IFormatter formatter = new BinaryFormatter();
        Stream stream = new FileStream(string.Concat(Application.persistentDataPath, savePath), FileMode.Create, FileAccess.Write);
        formatter.Serialize(stream, Container);
        stream.Close();
    }

    public void Load()
    {
        if(File.Exists(string.Concat(Application.persistentDataPath, savePath)))
        {
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(string.Concat(Application.persistentDataPath, savePath), FileMode.Open, FileAccess.Read);
            Inventory newContainer = (Inventory)formatter.Deserialize(stream);
            for (int i = 0; i < Container.Items.Length; i++)
            {
                Container.Items[i].UpdateSlot(newContainer.Items[i].ID, newContainer.Items[i].item, newContainer.Items[i].amount);
            }
            stream.Close();
        }
    }

    public void Clear()
    {
        Container.Clear();
        Save();
    }
}

[System.Serializable]
public class Inventory
{
    public InventorySlot[] Items = new InventorySlot[15];

    public void Clear()
    {
        for (int i = 0; i < Items.Length; i++)
        {
            Items[i].RemoveItem();
        }
    }
}

[System.Serializable]
public class InventorySlot
{
    public int ID;
    public Item item;
    public int amount;

    public InventorySlot()
    {
        ID = -1;
        item = null;
        amount = 0;
    }

    public InventorySlot(int id, Item it, int am)
    {
        ID = id;
        item = it;
        amount = am;
    }

    public void UpdateSlot(int id, Item it, int am)
    {
        ID = id;
        item = it;
        amount = am;
    }

    public void RemoveItem()
    {
        ID = -1;
        item = new Item();
        amount = 0;
    }

    public void AddAmount(int value)
    {
        amount += value;
    }
}

