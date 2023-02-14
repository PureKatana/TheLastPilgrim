using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class OpenTeleport : Interactible
{
    private GameManager manager;
    private ThirdPersonController controller;
    [SerializeField] private GameObject teleportPanel;

    private void Start()
    {
        manager = GameManager.instance;
    }

    private void Awake()
    {
        controller = GameObject.Find("Player").GetComponentInChildren<ThirdPersonController>();
    }

    public override void Activate()
    {
        base.Activate();
        teleportPanel.SetActive(true);
        manager.canPause = false;
        Cursor.lockState = CursorLockMode.None;
        controller.LockCameraPosition = true;
    }

    public override void Deactivate()
    {
        base.Deactivate();
        teleportPanel.SetActive(false);
        manager.canPause = true;
        Cursor.lockState = CursorLockMode.Locked;
        controller.LockCameraPosition = false;
    }

    public void CloseOption()
    {
        teleportPanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }

}
