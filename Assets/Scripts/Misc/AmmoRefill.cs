using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoRefill : Interactible
{
    public override void Activate()
    {
        base.Activate();
        GameObject.Find("Player").GetComponentInChildren<Player>().CurrentAmmo = GameObject.Find("Player").GetComponentInChildren<Player>().MaxCurrentAmmo;
        GameObject.Find("Player").GetComponentInChildren<Player>().MaxAmmo = GameObject.Find("Player").GetComponentInChildren<Player>().TrueMaxAmmo;

        GameObject.Find("GameManager").GetComponent<GameManager>().PlaySoundEffect(
                        GameObject.Find("GameManager").GetComponent<GameManager>().GameAudio,
                        GameObject.Find("Player").GetComponentInChildren<ThirdPersonShooterController>().Reload1, 0.3f, 1f);
    }

}
