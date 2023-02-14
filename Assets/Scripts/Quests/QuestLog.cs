using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Runtime.Serialization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class QuestLog : MonoBehaviour
{
    public static QuestLog instance = null;
    [SerializeField] private GameObject questPrefab;
    [SerializeField] private Transform questArea;
    [SerializeField] private TextMeshProUGUI questDescription;
    public InventoryObject inventory;
    private SaveQuest select;

    public bool quest0C = false;
    public bool quest1C = false;
    public bool quest2C = false;
    public bool quest3C = false;
    public bool quest4C = false;
    public bool FQ2, FQ3, FQ4;

    private const string savePath = "\\QuestSaveFile.bin";
    private List<SaveQuest> savedQuests = new List<SaveQuest>();

    public Dictionary<SaveQuest, GameObject> questList = new Dictionary<SaveQuest, GameObject>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
        }
    }

    public void CheckAcceptQuest(SaveQuest quest)
    {
        foreach(SaveQuest checkQuest in questList.Keys)
        {
            if (quest.Name == checkQuest.Name)
            {
                Player.instance.GetComponentInChildren<GainItem>().CantAcceptQuest();
                return;
            }
        }
        if((quest.ID == 2 && FQ2) || (quest.ID == 3 && FQ3) || (quest.ID == 4 && FQ4))
        {
            Player.instance.GetComponentInChildren<GainItem>().CantAcceptQuest();
            return;
        }
        AcceptQuest(quest);
        Player.instance.GetComponentInChildren<GainItem>().AcceptedQuest(quest);
    }

    public void AcceptQuest(SaveQuest quest)
    {
        GameObject gameObject = Instantiate(questPrefab, Vector3.zero, Quaternion.identity, questArea.transform);
        gameObject.GetComponent<QuestButton>().myQuest = quest;
        gameObject.GetComponent<TextMeshProUGUI>().text = quest.Name;
        questList.Add(quest, gameObject);

        foreach(CollectObjective objective in quest.CollectObjectives)
        {
            for (int i = 0; i < inventory.Container.Items.Length; i++)
            {
                if (inventory.Container.Items[i].item.ID == objective.questItem.ID)
                {
                    GameManager.instance.OnItemObtained(inventory.Container.Items[i].item);

                    if (inventory.Container.Items[i].amount > objective.amount)
                        objective.currentAmount = objective.amount;
                    else
                        objective.currentAmount = inventory.Container.Items[i].amount;

                    UpdateSelected();
                    CheckCompletion();
                    break;
                }
            }

            GameManager.instance.itemObtainEvent += new ItemObtained(objective.UpdateItemCount);
        }

        foreach (KillObjective objective in quest.KillObjectives)
        {
            GameManager.instance.killConfirmedEvent += new KillConfirmed(objective.UpdateKillCount);
        }
    }
    

    public void ShowDesc(SaveQuest quest)
    {
        if(quest != null)
        {
            foreach(KeyValuePair<SaveQuest, GameObject> pair in questList)
            {
                if (pair.Key != quest)
                    pair.Value.GetComponent<QuestButton>().DeSelect();
            }

            string objective = string.Empty;

            foreach (Objective obj in quest.KillObjectives)
                objective += obj.type + ": " + obj.currentAmount + "/" + obj.amount + "\n";

            foreach(Objective obj in quest.CollectObjectives)
                objective += obj.type + ": " + obj.currentAmount + "/" + obj.amount + "\n";

            select = quest;
            questDescription.text = string.Format("{0}\n{1}\n\n</size>\nObjective(s):\n\n</size>{2}", quest.Name, quest.Desc, objective);
        }
    }

    public void UpdateSelected()
    {
        ShowDesc(select);

        foreach(KeyValuePair<SaveQuest, GameObject> pair in questList)
            pair.Value.GetComponent<QuestButton>().IsComplete();
    }

    public bool CheckCompletion()
    {
        foreach(SaveQuest quest in questList.Keys)
            return quest.isComplete;

        return false;
    }

    public void QuestCompleted(SaveQuest quest)
    {
        foreach(CollectObjective objective in quest.CollectObjectives)
        {
            for (int i = 0; i < objective.amount; i++)
            {
                foreach (InventorySlot item in inventory.Container.Items)
                {
                    if (item.item.ID == objective.questItem.ID)
                        inventory.RemoveItem(objective.questItem);
                }
            }
        }

        bool hasValue = questList.TryGetValue(quest, out GameObject value);
        if (hasValue)
            Destroy(value);

        questDescription.text = "";
        questList.Remove(quest);
    }

    public void Save()
    {
        foreach (SaveQuest quest in questList.Keys)
        {
            savedQuests.Add(quest);
        }

        BinaryFormatter formatter = new BinaryFormatter();
        Stream stream = new FileStream(string.Concat(Application.persistentDataPath, savePath), FileMode.Create, FileAccess.Write);
        formatter.Serialize(stream, savedQuests);
        stream.Close();

    }

    public void Load()
    {
        if (File.Exists(string.Concat(Application.persistentDataPath, savePath)))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(string.Concat(Application.persistentDataPath, savePath), FileMode.Open, FileAccess.Read);
            List<SaveQuest> loadData = (List<SaveQuest>)formatter.Deserialize(stream);

            foreach (SaveQuest savedQuest in loadData)
                AcceptQuest(savedQuest);

            savedQuests = new List<SaveQuest>();
            stream.Close();
        }
    }

    public void DeleteAll()
    {
        foreach(SaveQuest quest in questList.Keys)
            quest.ClearQuest();

        questList.Clear();
        Save();
    }
}

