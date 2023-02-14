using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PriestessDialogueSorter : SelectDialogue
{
    private TalkToNPC npc;
    public ItemObject medicine;
    public bool canGetReward;
    [SerializeField] private InventoryObject inventory;

    private void Start()
    {
        npc = gameObject.GetComponent<TalkToNPC>();
        base.dialogueManager = DialogueManager.GetInstance();
    }

    public override void GetNextDialogue()
    {
        switch (npc.select)
        {
            case 0:
                if (dialogueManager.questAccepted == true)
                {
                    npc.select = 2;
                    canGetReward = true;
                    dialogueManager.questAccepted = false;
                }
                else
                    npc.select = 1;
                break;

            case 1:
                if (dialogueManager.questAccepted == true)
                {
                    npc.select = 2;
                    canGetReward = true;
                    dialogueManager.questAccepted = false;
                }
                break;

            case 2: case 3: case 4:
                //in ontriggerstay
                break;

            case 5:
                if (dialogueManager.questAccepted == true)
                {
                    npc.select = 7;
                    canGetReward = true;
                    dialogueManager.questAccepted = false;
                }
                else
                    npc.select = 6;
                break;

            case 6:
                if (dialogueManager.questAccepted == true)
                {
                    npc.select = 7;
                    canGetReward = true;
                    dialogueManager.questAccepted = false;
                }
                break;
        }
    }

    public override void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (dialogueManager.dialogueEnded)
            {
                GetNextDialogue();
                dialogueManager.dialogueEnded = false;
            }

            if (npc.select == 2 && QuestLog.instance.quest0C == true)
                npc.select = 3;

            if (npc.select == 4 && QuestLog.instance.quest2C && QuestLog.instance.quest3C && QuestLog.instance.quest4C)
                npc.select = 5;

            if (npc.select == 7 && QuestLog.instance.quest1C == true)
                npc.select = 8;
        }

        if (other.CompareTag("Player") && canGetReward == true)
        {
            if (dialogueManager.dialogueIsPlaying == true && npc.select == 3)
            {
                foreach (SaveQuest quest in QuestLog.instance.questList.Keys)
                {
                    if (quest.ID == 0)
                    {
                        QuestLog.instance.QuestCompleted(quest);
                        break;
                    }
                }
                inventory.AddItem(medicine.CreateItem(), 10);
                Player.instance.GetComponentInChildren<GainItem>().NewItem(medicine.itemName);
                Player.instance.AddGold(2000);
                Player.instance.AddExp(300);

                npc.select = 4;
                canGetReward = false;
            }

            if (dialogueManager.dialogueIsPlaying == true && npc.select == 8)
            {
                foreach (SaveQuest quest in QuestLog.instance.questList.Keys)
                {
                    if (quest.ID == 1)
                    {
                        QuestLog.instance.QuestCompleted(quest);
                        break;
                    }
                }
                Player.instance.AddGold(3000);
                Player.instance.AddExp(400);

                npc.select = 9;
                canGetReward = false;
            }
        }
    }
}
