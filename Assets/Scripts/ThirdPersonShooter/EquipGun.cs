using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipGun : MonoBehaviour
{
    private Material mat;
    public bool isUnequipped = false;

    private void Start()
    {
        mat = GetComponent<Renderer>().material;
        mat.SetFloat("_Dissolve", 1);
    }
    public void Equip()
    {
        isUnequipped = false;
        mat.SetFloat("_Dissolve", 0);
    }

    public void Unequip()
    {
        isUnequipped = true;
        StartCoroutine("Dissolve");
    }

    private IEnumerator Dissolve()
    {
        for (float i = 0; i <= 1; i += 0.01f)
        {
            yield return new WaitForSeconds(0.01f);
            mat.SetFloat("_Dissolve", i);
        }
    }
}
