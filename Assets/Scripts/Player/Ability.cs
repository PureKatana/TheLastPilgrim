using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability : ScriptableObject
{
    public new string name;
    public float cooldownTime;
    public float cooldownTimeUI;
    public float activeTime;
    public int levelRequirement;
    [SerializeField] private bool canUse = true;
    [SerializeField] private bool performingAbility = false;

    public bool CanUse { get => canUse; set => canUse = value; }
    public bool PerformingAbility { get => performingAbility; set => performingAbility = value; }

    public virtual void TriggerAbility()
    {

    }

    public IEnumerator StartCooldown()
    {
        canUse = false;
        for (float i = cooldownTimeUI; i >= 0; i--)
        {
            yield return new WaitForSeconds(1f);
            cooldownTimeUI--;
        }
        canUse = true;
        cooldownTimeUI = cooldownTime;
    }

    public IEnumerator WaitForActiveTime()
    {
        performingAbility = true;
        yield return new WaitForSeconds(activeTime);
        performingAbility = false;
    }
}
