using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectMenu : MonoBehaviour
{

    [SerializeField] private Selectable defaultItems;
    // Start is called before the first frame update
    void Start()
    {
        defaultItems.Select();
    }

   

    // Update is called once per frame
    void Update()
    {
        
    }
}
