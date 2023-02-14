using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class BossCutscene : MonoBehaviour
{
    public GameObject mainCamera;
    public GameObject playerCamera;
    public GameObject cutsceneCamera;
    public GameObject playerCutsceneCamera;
    public GameObject cutscene;
    public GameObject canvasUI;
    public AudioSource music;

    public GameObject bossHealth;
    private BossMonster boss;
    private Animator anim;
    private ThirdPersonController controller;

    private void Start()
    {
        boss = GameObject.Find("BossMonster").GetComponent<BossMonster>();
        anim = GameObject.Find("BossMonster").GetComponent<Animator>();
        controller = GameObject.Find("Player").GetComponentInChildren<ThirdPersonController>();
        music = GameObject.Find("Music").GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            this.enabled = false;
            PlayCutscene();
        }
    }

    private void PlayCutscene()
    {
        for (float i = music.volume; i > 0; i -= 0.1f)
        {
            music.volume -= 0.1f;
        }
        music.Stop();
        SetCutscene(true, 0f);
    }

    public void EndCutscene()
    {
        music.volume = 0.12f;
        music.PlayOneShot(GameObject.Find("GameManager").GetComponent<GameManager>().bossMusic);
        SetCutscene(false,2f);
        
        boss.enabled = true;
        anim.SetTrigger("Awaken");
        bossHealth.SetActive(true);
        GameObject.Find("BossWall").GetComponent<BoxCollider>().enabled = true;
        Destroy(gameObject);
        
    }

    private void SetCutscene(bool value, float speed)
    {
        cutscene.SetActive(value);
        cutsceneCamera.SetActive(value);
        playerCutsceneCamera.SetActive(value);
        mainCamera.SetActive(!value);
        playerCamera.SetActive(!value);
        canvasUI.SetActive(!value);
        GameObject.Find("Player").GetComponentInChildren<ThirdPersonShooterController>().enabled = !value;
        GameObject.Find("Player").GetComponentInChildren<AbilityHolder>().enabled = !value;
        controller.MoveSpeed = speed;
    }

    
}
