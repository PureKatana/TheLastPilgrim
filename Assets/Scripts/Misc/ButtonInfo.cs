using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonInfo : MonoBehaviour
{
    public int UpgradeID;
    
    public TextMeshProUGUI UpgradelvlTxt;
    public TextMeshProUGUI CostTxt;
    public GameObject shopManager;
    

    // Update is called once per frame
    void Update()
    {
        CostTxt.text = "$ " + shopManager.GetComponent<ShopManager>().ShopUpgrades[2, UpgradeID].ToString();
        UpgradelvlTxt.text = "Lvl:  " + shopManager.GetComponent<ShopManager>().ShopUpgrades[3, UpgradeID].ToString();
    }
}
