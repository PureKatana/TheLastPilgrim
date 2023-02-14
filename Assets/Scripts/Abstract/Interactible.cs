using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;
using TMPro;

public abstract class Interactible : MonoBehaviour
{
    public bool PlayerInRange = false;

    [SerializeField] private TextMeshProUGUI InteractableText;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            InteractableText.enabled = true;

#if UNITY_PS4
            InteractableText.text = "Press O to interact";
#elif UNITY_XBOXONE
            InteractableText.text = "Press B to interact";
#else
            InteractableText.text = "Press F to interact";
#endif
            PlayerInRange = true;
            GameObject.Find("Player").GetComponentInChildren<StarterAssetsInputs>().interact = false;
            //so player doesnt enter with interact being true from pressing f beforehand, which would auto interact
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            InteractableText.enabled = false;
            PlayerInRange = false;
            Deactivate();
        }
    }

    public virtual void Activate()
    {
        InteractableText.enabled = false;
    }

    public virtual void Deactivate()
    {

    }

    void FixedUpdate()
    {
        if(GameObject.Find("Player").GetComponentInChildren<StarterAssetsInputs>().interact && PlayerInRange)
        {
            Activate();
            GameObject.Find("Player").GetComponentInChildren<StarterAssetsInputs>().interact = false;
        }

    }
}
