using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainPosition : MonoBehaviour
{

    private void Awake()
    {
        
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = GameObject.Find("PlayerCharacter").transform.position + new Vector3(0, 10, 0);
    }
}
