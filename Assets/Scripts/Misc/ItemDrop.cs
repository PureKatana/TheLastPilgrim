using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    public ItemObject[] item;
    public string goldDrop;
    public float gold = 0;
    public int randomItem;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            randomItem = Random.Range(0, 14);

            switch (randomItem)
            {
                case 0: case 1: case 2:
                    Player.instance.AddGold(gold);
                    Player.instance.GetComponentInChildren<GainItem>().NewItem(goldDrop);
                    break;

                case 3: case 4: case 5:
                    Player.instance.AddGold(gold);
                    Player.instance.GetComponentInChildren<GainItem>().NewItem(goldDrop);
                    Player.instance.GetComponentInChildren<GainItem>().NewItem(item[0].itemName);
                    GameManager.instance.OnItemObtained(item[0].CreateItem());
                    break;

                case 6: case 7:
                    Player.instance.AddGold(gold);
                    Player.instance.GetComponentInChildren<GainItem>().NewItem(goldDrop);
                    Player.instance.GetComponentInChildren<GainItem>().NewItem(item[1].itemName);
                    GameManager.instance.OnItemObtained(item[1].CreateItem());
                    break;

                case 8: case 9:
                    Player.instance.AddGold(gold);
                    Player.instance.GetComponentInChildren<GainItem>().NewItem(goldDrop);
                    Player.instance.GetComponentInChildren<GainItem>().NewItem(item[2].itemName);
                    GameManager.instance.OnItemObtained(item[2].CreateItem());
                    break;

                case 10: case 11:
                    Player.instance.AddGold(gold);
                    Player.instance.GetComponentInChildren<GainItem>().NewItem(goldDrop);
                    Player.instance.GetComponentInChildren<GainItem>().NewItem(item[3].itemName);
                    GameManager.instance.OnItemObtained(item[3].CreateItem());
                    break;

                case 12:
                    Player.instance.AddGold(gold);
                    Player.instance.GetComponentInChildren<GainItem>().NewItem(goldDrop);
                    Player.instance.GetComponentInChildren<GainItem>().NewItem(item[4].itemName);
                    GameManager.instance.OnItemObtained(item[4].CreateItem());
                    break;

                case 13:
                    Player.instance.AddGold(gold);
                    Player.instance.GetComponentInChildren<GainItem>().NewItem(goldDrop);
                    Player.instance.GetComponentInChildren<GainItem>().NewItem(item[5].itemName);
                    GameManager.instance.OnItemObtained(item[5].CreateItem());
                    break;

                default:
                    Player.instance.AddGold(gold);
                    break;
            }

            Destroy(gameObject);
        }
    }
}
