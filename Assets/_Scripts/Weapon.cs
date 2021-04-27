using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Weapon : MonoBehaviour
{
    [Header("Gun Stats")]
    public bool cooldown;
    public bool reloading;
    public ParticleSystem flash;
    [Space]
    public int weaponType;
    public bool semi;
    public float reloadSpeed;
    public float weaponInterval;
    public AudioClip gunShot;
    [Space]
    public string shotAnimName;
    public string reloadAnimName;
    public string equipAnimName;

    public string Shoot(bool aiming)
    {
        if (cooldown == true)
        {
            return "";
        }
        flash.Play();
        StartCoroutine(CoolDown());
        cooldown = true;
        return shotAnimName;
    }

    public void Reload()
    {
        reloading = true;
        StartCoroutine(ReloadCo());
    }
    
    IEnumerator CoolDown()
    {
        yield return new WaitForSeconds(0.1f);
        yield return new WaitForSeconds((weaponInterval - 0.1f));
        cooldown = false;
    }

    IEnumerator ReloadCo()
    {
        yield return new WaitForSeconds(reloadSpeed);
        reloading = false;
    }

}
