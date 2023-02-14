using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GainItem : MonoBehaviour
{
    public TextMeshProUGUI text;
    private GameObject textPrefab;

    public void NewItem(string itemGained)
    {
        textPrefab = Instantiate(text.gameObject, transform);
        textPrefab.GetComponent<TextMeshProUGUI>().text = "You gained " + itemGained;
    }

    public void AcceptedQuest(SaveQuest quest)
    {
        textPrefab = Instantiate(text.gameObject, transform);
        textPrefab.GetComponent<TextMeshProUGUI>().text = "New quest: '" + quest.Name + "'";
    }

    public void CantAcceptQuest()
    {
        textPrefab = Instantiate(text.gameObject, transform);
        textPrefab.GetComponent<TextMeshProUGUI>().text = "You've already accepted this quest!";
    }
}
