using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class AbstractTeleport : MonoBehaviour
{
    private AsyncOperation async;
    [SerializeField] private int sceneIndex;
    public IEnumerator Teleporting()
    {
        PlayerPrefs.SetInt("NextScene", sceneIndex);

        GameManager.instance.BlackScreenFade(0, 1, 2, false);
        yield return new WaitForSeconds(1.9f);
        async = SceneManager.LoadSceneAsync(1);//Go to Loading Screen

        async.allowSceneActivation = true;
    }
}
