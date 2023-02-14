using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using StarterAssets;

public delegate void KillConfirmed(EnemyAI enemy);
public delegate void ItemObtained(Item item);

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    private Player player;
    private ShopManager shopManager;

    //Death system
    private AsyncOperation async;
    private float respawnTime = -1;
    private bool timerDone = false;
    [SerializeField] private Image blackScreen;
    [SerializeField] private GameObject message;
    [SerializeField] private TextMeshProUGUI respawn_Text;
    [SerializeField] private InventoryObject inventory;

    //Sound effects
    [SerializeField] private AudioSource gameAudio;
    [SerializeField] private AudioClip gameOver;
    [SerializeField] private AudioClip fireballhit;
    [SerializeField] private AudioClip fireball;
    public AudioClip bossMusic;
    public AudioClip mainMusic;

    //Pause menu
    private bool gamePaused;
    [SerializeField] GameObject pause_Menu;
    [SerializeField] GameObject quest_Menu;
    [SerializeField] GameObject inventory_Menu;
    public bool canPause = true;

    //Quests
    public event KillConfirmed killConfirmedEvent;
    public event ItemObtained itemObtainEvent;

    //Quests
    [SerializeField] private GameObject questLog;
    public bool questLogActive = false;

    public bool firstTime;
    public bool deleteData;

    //Combat State
    public List<EnemyAI> enemyList = new List<EnemyAI>();
    [SerializeField] private TextMeshProUGUI inCombatText;
    [SerializeField] private GameObject music;
    [SerializeField] private GameObject combatMusic;
    

    public AudioSource GameAudio { get => gameAudio; set => gameAudio = value; }
    public AudioClip FireballHit { get => fireballhit; set => fireballhit = value; }
    public AudioClip Fireball { get => fireball; set => fireball = value; }

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

    // Start is called before the first frame update
    void Start()
    {
        if(deleteData)
           DeleteData(); //enable this if you want to delete the data of the player
        player = Player.instance;
        gameAudio = GetComponent<AudioSource>();
        pause_Menu.SetActive(false);
        quest_Menu.SetActive(false);
        inventory_Menu.SetActive(false);
        //To make sure that you can't shoot in the church
        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            shopManager = GameObject.Find("Shop Manager").GetComponent<ShopManager>();
            if (PlayerPrefs.GetInt("firstTime") == 0)
            {
                //Set the default time of the day
                GameObject.Find("Day/Night Cycle").GetComponent<LightingManager>().TimeOfDay = 6;

                SetPlayerStats();
                SetShop();
                SaveData();
                firstTime = true;
                PlayerPrefs.SetInt("firstTime", firstTime ? 1 : 0);
            }
            LoadData();
            player.Heal(100000);
            Cursor.lockState = CursorLockMode.Locked;
        }
        else if(SceneManager.GetActiveScene().buildIndex == 3)
        {
            LoadData();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!(SceneManager.GetActiveScene().buildIndex == 0))
        {
            //For GameOver
            if (timerDone)
            {
                StartCoroutine(Respawn());
                timerDone = false;
            }

            //Pause Menu
            if ((Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("Cancel")) && canPause)
            {
                if (gamePaused)
                    Resume();
                else
                {
                    PauseGame();
                }
            }

            if (player.GetComponentInChildren<StarterAssetsInputs>().questLog)
            {
                if (!questLogActive)
                {
                    questLog.SetActive(true);
                    questLogActive = true;
                    Cursor.lockState = CursorLockMode.None;
                }
                else
                {
                    Cursor.lockState = CursorLockMode.Locked;
                    questLog.SetActive(false);
                    questLogActive = false;
                }
                player.GetComponent<StarterAssetsInputs>().questLog = false;
            }
                
            if(SceneManager.GetActiveScene().buildIndex == 3)
            {
                if (InCombatState())
                {
                    inCombatText.enabled = true;
                    this.music.SetActive(false);
                    this.combatMusic.SetActive(true);
                }
                else
                {
                    inCombatText.enabled = false;
                    this.music.SetActive(true);
                    this.combatMusic.SetActive(false);
                }
            }
        }
    }

    public bool InCombatState()
    {
        foreach(EnemyAI enemy in enemyList)
        {
            if(enemy.combat)
            {
                return true;
            }
        }
        return false;
    }

    public void PlaySoundEffect(AudioSource source, AudioClip clip, float volume, float pitch)
    {
        source.volume = volume;
        source.pitch = pitch;
        source.PlayOneShot(clip);
    }

    public void GameOver()
    {
        SaveData();
        music.GetComponent<AudioSource>().volume = 0f;
        combatMusic.GetComponent<AudioSource>().volume = 0f;
        canPause = false;
        this.BlackScreenFade(0f, 1f, 2, true);
        PlaySoundEffect(gameAudio, gameOver, 0.5f, 1f);
        //"You died" message appears
        message.SetActive(true);
        LeanTween.cancel(message);
        message.transform.localScale = Vector3.one;
        LeanTween.scale(message, Vector3.one * 2, 3).setEaseOutExpo();

        //Respawn Text appears
        respawnTime = 5f;
        respawn_Text.enabled = true;
        respawn_Text.canvasRenderer.SetAlpha(0f);
        respawn_Text.CrossFadeAlpha(1, 1, false);
        StartCoroutine(RespawnText());
    }

    private void PauseGame()
    {
        this.BlackScreenFade(0f, 0.7f, 1, false);
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0f;
        gamePaused = true;
        pause_Menu.SetActive(true);
    }

    public void Resume()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1f;
        gamePaused = false;
        this.BlackScreenFade(0.7f, 0f, 1, false);
        pause_Menu.SetActive(false);
        quest_Menu.SetActive(false);
        inventory_Menu.SetActive(false);
    }

    public void QuestMenu()
    {
        pause_Menu.SetActive(false);
        quest_Menu.SetActive(true);
    }

    public void ReturnFromQuestMenu()
    {
        pause_Menu.SetActive(true);
        quest_Menu.SetActive(false);
    }

    public void InventoryMenu()
    {
        pause_Menu.SetActive(false);
        inventory_Menu.SetActive(true);
    }

    public void ReturnFromInventoryMenu()
    {
        inventory_Menu.SetActive(false);
        pause_Menu.SetActive(true);
    }

    public void ReturnToMainMenu()
    {
        SaveData();
        PlayerPrefs.SetInt("NextScene", 0);
        this.BlackScreenFade(0.7f, 1f, 2, true);
        Time.timeScale = 1f;
        gamePaused = false;
        async = SceneManager.LoadSceneAsync(1);
        async.allowSceneActivation = true;
    }

    public void BlackScreenFade(float setAlpha, float alpha, int duration, bool value)
    {
        blackScreen.enabled = true;
        blackScreen.canvasRenderer.SetAlpha(setAlpha);
        blackScreen.CrossFadeAlpha(alpha, duration, true);
        StartCoroutine(Faded(duration, value));
    }

    private IEnumerator Faded(float time, bool value)
    {
        yield return new WaitForSeconds(time);
        blackScreen.enabled = value;
    }

    private IEnumerator RespawnText()
    {
        for(float i = respawnTime; i >= 0; i--)
        {
            yield return new WaitForSeconds(1f);
            respawn_Text.text = "Respawning in " + respawnTime + " seconds...";
            respawnTime--;
        }

        timerDone = true;

    }

    private IEnumerator Respawn()
    {
        message.SetActive(false);
        respawn_Text.enabled = false;
        PlayerPrefs.SetInt("NextScene", 2); //Index of the Church scene

        yield return new WaitForSeconds(2f);
        async = SceneManager.LoadSceneAsync(1);//Go to Loading Screen

        async.allowSceneActivation = true;
    }

    public void SaveData()
    {
        //Player stats
        PlayerPrefs.SetFloat("currentHealth", player.CurrentHealth);
        PlayerPrefs.SetFloat("maxHealth", player.MaxHealth);
        PlayerPrefs.SetFloat("currentAmmo", player.CurrentAmmo);
        PlayerPrefs.SetFloat("maxAmmo", player.MaxAmmo);
        PlayerPrefs.SetFloat("maxCurrentAmmo", player.MaxCurrentAmmo);
        PlayerPrefs.SetFloat("trueMaxAmmo", player.TrueMaxAmmo);
        PlayerPrefs.SetFloat("damage", player.Damage);
        PlayerPrefs.SetFloat("level", player.Level);
        PlayerPrefs.SetFloat("currentExp", player.CurrentExp);
        PlayerPrefs.SetInt("requiredExp", player.RequiredExp);
        PlayerPrefs.SetFloat("gold", player.Gold);
        PlayerPrefs.SetFloat("abilityDamage", player.AbilityDamage);

        PlayerPrefs.SetInt("Quest0", QuestLog.instance.quest0C ? 1 : 0);
        PlayerPrefs.SetInt("Quest1", QuestLog.instance.quest1C ? 1 : 0);
        PlayerPrefs.SetInt("Quest2", QuestLog.instance.quest2C ? 1 : 0);
        PlayerPrefs.SetInt("Quest3", QuestLog.instance.quest3C ? 1 : 0);
        PlayerPrefs.SetInt("Quest4", QuestLog.instance.quest4C ? 1 : 0);
        QuestLog.instance.Save();
        inventory.Save();

        //Shop upgrades and dialogue
        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            PlayerPrefs.SetInt("upgradeCost1", shopManager.ShopUpgrades[2, 1]);
            PlayerPrefs.SetInt("upgradeCost2", shopManager.ShopUpgrades[2, 2]);
            PlayerPrefs.SetInt("upgradeCost3", shopManager.ShopUpgrades[2, 3]);
            PlayerPrefs.SetInt("upgradeCost4", shopManager.ShopUpgrades[2, 4]);

            PlayerPrefs.SetInt("upgradeLevel1", shopManager.ShopUpgrades[3, 1]);
            PlayerPrefs.SetInt("upgradeLevel2", shopManager.ShopUpgrades[3, 2]);
            PlayerPrefs.SetInt("upgradeLevel3", shopManager.ShopUpgrades[3, 3]);
            PlayerPrefs.SetInt("upgradeLevel4", shopManager.ShopUpgrades[3, 4]);

            PlayerPrefs.SetInt("PDialogue", GameObject.Find("Priestess").GetComponent<TalkToNPC>().select);
            PlayerPrefs.SetInt("PReward", GameObject.Find("Priestess").GetComponent<PriestessDialogueSorter>().canGetReward ? 1 : 0);
            PlayerPrefs.SetInt("MDialogue", GameObject.Find("Merchant").GetComponent<TalkToNPC>().select);
            PlayerPrefs.SetInt("MReward", GameObject.Find("Merchant").GetComponent<MerchantDialogueSorter>().canGetReward ? 1 : 0);
            PlayerPrefs.SetInt("FQ2", QuestLog.instance.FQ2 ? 1 : 0);
            PlayerPrefs.SetInt("FQ3", QuestLog.instance.FQ3 ? 1 : 0);
            PlayerPrefs.SetInt("FQ4", QuestLog.instance.FQ4 ? 1 : 0);
        }

        //Time of the day
        PlayerPrefs.SetFloat("timeOfDay", GameObject.Find("Day/Night Cycle").GetComponent<LightingManager>().TimeOfDay);

        
        PlayerPrefs.Save();
    }

    
    public void SetPlayerStats()
    {
        player.CurrentHealth = 100;
        player.MaxHealth = player.CurrentHealth;
        player.CurrentAmmo = 12;
        player.MaxAmmo = 120;
        player.MaxCurrentAmmo = player.CurrentAmmo;
        player.TrueMaxAmmo = player.MaxAmmo;
        player.Damage = 25;
        player.Level = 1;
        player.CurrentExp = 0;
        player.RequiredExp = (int)Mathf.Floor(25 * Mathf.Pow(player.Level, 2));
        player.Gold = 1000;
        player.AbilityDamage = 150;


    }

    public void SetShop()
    {
        //Upgrade price
        shopManager.ShopUpgrades[2, 1] = 100;
        shopManager.ShopUpgrades[2, 2] = 100;
        shopManager.ShopUpgrades[2, 3] = 100;
        shopManager.ShopUpgrades[2, 4] = 100;

        //Upgrade Lvl
        shopManager.ShopUpgrades[3, 1] = 1;
        shopManager.ShopUpgrades[3, 2] = 1;
        shopManager.ShopUpgrades[3, 3] = 1;
        shopManager.ShopUpgrades[3, 4] = 1;
    }

    public void LoadData()
    {
        //Player stats
        player.CurrentHealth = PlayerPrefs.GetFloat("currentHealth");
        player.MaxHealth = PlayerPrefs.GetFloat("maxHealth");
        player.CurrentAmmo = PlayerPrefs.GetFloat("currentAmmo");
        player.MaxAmmo = PlayerPrefs.GetFloat("maxAmmo");
        player.MaxCurrentAmmo = PlayerPrefs.GetFloat("maxCurrentAmmo");
        player.TrueMaxAmmo = PlayerPrefs.GetFloat("trueMaxAmmo");
        player.Damage = PlayerPrefs.GetFloat("damage");
        player.Level = PlayerPrefs.GetFloat("level");
        player.CurrentExp = PlayerPrefs.GetFloat("currentExp");
        player.RequiredExp = PlayerPrefs.GetInt("requiredExp");
        player.Gold = PlayerPrefs.GetFloat("gold");
        player.AbilityDamage = PlayerPrefs.GetFloat("abilityDamage");

        QuestLog.instance.quest0C = PlayerPrefs.GetInt("Quest0") == 1 ? true : false;
        QuestLog.instance.quest1C = PlayerPrefs.GetInt("Quest1") == 1 ? true : false;
        QuestLog.instance.quest2C = PlayerPrefs.GetInt("Quest2") == 1 ? true : false;
        QuestLog.instance.quest3C = PlayerPrefs.GetInt("Quest3") == 1 ? true : false;
        QuestLog.instance.quest4C = PlayerPrefs.GetInt("Quest4") == 1 ? true : false;
        QuestLog.instance.Load();
        inventory.Load();

        //Shop upgrades and dialogue
        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            shopManager.ShopUpgrades[2, 1] = PlayerPrefs.GetInt("upgradeCost1");
            shopManager.ShopUpgrades[2, 2] = PlayerPrefs.GetInt("upgradeCost2");
            shopManager.ShopUpgrades[2, 3] = PlayerPrefs.GetInt("upgradeCost3");
            shopManager.ShopUpgrades[2, 4] = PlayerPrefs.GetInt("upgradeCost4");

            shopManager.ShopUpgrades[3, 1] = PlayerPrefs.GetInt("upgradeLevel1");
            shopManager.ShopUpgrades[3, 2] = PlayerPrefs.GetInt("upgradeLevel2");
            shopManager.ShopUpgrades[3, 3] = PlayerPrefs.GetInt("upgradeLevel3");
            shopManager.ShopUpgrades[3, 4] = PlayerPrefs.GetInt("upgradeLevel4");

            GameObject.Find("Priestess").GetComponent<TalkToNPC>().select = PlayerPrefs.GetInt("PDialogue");
            GameObject.Find("Priestess").GetComponent<PriestessDialogueSorter>().canGetReward = PlayerPrefs.GetInt("PReward") == 1 ? true : false;
            GameObject.Find("Merchant").GetComponent<TalkToNPC>().select = PlayerPrefs.GetInt("MDialogue");
            GameObject.Find("Merchant").GetComponent<MerchantDialogueSorter>().canGetReward = PlayerPrefs.GetInt("MReward") == 1 ? true : false;
            QuestLog.instance.FQ2 = PlayerPrefs.GetInt("FQ2") == 1 ? true : false;
            QuestLog.instance.FQ3 = PlayerPrefs.GetInt("FQ3") == 1 ? true : false;
            QuestLog.instance.FQ4 = PlayerPrefs.GetInt("FQ4") == 1 ? true : false;
        }

        //Time of the day
        GameObject.Find("Day/Night Cycle").GetComponent<LightingManager>().TimeOfDay = PlayerPrefs.GetFloat("timeOfDay");

        
    }

    public void DeleteData()
    {
        PlayerPrefs.DeleteAll();
        inventory.Clear();
        QuestLog.instance.DeleteAll();
    }

    public void OnKillConfirmed(EnemyAI enemy)
    {
        if (killConfirmedEvent != null)
            killConfirmedEvent(enemy);
    }

    public void OnItemObtained(Item item)
    {
        if (itemObtainEvent != null)
            itemObtainEvent(item);
    }

}
