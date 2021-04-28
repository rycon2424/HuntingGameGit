using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAnimation : MonoBehaviour
{
    public ShootingSystem ss;

    public void Reloaded()
    {
        ss.currentGun.mags--;
        ss.currentGun.currentAmmo = ss.currentGun.magSize;
        ss.UpdateAmmoUI(ss.currentGun.currentAmmo, ss.currentGun.mags);
    }
}
