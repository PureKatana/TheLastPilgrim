using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody rigid;
    [SerializeField] private float speed;
    [SerializeField] private Transform bulletImpact;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        rigid.velocity = transform.forward * speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        //add particles
        Instantiate(bulletImpact, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
