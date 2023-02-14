using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestButton : MonoBehaviour
{
    public SaveQuest myQuest;

    public void SelectDesc()
    {
        GetComponent<TextMeshProUGUI>().color = new Color(0.82f, 0.73f, 0.73f);
        QuestLog.instance.ShowDesc(myQuest);
    }

    public void DeSelect()
    {
        GetComponent<TextMeshProUGUI>().color = Color.white;
    }

    public void IsComplete()
    {
        if(myQuest.isComplete)
        {
            GetComponent<TextMeshProUGUI>().text = myQuest.Name + " [!]";
        }
        else if(!myQuest.isComplete)
        {
            GetComponent<TextMeshProUGUI>().text = myQuest.Name;
        }
    }
}
