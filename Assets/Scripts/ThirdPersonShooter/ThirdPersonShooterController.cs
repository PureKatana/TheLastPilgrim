using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using StarterAssets;
using UnityEngine.InputSystem;
using UnityEngine.Animations.Rigging;
using UnityEngine.SceneManagement;

public class ThirdPersonShooterController : MonoBehaviour
{
    //ThirdPersonShooter variables
    [SerializeField] private CinemachineVirtualCamera aimVirtualCamera;
    [SerializeField] private float baseSensitivity;
    [SerializeField] private float aimSensitivity;
    [SerializeField] private LayerMask aimColliderMask = new LayerMask();
    //[SerializeField] private Transform bullet;
    [SerializeField] private Transform spawnBullet;
    [SerializeField] private Transform target;
    [SerializeField] private GameObject crosshair;

    //Other Components
    private ThirdPersonController thirdPersonController;
    private StarterAssetsInputs starterAssetsInputs;
    private Animator animator;
    private Player player;

    //Rigging Animation
    [SerializeField] private Rig rigAim;
    private float rigAimWeight;

    //Gun variables
    private RaycastFiring firing;
    private EquipGun equipGun;
    private bool reloadAnim = false;

    //Sound effects
    private AudioSource gunAudio;
    [SerializeField] private AudioClip shoot;
    [SerializeField] private AudioClip reload;
    [SerializeField] private AudioClip noAmmo;

    public bool canShoot = true;

    //Ability variable
    private AbilityHolder abilityHolder;

    //Game Manager
    private GameManager manager;

    public CinemachineVirtualCamera AimVirtualCamera { get => aimVirtualCamera; set => aimVirtualCamera = value; }
    public AudioSource GunAudio { get => gunAudio; set => gunAudio = value; }
    public AudioClip Reload1 { get => reload; set => reload = value; }

    private void Awake()
    {
        thirdPersonController = GetComponent<ThirdPersonController>();
        animator = GetComponent<Animator>();
        firing = GetComponentInChildren<RaycastFiring>();
        equipGun = GetComponentInChildren<EquipGun>();
        gunAudio = GetComponent<AudioSource>();
        abilityHolder = GetComponent<AbilityHolder>();
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private void Start()
    {
        player = Player.instance;
        starterAssetsInputs = StarterAssetsInputs.instance;
    }


    private void Update()
    {
        if(SceneManager.GetActiveScene().buildIndex != 2)
        {
            SetThirdPersonShooter();
        }
        
        SetRigAim();
    }

    private void SetRigAim()
    {
        rigAim.weight = Mathf.Lerp(rigAim.weight, rigAimWeight, Time.deltaTime * 20f);
    }


    private void SetThirdPersonShooter()
    {
        //Calculate the mouse Position
        Vector3 worldPosition = Vector3.zero;

        Vector2 centerPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = Camera.main.ScreenPointToRay(centerPoint);
        if (Physics.Raycast(ray, out RaycastHit hit, 999f, aimColliderMask))
        {
            worldPosition = hit.point;
            target.position = hit.point;
        }

        //-----------------------------------------------AIMING----------------------------------------------------
        if (starterAssetsInputs.aim)
        {
            SetAimingBehaviors(true, aimSensitivity);
            //Start aiming animation
            animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 1f, Time.deltaTime * 10f));

            Vector3 aimTarget = worldPosition;
            aimTarget.y = transform.position.y;
            Vector3 aimDirection = (aimTarget - transform.position).normalized;

            //character rotation
            transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 20f);

