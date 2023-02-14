using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Burning : MonoBehaviour
{
    [SerializeField] private float duration;
    [SerializeField] private float damagePerSecond;
    [SerializeField] private AudioClip burningSFX;
    private AudioSource burningAudio;
    //[SerializeField] private Transform burningEffect;
    private Player player;

    public float DamagePerSecond { get => damagePerSecond; set => damagePerSecond = value; }

    // Start is called before the first frame update
    void Start()
    {
        player = Player.instance;
        burningAudio = GetComponent<AudioSource>();
        this.DamagePerSecond = DamagePerSecond + (1 * Player.instance.Level);
    }

    private void Update()
    {
        if(player.isBurned)
        {
            gameObject.SetActive(true);
            GameObject.Find("GameManager").GetComponent<GameManager>().PlaySoundEffect(burningAudio, burningSFX, 0.2f, 1f);
            StartCoroutine(BurningEffect());
            player.isBurned = false;
        }
    }  
    

    private IEnumerator BurningEffect()
    { 
        for (int i = 0; i <= duration; i++)
        {
            GameObject.Find("Player").GetComponentInChildren<Player>().TakeDamage(damagePerSecond);
            yield return new WaitForSeconds(1.5f);
        }

        gameObject.SetActive(false);
    }
}
