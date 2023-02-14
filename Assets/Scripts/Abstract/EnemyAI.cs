using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class EnemyAI : MonoBehaviour
{
    public NavMeshAgent agent;
    [SerializeField] private Transform playerPos;
    [SerializeField] LayerMask whatIsGround;
    [SerializeField] LayerMask whatIsPlayer;

    //Enemy's stats
    [SerializeField] private float health;
    [SerializeField] private float damage;
    [SerializeField] private float expValue;
    [SerializeField] private float goldValue;

    public bool isDead;
    public bool playerIsDead;
    public bool combat;

    //Patrolling
    private Vector3 walkPoint;
    private bool walkPointSet;
    public float walkPointRange; //how far does the monster walk
    private bool isIdling;
    public bool isBeingAttacked;

    //Attacking
    [SerializeField] private GameObject hitbox;
    public bool attacking;

    //States
    public float sightRange, attackRange, distanceFromPlayer; 
    //how far does the player need to be for the monster to see and/or attack him
    public bool playerInSightRange, playerInAttackRange;

    //Animation
    private Animator anim;
    public GameObject itemDrop;

    //kill quest
    [SerializeField] private string type;
    
    public float Damage { get => damage; set => damage = value; }
    public float Health { get => health; set => health = value; }
    public string Type { get => type; set => type = value; }

    void Awake()
    {
        playerPos = GameObject.Find("PlayerCharacter").transform;
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        hitbox.SetActive(false);
    }

    public virtual void Update()
    {
        if(!isDead)
        {
            //check attack range, sight range, distance from player
            playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
            playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
            distanceFromPlayer = Vector3.Distance(transform.position, playerPos.position);

            if (!isIdling)
                if (!playerInSightRange || playerIsDead) Patrolling();

            if(!attacking && !playerIsDead)
                if (playerInSightRange && playerInAttackRange) AttackPlayer();
        }
    }

    private void Patrolling()
    {
        agent.speed = 2f;
        anim.SetBool("Chasing", false);
        anim.SetBool("Attacking", false);
        combat = false;

        if (!walkPointSet) SearchWalkPoint(); //if there's no walk point, create one

        if (walkPointSet) agent.SetDestination(walkPoint);

        Vector3 distancetoWalkPoint = transform.position - walkPoint;
        if (distancetoWalkPoint.magnitude < 1f)
        {
            isIdling = true;
            anim.SetBool("Moving", false);
            StartCoroutine(Idling(Random.Range(0, 4)));
            walkPointSet = false;
        }
    }

    private IEnumerator Idling(float time)
    {
        yield return new WaitForSeconds(time);
        isIdling = false;
    }

    private void SearchWalkPoint()
    {
        //calculate a random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
        {
            walkPointSet = true;
            anim.SetBool("Moving", true);
        }
    }

    public void ChasePlayer()
    {
        if (Vector3.Distance(agent.transform.position, playerPos.transform.position) > 15f)
            agent.speed = 15f;
        else
            agent.speed = 5f;
        anim.SetBool("Moving", true);
        anim.SetBool("Chasing", true);
        agent.SetDestination(playerPos.position);
    }

    private void AttackPlayer()
    {
        if (!anim.GetBool("Attacking"))
            anim.SetFloat("AttackType", Random.Range(0, 2));


        agent.SetDestination(transform.position);
        transform.LookAt(playerPos);
        anim.SetTrigger("Attacking");

        if(!attacking)
            StartCoroutine(ResetAttack(1f));
    }

    public virtual IEnumerator ResetAttack(float time)
    {
        attacking = true;
        yield return new WaitForSeconds(0.6f);
        hitbox.SetActive(true);
        anim.SetBool("Moving", false);
        yield return new WaitForSeconds(0.6f);
        hitbox.SetActive(false);
        yield return new WaitForSeconds(time);
        attacking = false;
    }

    public virtual void TakeDamage(float damage)
    {
        this.health -= damage;
        this.isBeingAttacked = true;
        //add sound effect

        if (!isDead)
        {
            float exp = damage * 5 / 100;
            //playermanager.AddExp(exp);
            Player.instance.AddExp(exp);
            if (health <= 0) MonsterDeath();
        }
    }

    public virtual void MonsterDeath()
    {
        isDead = true;
        //agent.SetDestination(transform.position);
        combat = false;

        Player.instance.AddExp(expValue);

        anim.SetFloat("DeadAnim", Random.Range(0, 3));
        anim.SetTrigger("Dead");
        StartCoroutine(Dying(4));

        //confirm kill
        GameManager.instance.OnKillConfirmed(this);
    }

    public virtual void Stop(Vector3 value) { }

    private IEnumerator Dying(float time)
    {
        GameObject itemDropClone = Instantiate(itemDrop, transform.position, Quaternion.identity);
        itemDropClone.GetComponent<ItemDrop>().goldDrop = goldValue + " gold";
        itemDropClone.GetComponent<ItemDrop>().gold = goldValue;
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