            rigAimWeight = 1f;
            equipGun.Equip();
        
        }
        else
        {
            SetAimingBehaviors(false, baseSensitivity);
            rigAimWeight = 0f;
            animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 0f, Time.deltaTime * 0.2f));
            //Makes sure the player doesn't aim/doesn't equip the weapon again
            WaitForUnequipAnim();

            //Disable Laser Beam
            abilityHolder.laser.SetActive(false);
            
        }

        //-----------------------------------------------RELOADING----------------------------------------------------
        if (starterAssetsInputs.reload)
        {
            Reload();
            starterAssetsInputs.reload = false;
        }

        //-----------------------------------------------SHOOTING----------------------------------------------------
        if (starterAssetsInputs.shoot)
        {
            if (player.CurrentAmmo > 0 && player.CurrentAmmo <= player.MaxCurrentAmmo)
            {
                if (starterAssetsInputs.aim && !reloadAnim
                    && !abilityHolder.ability[0].PerformingAbility
                    && !abilityHolder.ability[1].PerformingAbility
                    && !abilityHolder.ability[2].PerformingAbility)
                {
                    /*
                    Vector3 aimDir = (worldPosition - spawnBullet.position).normalized;
                    Instantiate(bullet, spawnBullet.position, Quaternion.LookRotation(aimDir, Vector3.up));
                    */
                    animator.SetTrigger("Shoot");
                    manager.PlaySoundEffect(gunAudio, shoot, 0.3f, 0.8f);
                    firing.StartFiring();
                    //player.AddExp(10);
                    //player.AddGold(500);
                    //player.TakeDamage(30);
                    player.CurrentAmmo--;
                }
                //Set it back to false so it doesn't shoot constantly

                firing.StopFiring();
            }
            else
            {
                //Play no ammo sound effect
                manager.PlaySoundEffect(gunAudio, noAmmo, 0.3f, 1f);
            }
            starterAssetsInputs.shoot = false;
        }
        
        
        
    }

    private void Reload()
    {
        if (player.CurrentAmmo < player.MaxCurrentAmmo && player.MaxAmmo > 0) //To make sure that you have ammo to reload
        {
            reloadAnim = true;
            animator.SetTrigger("Reload");
            manager.PlaySoundEffect(gunAudio, reload, 0.3f, 1f);
            if (player.MaxAmmo >= player.MaxCurrentAmmo) //To make sure that you reload the right amount
            {
                float amount = player.MaxCurrentAmmo - player.CurrentAmmo; //Calculate the amount of ammo that will reload
                player.CurrentAmmo += amount; //Add the amount to the current ammo
                player.MaxAmmo -= amount;//Remove the amount to the maxAmmo
            }
            else //Reloads the leftover amount that is lower than maxCurrentAmmo from the maxAmmo
            {
                if (player.CurrentAmmo + player.MaxAmmo >= player.MaxCurrentAmmo) //Checks if you have enough ammo to refill to the maxCurrentAmmo
                {
                    float amount = player.MaxCurrentAmmo - player.CurrentAmmo; 
                    player.CurrentAmmo += amount; 
                    player.MaxAmmo -= amount;
                }
                else //If you don't have enough ammo to refill to the maxCurrentAmmo
                {
                    float amount = player.MaxAmmo; 
                    player.CurrentAmmo += player.MaxAmmo; 
                    player.MaxAmmo -= amount; 
                }
            }
            StartCoroutine(WaitForReloadAnim());
        }
    }


    private IEnumerator WaitForReloadAnim()
    {
        yield return new WaitForSeconds(3.5f);
        reloadAnim = false;
    }

    private void WaitForUnequipAnim()
    {
        if (animator.GetCurrentAnimatorStateInfo(1).IsName("Rifle Put Away"))
        {
            //1 = a complete cycle
            if(animator.GetCurrentAnimatorStateInfo(1).normalizedTime > 0.7f)
            {
                if (!equipGun.isUnequipped)
                    equipGun.Unequip();
            }
            
        }
       
    }
    private void SetAimingBehaviors(bool value, float sensitivity)
    {
        //Set the animation
        animator.SetBool("Aiming", value);
        //Set the virtual aim camera
        aimVirtualCamera.gameObject.SetActive(value);
        //Set the sensitivity
        thirdPersonController.SetSensitivity(sensitivity);
        //Set the rotation when you move
        thirdPersonController.SetRotateOnMove(!value);
        //Make the crosshair visible
        crosshair.SetActive(value);
        //Set the rig aim

    }
}
