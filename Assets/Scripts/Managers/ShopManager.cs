using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ShopManager : MonoBehaviour
{
    public int[,] ShopUpgrades = new int[5, 5];
    private Player player;

    [SerializeField] private GameObject NotEnoughGold;

    void Start()
    {
        NotEnoughGold.SetActive(false);
        player = Player.instance;

        //Upgrade ID
        ShopUpgrades[1, 1] = 1;
        ShopUpgrades[1, 2] = 2;
        ShopUpgrades[1, 3] = 3;
        ShopUpgrades[1, 4] = 4;

        /*
        if (PlayerPrefs.GetInt("firstTime") == 0)
        {
            //Upgrade price
            ShopUpgrades[2, 1] = 100;
            ShopUpgrades[2, 2] = 100;
            ShopUpgrades[2, 3] = 100;
            ShopUpgrades[2, 4] = 100;

            //Upgrade Lvl
            ShopUpgrades[3, 1] = 1;
            ShopUpgrades[3, 2] = 1;
            ShopUpgrades[3, 3] = 1;
            ShopUpgrades[3, 4] = 1;
        }
        */
    }

    public void Upgrade()
    {
        GameObject BtnRef = GameObject.FindGameObjectWithTag("Upgrade").GetComponent<EventSystem>().currentSelectedGameObject;

        if (player.Gold >= ShopUpgrades[2, BtnRef.GetComponent<ButtonInfo>().UpgradeID])
        {
            player.RemoveGold(ShopUpgrades[2, BtnRef.GetComponent<ButtonInfo>().UpgradeID]);
            ShopUpgrades[3, BtnRef.GetComponent<ButtonInfo>().UpgradeID]++;
            BtnRef.GetComponent<ButtonInfo>().UpgradelvlTxt.text = ShopUpgrades[3, BtnRef.GetComponent<ButtonInfo>().UpgradeID].ToString();
            ShopUpgrades[2, BtnRef.GetComponent<ButtonInfo>().UpgradeID] += 50 * ShopUpgrades[3, BtnRef.GetComponent<ButtonInfo>().UpgradeID];
            BtnRef.GetComponent<ButtonInfo>().CostTxt.text = ShopUpgrades[2, BtnRef.GetComponent<ButtonInfo>().UpgradeID].ToString();

            if(BtnRef.GetComponent<ButtonInfo>().UpgradeID == 1)
            {
                player.Damage += 5;
            }
            else if(BtnRef.GetComponent<ButtonInfo>().UpgradeID == 2)
            {
                player.MaxAmmo += 12;
                player.TrueMaxAmmo += 12;
            }
            else if(BtnRef.GetComponent<ButtonInfo>().UpgradeID == 3)
            {
                player.MaxCurrentAmmo += 1;
                player.CurrentAmmo += 1;
            }
            else if(BtnRef.GetComponent<ButtonInfo>().UpgradeID == 4)
            {
                player.AbilityDamage += 25;
            }
        }
        else
        {
            NotEnoughGold.SetActive(true);
            StartCoroutine(SetText());
        }
    }

    private IEnumerator SetText()
    {
        yield return new WaitForSeconds(2);
        NotEnoughGold.SetActive(false);
    }

}
