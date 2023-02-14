using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Ink.Runtime;
using StarterAssets;

public class TalkToNPC : Interactible
{
    private ThirdPersonController controller;
    private GameManager manager;
    private DialogueManager dialogue;
    private Animator anim;

    [SerializeField] public TextAsset[] inkJSON;
    public int select = 0;

    private void Awake()
    {
        controller = GameObject.Find("Player").GetComponentInChildren<ThirdPersonController>();
        anim = GetComponent<Animator>();
    }

    void Start()
    {
        dialogue = DialogueManager.GetInstance();
        manager = GameManager.instance;
        manager.canPause = true;
    }

    public override void Activate()
    {
        base.Activate();
        manager.canPause = false;
        Cursor.lockState = CursorLockMode.None;
        controller.LockCameraPosition = true;
        GameObject.Find("Player").GetComponentInChildren<StarterAssetsInputs>().dialogue = false;
        //to make sure dialogue is false when player enters dialogue
        //transform.LookAt(GameObject.Find("Player").transform.position);
        anim.SetBool("isTalking", true);
        dialogue.EnterDialogueMode(inkJSON[select]);
    }

    public override void Deactivate()
    {
        base.Deactivate();
        manager.canPause = true;
        anim.SetBool("isTalking", false);
        Cursor.lockState = CursorLockMode.Locked;
        controller.LockCameraPosition = false;
    }
}
