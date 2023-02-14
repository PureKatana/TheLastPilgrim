using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Quest : ScriptableObject
{
    public int questID;
    public string questName;
    [TextArea(5, 15)]
    public string description;
    public KillObjective[] killObjectives;
    public CollectObjective[] collectObjectives;

    public SaveQuest CreateSaveQuest()
    {
        SaveQuest newSaveQuest = new SaveQuest(this);
        return newSaveQuest;
    }
}

[System.Serializable]
public class SaveQuest
{
    public int ID;
    public string Name;
    [TextArea(5, 15)]
    public string Desc;
    public KillObjective[] KillObjectives;
    public CollectObjective[] CollectObjectives;

    public SaveQuest(Quest quest)
    {
        ID = quest.questID;
        Name = quest.questName;
        Desc = quest.description;
        KillObjectives = quest.killObjectives;
        CollectObjectives = quest.collectObjectives;
    }

    public bool isComplete
    {
        get
        {
            foreach (Objective objective in KillObjectives)
            {
                if (!objective.ObjIsComplete)
                {
                    return false;
                }
            }
            foreach (Objective objective in CollectObjectives)
            {
                if (!objective.ObjIsComplete)
                {
                    return false;
                }
            }

            if (ID == 0)
                QuestLog.instance.quest0C = true;
            if (ID == 1)
                QuestLog.instance.quest1C = true;
            if (ID == 2)
                QuestLog.instance.quest2C = true;
            if (ID == 3)
                QuestLog.instance.quest3C = true;
            if (ID == 4)
                QuestLog.instance.quest4C = true;

            return true;
        }
    }

    public void ClearQuest()
    {
        for (int i = 0; i < KillObjectives.Length; i++)
        {
            KillObjectives[i].currentAmount = 0;
        }

        for (int i = 0; i < CollectObjectives.Length; i++)
        {
            CollectObjectives[i].currentAmount = 0;
        }
    }
}

[System.Serializable]
public abstract class Objective
{
    public int amount;
    public int currentAmount;
    public int ID;
    public string type;
    public Item questItem;

    public bool ObjIsComplete
    {
        get { return currentAmount >= amount; }
    }
}

[System.Serializable]
public class CollectObjective : Objective
{
    public void UpdateItemCount(Item item)
    {
        if(ID == item.ID && currentAmount < amount)
        {
            currentAmount++;
            QuestLog.instance.UpdateSelected();
            QuestLog.instance.CheckCompletion();
        }
    }
}

[System.Serializable]
public class KillObjective : Objective
{
    public void UpdateKillCount(EnemyAI enemy)
    {
        if(type == enemy.Type && currentAmount < amount)
        {
            currentAmount++;
            QuestLog.instance.UpdateSelected();
            QuestLog.instance.CheckCompletion();
        }
    }
}
