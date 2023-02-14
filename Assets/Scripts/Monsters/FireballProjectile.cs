using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballProjectile : MonoBehaviour
{
    private Rigidbody rigid;
    private Player player;
    [SerializeField] private float speed;
    [SerializeField] private Transform fireballHit;
    [SerializeField] private float damage;
    private GameManager manager;
    private bool wait = false;

    public float Damage { get => damage; set => damage = value; }

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        rigid.velocity = transform.forward * speed;
        player = Player.instance;
        manager = GameManager.instance;
        this.Damage = Damage + (10 * Player.instance.Level);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !player.shieldOn)
        {
            player.TakeDamage(damage);
            player.isBurned = true;
            player.Burning.SetActive(true);
        }
        
        Instantiate(fireballHit, transform.position, Quaternion.identity);
        manager.PlaySoundEffect(GameObject.Find("FireballSpawn").GetComponent<AudioSource>(),manager.FireballHit, 0.5f, 1f);
        if (!other.CompareTag("Hitbox") && !other.CompareTag("Boss"))
           Destroy(gameObject);
    }

    private void Update()
    {
        //If it doesn't collide with anything, destroy after 10 seconds
        if (!wait)
        {
            StartCoroutine(DestroyAfterNoHit());
            wait = true;
        }
    }

    private IEnumerator DestroyAfterNoHit()
    {
        yield return new WaitForSeconds(5);
        Instantiate(fireballHit, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
