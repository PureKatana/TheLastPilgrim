using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour {

    private AsyncOperation async;
    private RectTransform rect;
    private Image image;

    //For progress
    [SerializeField] private float speed = 200f;
    public TextMeshProUGUI progressText;

    //For Tween
    [SerializeField] private GameObject logo;
    [SerializeField] private float tweenTime;

    //For setting active
    [SerializeField] private GameObject obj;

    //For loading the scene by index
    [SerializeField] private int loadSceneByindex = -1;
    private int nextScene;
    // Use this for initialization
    void Start () 
    {
        rect = GetComponent<RectTransform>();
        image = rect.GetComponent<Image>();
        image.fillAmount = 0.0f;
        nextScene = PlayerPrefs.GetInt("NextScene");
    }
	
	// Update is called once per frame
	void Update () 
    {
        int progress = 0;
        if (image.fillAmount != 1f)
        {
            image.fillAmount = image.fillAmount + Time.deltaTime * speed;
            progress = (int)(image.fillAmount * 100);
            
            progressText.text = progress + "%";

            LeanTween.cancel(logo);

            LeanTween.scale(logo, logo.transform.localScale * 1.5f, tweenTime).setEasePunch();

        }
        else
        {
            //image.fillAmount = 0.0f;
            progressText.text = "";

            StartCoroutine(Load());

        }
    }

    private IEnumerator Load()
    {
        yield return new WaitForSeconds(3);
        obj.SetActive(false);
        LoadScene();
    }

    private void LoadScene()
    {
        if (loadSceneByindex < 0 || loadSceneByindex > SceneManager.sceneCountInBuildSettings - 1)
        {
            Scene currentScene = SceneManager.GetActiveScene();
            async = SceneManager.LoadSceneAsync(currentScene.buildIndex + 1);
        }
        else
        {
            async = SceneManager.LoadSceneAsync(nextScene);
        }

        async.allowSceneActivation = true;
    }
}
