using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Scripts.Weapon;
using Scripts.Items;

public class WeaponManager : MonoBehaviour
{
    public FireArms mainWeapon;
    public FireArms pistol;
    public FireArms takeGun;

    private FireArms curWeapon;
    private PlayerMovement player;

    public Transform godCam;
    public float raycastDis;
    public LayerMask itemLayer;
    public GameObject medkit;

    public GameObject crosshair;
    public Text ammoText;
    public Image MainWeaponBackground;
    public Image pistolBackground;

    internal bool isFiring;
    private bool took;

    public bool isDead
    { get { return player.Health <= 0; } }

    private void Start()
    {
        curWeapon = pistol;
        player = FindObjectOfType<PlayerMovement>();
        player.setAnimator(curWeapon.GunAni);
    }

    private void Update()
    {
        if (!isDead) {
            CheckItem();

            if (!curWeapon) return;

            if (pistol.pool.transform.childCount >= 1 && curWeapon == pistol)
                pistol.bulletPool.Clear();
            if (took) // took ak47
                if (mainWeapon.pool.transform.childCount >= 1 && curWeapon == mainWeapon)
                    mainWeapon.bulletPool.Clear();

            SwapWeapon();

            if (Input.GetMouseButton(0)) {
                if (!curWeapon.isPlaying(2)) {
                    curWeapon.shootTime += Time.deltaTime;
                    curWeapon.shootTriggerOn();
                    isFiring = true;
                }
            }
            else
            {
                curWeapon.shootTriggerOff();
                isFiring = false;
            }

            if (Input.GetKeyDown(KeyCode.R)) {
                if (curWeapon.curAmmo >= 0 && curWeapon.curMaxAmmo > 0 && !curWeapon.isPlaying(2))
                    curWeapon.reloadAmmo();
            }

            if (Input.GetMouseButton(1)) {
                crosshair.SetActive(false);
                curWeapon.Aim(true);
            }
            else {
                crosshair.SetActive(true);
                curWeapon.Aim(false);
            }

            AmmoInfo(curWeapon.curAmmo, curWeapon.curMaxAmmo);
            if (curWeapon.curMaxAmmo < 0)
                curWeapon.curMaxAmmo = 0;

            // auto reload
            if (curWeapon.curAmmo <= 0 && curWeapon.curMaxAmmo > 0 && !curWeapon.isPlaying(2))
                curWeapon.reloadAmmo();
        }
    }

    private void SwapWeapon()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && curWeapon == pistol) {
            if (mainWeapon != null) {
                SetupWeapon(mainWeapon);
                player.setAnimator(curWeapon.GunAni);
                pistol.bulletPool.Clear();
                MainWeaponBackground.color = new Color(255,0,0,255);
                pistolBackground.color = new Color(255,255,255,255);
            }
        } 
        else if (Input.GetKeyDown(KeyCode.Alpha2) && curWeapon == mainWeapon) {
            if (pistol != null) {
                SetupWeapon(pistol);
                player.setAnimator(curWeapon.GunAni);
                mainWeapon.bulletPool.Clear();
                MainWeaponBackground.color = new Color(255,255,255,255);
                pistolBackground.color = new Color(255,0,0,255);
            }
        }
    }

    private void SetupWeapon(FireArms weapon)
    {
        curWeapon.gameObject.SetActive(false);
        curWeapon = weapon;
        curWeapon.gameObject.SetActive(true);
    }

    private void AmmoInfo(int ammo, int remain)
    {
        ammoText.text = ammo.ToString() + " / " + remain.ToString();
    }

    //item
    private void CheckItem()
    {
        // raycast > itemLayer
        bool item = Physics.Raycast(godCam.position, godCam.forward, out RaycastHit hit, raycastDis, itemLayer);

        if (item) {
            if(Input.GetKeyDown(KeyCode.E)) {
                // try get gameobject component<Items>
                bool itemMatch = hit.collider.TryGetComponent(out Items items);
                if (itemMatch) {
                    // if items is gun
                    if (items is ArmsItem gun) {
                        if (gun.curGun == ArmsItem.GunType.AK47 && !took) {
                            mainWeapon = takeGun;
                            SetupWeapon(mainWeapon);
                            took = true;
                        }

                        if (gun.curGun == ArmsItem.GunType.Ammo) {
                            curWeapon.curAmmo = curWeapon.Ammo;
                            curWeapon.curMaxAmmo = curWeapon.MaxAmmo;
                        }

                        if (gun.curGun == ArmsItem.GunType.Medkit) {
                            if (player.Health < 100) {
                                player.AddHP(50);
                                Destroy(hit.collider.gameObject);
                            }
                        }
                    }
                }
            }
        }
    }
}
