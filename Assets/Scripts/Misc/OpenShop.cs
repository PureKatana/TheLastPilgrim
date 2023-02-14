using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using StarterAssets;

public class OpenShop : Interactible
{
    private ThirdPersonController controller;

    [SerializeField] private GameObject ShopUI;

    private GameManager manager;

    public override void Activate()
    {
        base.Activate();
        ShopUI.SetActive(true);
        manager.canPause = false;
        Cursor.lockState = CursorLockMode.None;
        controller.LockCameraPosition = true;
    }

    public override void Deactivate()
    {
        base.Deactivate();
        ShopUI.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        controller.LockCameraPosition = false;
        StartCoroutine(CanPauseAgain(0.2f));
    }

    void Awake()
    {
        controller = GameObject.Find("Player").GetComponentInChildren<ThirdPersonController>();
    }
    
    private void Start()
    {
        manager = GameManager.instance;
        manager.canPause = true;
    }

    private void Update()
    {
        
        if(Input.GetKeyDown(KeyCode.Escape) && manager.canPause == false)
        {
            Deactivate();
        }
        
    }

    private IEnumerator CanPauseAgain(float time)
    {
        yield return new WaitForSeconds(time);
        manager.canPause = true;
    }
    
}
