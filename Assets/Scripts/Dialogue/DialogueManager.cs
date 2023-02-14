using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;
using StarterAssets;
using UnityEngine.EventSystems;
using System;

public class DialogueManager : MonoBehaviour
{
    private static DialogueManager instance = null;
    private ThirdPersonController controller;
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;
    private TalkToNPC interaction;

    private Story currentStory;
    public bool dialogueIsPlaying;
    public bool dialogueEnded;

    [SerializeField] public Quest[] quests;
    private QuestLog questLog;
    public int questIndex;
    public bool questAccepted;
    public bool wait;

    [SerializeField] private GameObject[] choices;
    private TextMeshProUGUI[] choicesText;

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

    public static DialogueManager GetInstance()
    {
        return instance;
    }

    private void Start()
    {
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        questLog = QuestLog.instance;
        interaction = GameObject.FindObjectOfType<TalkToNPC>();
        questAccepted = false;
        controller = Player.instance.GetComponentInChildren<ThirdPersonController>();

        choicesText = new TextMeshProUGUI[choices.Length];
        int index = 0;
        foreach(GameObject choice in choices)
        {
            choicesText[index] = choice.GetComponentInChildren<TextMeshProUGUI>();
            index++;
        }
    }

    public void EnterDialogueMode(TextAsset inkJSON)
    {
        GameObject.Find("Player").GetComponentInChildren<StarterAssetsInputs>().dialogue = false; 
        //so player doesnt enter with dialogue being true, which would skip the first line
        currentStory = new Story(inkJSON.text);
        dialogueIsPlaying = true;
        dialoguePanel.SetActive(true);
        StartCoroutine(StopPlayer(0f, 0.2f, false));
        ContinueStory();
    }

    private void ExitDialogueMode()
    {
        StartCoroutine(StopPlayer(2f, 0f, true));
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        interaction.Deactivate();
        dialogueText.text = "";
        dialogueEnded = true;
    }

    private void ContinueStory()
    {
        if (currentStory.canContinue)
        {
            if (!questAccepted && wait)
            {
                currentStory.ObserveVariable("selectQuest", (string varName, object newValue) => {
                    AcceptQuest((int)newValue);
                });
                wait = false;
            }

            dialogueText.text = currentStory.Continue();
            DisplayChoices();
        }
        else
            ExitDialogueMode();

    }

    private void AcceptQuest(int quest)
    {
        if (quest >= 0)
        {
            Debug.Log("Accepting quest " + quest);
            questLog.CheckAcceptQuest(quests[quest].CreateSaveQuest());
            questAccepted = true;
            wait = false;
        }
    }

    private void DisplayChoices()
    {
        List<Choice> currentChoices = currentStory.currentChoices;
        int index = 0;

        //enable active choices
        foreach(Choice choice in currentChoices)
        {
            choices[index].gameObject.SetActive(true);
            choicesText[index].text = choice.text;
            index++;
        }

        //disable inactive choices
        for(int i=index; i < choices.Length; i++)
            choices[i].gameObject.SetActive(false);

    }

    public void MakeChoice(int choiceIndex)
    {
        currentStory.ChooseChoiceIndex(choiceIndex);

        if(choiceIndex == 1 && wait == false)
            wait = true;

        ContinueStory();
    }

    private IEnumerator StopPlayer(float speed, float time, bool enable)
    {
        controller.MoveSpeed = speed;
        yield return new WaitForSeconds(time);
        controller.enabled = enable;
    }

    private void Update()
    {
        if (!dialogueIsPlaying)
            return;

        if (currentStory.currentChoices.Count == 0 && Player.instance.GetComponentInChildren<StarterAssetsInputs>().dialogue)
        {
            ContinueStory();
            GameObject.Find("Player").GetComponentInChildren<StarterAssetsInputs>().dialogue = false;
        }
    }
}
