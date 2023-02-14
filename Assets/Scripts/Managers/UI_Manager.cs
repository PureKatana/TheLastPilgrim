using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Manager : MonoBehaviour
{
    //For black screen
    [SerializeField] private Image blackScreen;
    

    //For title
    [SerializeField] private GameObject title;
    [SerializeField] private float tweenTime;

    //For menu
    [SerializeField] private GameObject[] buttons;

    // Start is called before the first frame update
    void Awake()
    {
        //Set the elements of the UI
        blackScreen.enabled = true;
        blackScreen.canvasRenderer.SetAlpha(1.0f);
        //title.SetActive(false);
        SetMenu(false);

        //Fade out
        blackScreen.CrossFadeAlpha(0, 3, false);
        StartCoroutine(Faded());

        //Show title
        //StartCoroutine(ShowTitle());
        Tween();

        //Show menu
        StartCoroutine(ShowMenu());

    }

    private void SetMenu(bool value)
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].SetActive(value);
        }
    }   
    
    private IEnumerator Faded()
    {
        yield return new WaitForSeconds(3);
        blackScreen.enabled = false;
    }

    /*
    private IEnumerator ShowTitle()
    {
        yield return new WaitForSeconds(4);
        title.SetActive(true);
        Tween();
    }
    */
    
    private IEnumerator ShowMenu()
    {
        yield return new WaitForSeconds(2);
        SetMenu(true);
    }
    
    public void Tween()
    {
        LeanTween.cancel(title);
        title.transform.localScale = Vector3.one;
        LeanTween.scale(title, Vector3.one * 2, tweenTime).setEaseOutExpo();
    }
}
