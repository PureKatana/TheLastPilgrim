using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;
using UnityEngine.SceneManagement;

public class AbilityHolder : MonoBehaviour
{
    public Ability[] ability;
    public GameObject dashEffect;
    public GameObject laser;
    public GameObject shield;
    public bool dash;

    public AudioClip[] abilitySounds;
    private StarterAssetsInputs inputs;
    private Player player;

    private void Awake()
    {
        inputs = GetComponent<StarterAssetsInputs>();
    }
    private void Start()
    {
        for(int i = 0; i < ability.Length; i++)
        {
            ability[i].CanUse = true;
            ability[i].cooldownTimeUI = ability[i].cooldownTime;
        }
        player = Player.instance;
    }
    // Update is called once per frame
    void Update()
    {
        if(SceneManager.GetActiveScene().buildIndex != 2)
        {
            //Ability 1
            if (inputs.ability1 && player.Level >= ability[0].levelRequirement)
            {
                //Makes sure that you can only use this while aiming and also not while using the laser beam
                if(inputs.aim && !ability[2].PerformingAbility)
                   ActivateAbility(0);
                inputs.ability1 = false;
            }

            //Ability2
            if(inputs.ability2 && player.Level >= ability[1].levelRequirement)
            {
                ActivateAbility(1);

                inputs.ability2 = false;
            }

            //Ability3
            if (inputs.ability3 && player.Level >= ability[2].levelRequirement)
            {
                if (inputs.aim)
                    ActivateAbility(2);

                inputs.ability3 = false;
            }
            //Ability4
            if (inputs.ability4 && player.Level >= ability[3].levelRequirement)
            {
                ActivateAbility(3);

                inputs.ability4 = false;
            }
        }
        
    }

    private void ActivateAbility(int index)
    { 
        if (ability[index].CanUse)
        {
            //Activate
            ability[index].TriggerAbility();
            StartCoroutine(ability[index].WaitForActiveTime());
            StartCoroutine(ability[index].StartCooldown());

            //Only for dash ability
            if (dash)
                StartCoroutine(DashEffect());
        }
    }

    private IEnumerator DashEffect()
    {
        dashEffect.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        dashEffect.SetActive(false);
        dash = false;
    }

    
}
