using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MerchantDialogueSorter : SelectDialogue
{
    private TalkToNPC npc;
    public bool canGetReward = false;

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
                npc.select++;
                break;

            case 1:
                //ontriggerstay
                break;

            case 2:
                if (dialogueManager.questAccepted == true)
                    dialogueManager.questAccepted = false;
                break;

            case 3: case 4: case 5:
                if (QuestLog.instance.quest2C && QuestLog.instance.quest3C && QuestLog.instance.quest4C)
                    npc.select = 6;
                else
                    npc.select = 2;
                break;

            case 6:
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

            if (npc.select == 1 && QuestLog.instance.quest0C)
                npc.select = 2;

            if (!QuestLog.instance.FQ2 && npc.select == 2 && QuestLog.instance.quest2C)
            {
                canGetReward = true;
                npc.select = 3;
            }
            if (!QuestLog.instance.FQ3 && npc.select == 2 && QuestLog.instance.quest3C)
            {
                canGetReward = true;
                npc.select = 4;
            }
            if (!QuestLog.instance.FQ4 && npc.select == 2 && QuestLog.instance.quest4C)
            {
                canGetReward = true;
                npc.select = 5;
            }

            if (dialogueManager.dialogueIsPlaying == true && npc.select == 3 && canGetReward == true)
            {
                foreach (SaveQuest quest in QuestLog.instance.questList.Keys)
                {
                    if (quest.ID == 2)
                    {
                        QuestLog.instance.QuestCompleted(quest);
                        break;
                    }
                }
                Player.instance.AddGold(2000);
                Player.instance.AddExp(200);
                canGetReward = false;
                QuestLog.instance.FQ2 = true;
            }

            if (dialogueManager.dialogueIsPlaying == true && npc.select == 4 && canGetReward == true)
            {
                foreach (SaveQuest quest in QuestLog.instance.questList.Keys)
                {
                    if (quest.ID == 3)
                    {
                        QuestLog.instance.QuestCompleted(quest);
                        break;
                    }
                }
                Player.instance.AddGold(1000);
                Player.instance.AddExp(200);
                canGetReward = false;
                QuestLog.instance.FQ3 = true;
            }

            if (dialogueManager.dialogueIsPlaying == true && npc.select == 5 && canGetReward == true)
            {
                foreach (SaveQuest quest in QuestLog.instance.questList.Keys)
                {
                    if (quest.ID == 4)
                    {
                        QuestLog.instance.QuestCompleted(quest);
                        break;
                    }
                }
                Player.instance.AddGold(1500);
                Player.instance.AddExp(200);
                canGetReward = false;
                QuestLog.instance.FQ4 = true;
            }
        }

    }
}
