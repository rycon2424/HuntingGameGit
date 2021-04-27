using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingSystem : MonoBehaviour
{
    public Weapon currentGun;
    public AudioSource GunShot;
    public GameObject crossHair;
    public Animation weaponsAnim;

    public void ShootMechanic(FPSPlayer fp)
    {
        if (fp.sprinting == true)
        {
            fp.thirdPersonAnim.SetBool("aiming", false);
            fp.aimAnim.SetBool("Aim", false);
            return;
        }
        if (Input.GetMouseButton(1))
        {
            fp.thirdPersonAnim.SetBool("aiming", true);
            fp.aimAnim.SetBool("Aim", true);
        }
        else
        {
            fp.thirdPersonAnim.SetBool("aiming", false);
            fp.aimAnim.SetBool("Aim", false);
        }
        crossHair.SetActive(!fp.aimAnim.GetBool("Aim"));
        if (currentGun.reloading == true)
        {
            return;
        }
        if (currentGun.semi)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Shoot(fp);
            }
        }
        else
        {
            if (Input.GetMouseButton(0))
            {
                Shoot(fp);
            }
        }
        if (Input.GetKeyDown(fp.pc.reload))
        {
            Reload();
        }
    }

    void Reload()
    {
        if (currentGun.reloading == false && currentGun.cooldown == false)
        {
            weaponsAnim.Play(currentGun.reloadAnimName);
            currentGun.Reload();
        }
    }

    void Shoot(FPSPlayer fp)
    {
        string shot = currentGun.Shoot(fp.aimAnim.GetBool("Aim"));
        if (shot != "")
        {
            fp.fpsAnim.SetTrigger("Shoot");
            weaponsAnim.Play(shot);
            //GunShot.clip = currentGun.gunShot;
            //GunShot.Play();
        }
    }

    public void SwapWeapon(Weapon newWeapon)
    {
        weaponsAnim.Play(newWeapon.equipAnimName);
        currentGun = newWeapon;
    }
    
}
