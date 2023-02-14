using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerUI_Manager : MonoBehaviour
{
    private Player player;
    private AudioSource uiAudio;
    [SerializeField] private AudioClip levelUp;
    [SerializeField] private TextMeshProUGUI ammoText;
    [SerializeField] private Image expBar;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI expNotifier;
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private TextMeshProUGUI goldNotifier;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private GameObject[] ability;
    [SerializeField] private TextMeshProUGUI[] cooldownText;
    [SerializeField] private GameObject[] lockImage;
    [SerializeField] private Image healthBar;
    private AbilityHolder abilityHolder;
    public bool gotExp = false;
    public bool gotGold = false;
    public bool lostGold = false;
    public bool lostExp = false;

    public AudioSource UiAudio { get => uiAudio; set => uiAudio = value; }
    public AudioClip LevelUp { get => levelUp; set => levelUp = value; }


    // Start is called before the first frame update
    void Start()
    {
        player = Player.instance;
        uiAudio = GetComponent<AudioSource>();
        GameObject.Find("GameManager").GetComponent<GameManager>().BlackScreenFade(1, 0, 2, false);
        abilityHolder = GameObject.Find("Player").GetComponentInChildren<AbilityHolder>();
        for (int i = 0; i < cooldownText.Length; i++)
        {
            cooldownText[i].enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //-----------------Health Text-----------------
        healthText.text = player.CurrentHealth.ToString() + " / " + player.MaxHealth.ToString();
        healthBar.fillAmount = player.CurrentHealth / player.MaxHealth;

        //-----------------Ammo Text-----------------
        ammoText.text = player.CurrentAmmo.ToString() + " / " + player.MaxAmmo.ToString();

        //-----------------Exp Text-----------------
        expBar.fillAmount = player.CurrentExp / player.RequiredExp;
        levelText.text = player.Level.ToString();
        if (gotExp)
        {
            Notifier(expNotifier, player.ExpGained, " Exp", "+ ");
            gotExp = false;
        }
        else if (lostExp)
        {
            Notifier(expNotifier, player.ExpLost, "", "-");
            lostExp = false;
        }

        //-----------------Gold Text-----------------
        goldText.text = player.Gold.ToString();
        if (gotGold)
        {
            Notifier(goldNotifier, player.GoldGained, " ", "+");
            gotGold = false;
        }
        else if (lostGold)
        {
            Notifier(goldNotifier, player.GoldLost, "", "-");
            lostGold = false;
        }

        //-----------------Ability Text-----------------

        //Ability 1
        if(player.Level >= abilityHolder.ability[0].levelRequirement)
        {
            lockImage[0].SetActive(false);
            SetAbilityUI(0);
        }


        //Ability 2
        if (player.Level >= abilityHolder.ability[1].levelRequirement)
        {
            lockImage[1].SetActive(false);
            SetAbilityUI(1);
        }

        //Ability 3
        if (player.Level >= abilityHolder.ability[2].levelRequirement)
        {
            lockImage[2].SetActive(false);
            SetAbilityUI(2);
        }

        //Ability4
        if (player.Level >= abilityHolder.ability[3].levelRequirement)
        {
            lockImage[3].SetActive(false);
            SetAbilityUI(3);
        }

    }

    private void SetAbilityUI(int index)
    {
        if (abilityHolder.ability[index].CanUse)
        {
            ability[index].SetActive(true);
            cooldownText[index].enabled = false;
        }
        else
        {
            ability[index].SetActive(false);
            cooldownText[index].enabled = true;
            cooldownText[index].text = abilityHolder.ability[index].cooldownTimeUI.ToString();
        }
    }

    private IEnumerator TurnOffNotifier(TextMeshProUGUI text)
    {
        yield return new WaitForSeconds(0.5f);
        text.enabled = false;
    }

    private void Notifier(TextMeshProUGUI text, float valueGained, string message, string operation)
    {
        text.enabled = true;
        text.text = operation + valueGained.ToString() + message;
        StartCoroutine(TurnOffNotifier(text));
    }

}
