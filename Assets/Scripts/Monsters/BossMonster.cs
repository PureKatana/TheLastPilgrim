using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossMonster : EnemyAI
{
    [SerializeField] private Transform fireball;
    [SerializeField] private Transform muzzle;
    [SerializeField] private float cooldownTime;
    [SerializeField] private Image bossHealthBar;
    [SerializeField] private GameObject bossUI;
    [SerializeField] private GameObject bossDefeat;
    [SerializeField] private AudioClip bossDead;
    [SerializeField] private AudioSource music;
    [SerializeField] private InventoryObject inventory;
    [SerializeField] private ItemObject supplies;
    private AudioSource bossAudio;

    private float maxHealth;
    private bool canUse = true;
    private bool usingAbility = false;
    private Animator animator;
    private GameManager manager;

    // Start is called before the first frame update
    void Start()
    {
        manager = GameManager.instance;
        animator = GetComponent<Animator>();
        bossAudio = GetComponent<AudioSource>();

        this.Health = Health + (100 * Player.instance.Level);
        this.Damage = Damage + (15 * Player.instance.Level);

        maxHealth = base.Health;

    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        if (!isDead)
        {
            if (playerInSightRange && !playerInAttackRange && !playerIsDead && !attacking
                || isBeingAttacked && !playerInAttackRange && !playerIsDead && !attacking)
            {
                if (!usingAbility)
                {
                    ChasePlayer();
                    StartCoroutine(UseAbility());
                }
            }
        }

        if(isDead)
        {
            Stop(transform.position);
        }

        bossHealthBar.fillAmount = base.Health / maxHealth;

    }

    private IEnumerator UseAbility()
    {
        if (canUse && !isDead)
        {
            usingAbility = true;
            yield return new WaitForSeconds(1.1f);
            Stop(GameObject.Find("PlayerCharacter").transform.position);
            animator.SetTrigger("Shooting");
            yield return new WaitForSeconds(0.8f);
            ShootFireball();
            yield return new WaitForSeconds(0.2f);
            usingAbility = false;
            StartCoroutine(Cooldown());
        }
    }

    private void ShootFireball()
    {
        Vector3 pos = GameObject.Find("FireballSpawn").transform.position;
        Vector3 end = GameObject.Find("PlayerCharacter").transform.position;
        Vector3 dir = (end - pos).normalized + new Vector3(0, 0.1f, 0);
        Instantiate(fireball, pos, Quaternion.LookRotation(dir, Vector3.up));
        Instantiate(muzzle, pos, Quaternion.LookRotation(dir, Vector3.up));

        //Play sound
        manager.PlaySoundEffect(GameObject.Find("FireballSpawn").GetComponent<AudioSource>(),manager.Fireball, 0.5f, 0.9f);
    }

    private IEnumerator Cooldown()
    {
        canUse = false;
        yield return new WaitForSeconds(cooldownTime);
        canUse = true;
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
    }

    public override void MonsterDeath()
    {
        bossUI.SetActive(false);
        GameObject.Find("BossWall").GetComponent<BoxCollider>().enabled = false;
        bossDefeat.SetActive(true);
        LeanTween.cancel(bossDefeat);
        bossDefeat.transform.localScale = Vector3.one;
        LeanTween.scale(bossDefeat, Vector3.one * 2, 3).setEaseOutExpo();
        StartCoroutine(ChangeMusic());

        inventory.AddItem(supplies.CreateItem(), 1);
        Player.instance.GetComponentInChildren<GainItem>().NewItem(supplies.itemName);

        base.MonsterDeath();
    }

    private IEnumerator ChangeMusic()
    {
        music.volume = 0f;
        music.Stop();
        manager.PlaySoundEffect(bossAudio,bossDead, 0.3f, 0.9f);
        yield return new WaitForSeconds(4f);
        bossDefeat.SetActive(false);
        music.volume = 0.5f;
        music.PlayOneShot(manager.mainMusic);
        
    }

    public override void Stop(Vector3 value)
    {
        base.agent.SetDestination(transform.position);
        transform.LookAt(value);
    }
}
