using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;

public class Menu_Manager : AbstractTeleport
{
    public static Menu_Manager instance = null;
    //For title
    [SerializeField] private GameObject title;
    [SerializeField] private float tweenTime;

    //For menu
    [SerializeField] private GameObject buttons;

    //For Options
    [SerializeField] private GameObject gameManager;
    [SerializeField] private GameObject options;
    public AudioMixer audioMixer;
    public float musicVolume;
    public float sfxVolume;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
        }

        buttons.SetActive(false);
        Tween();
        //Show menu
        StartCoroutine(ShowMenu());
        options.SetActive(false);
    }

    private void Start()
    {
        
        gameManager.GetComponent<GameManager>().canPause = false;
        //Fade out
        gameManager.GetComponent<GameManager>().BlackScreenFade(1f, 0f, 3, false);

        //Options
        musicVolume = PlayerPrefs.GetFloat("MusicParam", 1f);
        sfxVolume = PlayerPrefs.GetFloat("SFXParam", 1f);

        //Set the Audio
        audioMixer.SetFloat("MusicParam", Mathf.Log10(musicVolume) * 30);
        audioMixer.SetFloat("SFXParam", Mathf.Log10(sfxVolume) * 30);
    }

    private IEnumerator ShowMenu()
    {
        yield return new WaitForSeconds(2);
        buttons.SetActive(true);
    }

    public void Tween()
    {
        LeanTween.cancel(title);
        title.transform.localScale = Vector3.one;
        LeanTween.scale(title, Vector3.one * 2, tweenTime).setEaseOutExpo();
    }

    public void StartGame()
    {
        StartCoroutine(Teleporting());
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ShowOptions()
    {
        options.SetActive(true);
        buttons.SetActive(false);
    }

    public void Return()
    {
        options.SetActive(false);
        PlayerPrefs.Save();
        buttons.SetActive(true);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            GameObject.Find("GameManager").GetComponent<GameManager>().DeleteData();
        }
    }

}
