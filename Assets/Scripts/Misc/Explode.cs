using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explode : MonoBehaviour
{
    public int time;

    // Update is called once per frame
    void Update()
    {
        Destroy(gameObject, time);
    }
}
