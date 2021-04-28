using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class ShootingSystem : MonoBehaviour
{
    [Header("References")]
    public Weapon currentGun;
    public Weapon primary;
    public Weapon lantern;
    [Space]
    public GameObject[] allWeaponsTP;
    [Space]
    public AudioSource GunShot;
    public GameObject crossHair;
    public Animation weaponsAnim;

    FPSPlayer fp;

    private void Awake()
    {
        fp = GetComponent<FPSPlayer>();
    }

    private void Start()
    {
        weaponName.text = currentGun.weaponName;
        UpdateAmmoUI(currentGun.currentAmmo, currentGun.mags);
    }

    public void ShootMechanic()
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
                Shoot();
            }
        }
        else
        {
            if (Input.GetMouseButton(0))
            {
                Shoot();
            }
        }
        if (Input.GetKeyDown(fp.pc.reload))
        {
            Reload();
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwapWeapon(primary);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwapWeapon(lantern);
        }
    }

    void Reload()
    {
        if (currentGun.reloading == false && currentGun.cooldown == false && currentGun.mags > 0)
        {
            weaponsAnim.Play(currentGun.reloadAnimName);
            currentGun.Reload();
        }
    }

    void Shoot()
    {
        string shot = currentGun.Shoot();

        if (shot != "")
        {
            fp.fpsAnim.SetTrigger("Shoot");
            weaponsAnim.Play(shot);
            currentGun.currentAmmo--;
            UpdateAmmoUI(currentGun.currentAmmo, currentGun.mags);
            fp.pv.RPC("ShootSync", RpcTarget.All);
            //GunShot.clip = currentGun.gunShot;
            //GunShot.Play();
        }
    }

    [PunRPC]
    void ShootSync()
    {
        currentGun.TPflash.Play();
    }

    public void SwapWeapon(Weapon newWeapon)
    {
        fp.fpsAnim.SetTrigger("Swap");

        currentGun.gameObject.SetActive(false);
        newWeapon.gameObject.SetActive(true);

        weaponsAnim.Play(newWeapon.equipAnimName);
        currentGun = newWeapon;
        weaponName.text = currentGun.weaponName;
        UpdateAmmoUI(currentGun.currentAmmo, currentGun.mags);

        fp.pv.RPC("SwapWeaponSync", RpcTarget.All, currentGun.gameObject.name);
    }

    [PunRPC]
    void SwapWeaponSync(string weapon)
    {
        fp.thirdPersonAnim.SetLayerWeight(1, 0);
        foreach (GameObject g in allWeaponsTP)
        {
            if (g.name == weapon)
            {
                g.SetActive(true);
                if (g.name == "Lantern") // Maybe change later to onehanded?
                {
                    fp.thirdPersonAnim.SetLayerWeight(1, 1);
                }
            }
            else
            {
                g.SetActive(false);
            }
        }
    }

    [Header("UI")]
    public Text weaponName;
    public Text currentAmmo;
    public Text maxAmmo;

    public void UpdateAmmoUI(int cAmmo, int magsLeft)
    {
        currentAmmo.text = cAmmo.ToString();
        maxAmmo.text = magsLeft.ToString();
    }
    
}
