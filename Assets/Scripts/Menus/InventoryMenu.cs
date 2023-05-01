using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryMenu : MonoBehaviour
{
    public GameObject inventoryMenu;
    public static bool isOpened;

    // Start is called before the first frame update
    void Start()
    {
        inventoryMenu.SetActive(false);
        isOpened = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I)) {
            if (isOpened) {
                CloseInventory();
            } else {
                OpenInventory();
            }
        }
    }

    public void OpenInventory() {
        inventoryMenu.SetActive(true);
        Time.timeScale = 0f;
        isOpened = true;
    }

    public void CloseInventory() {
        inventoryMenu.SetActive(false);
        Time.timeScale = 1f;
        isOpened = false;
    }

    public void ButtonPressed(int index) {
        switch (index) {
            case 0:
                WormController.activeProjectile = ProjectileType.ROCKET;
                WormController.activeWeapon = WeaponType.ROCKETLAUNCHER;
                break;
            case 1:
                WormController.activeProjectile = ProjectileType.BULLET;
                WormController.activeWeapon = WeaponType.HANDGUN;
                break;
            case 2:
                WormController.activeProjectile = ProjectileType.BULLET;
                WormController.activeWeapon = WeaponType.UZI;
                break;
            case 3:
                WormController.activeProjectile = ProjectileType.BUCKSHOT;
                WormController.activeWeapon = WeaponType.SHOTGUN;
                break;
            case 4:
                WormController.activeProjectile = ProjectileType.ROCKETBLUE;
                WormController.activeWeapon = WeaponType.ROCKETLAUNCHERB;
                break;
            case 5:
                WormController.activeProjectile = ProjectileType.HOMINGROCKET;
                WormController.activeWeapon = WeaponType.ROCKETLAUNCHERB;
                break;
            case 6:
                WormController.activeProjectile = ProjectileType.C4;
                WormController.activeWeapon = WeaponType.C4;
                break;
            case 7:
                WormController.activeProjectile = ProjectileType.GRENADE;
                WormController.activeWeapon = WeaponType.GRENADE;
                break;
            case 8:
                WormController.activeProjectile = ProjectileType.DYNAMITE;
                WormController.activeWeapon = WeaponType.DYNAMITE;
                break;
            case 9:
                WormController.activeProjectile = ProjectileType.CLUSTERBOMB;
                WormController.activeWeapon = WeaponType.CLUSTERBOMB;
                break;
            case 10:
                WormController.activeProjectile = ProjectileType.HOLYGRENADE;
                WormController.activeWeapon = WeaponType.HOLYGRENADE;
                break;
            case 11:
                WormController.activeProjectile = ProjectileType.HOMINGCLUSTERBOMB;
                WormController.activeWeapon = WeaponType.HOMINGCLUSTERBOMB;
                break;
            case 12:
                WormController.activeProjectile = ProjectileType.MBBOMB;
                WormController.activeWeapon = WeaponType.MBBOMB;
                break;
            case 13:
                WormController.activeProjectile = ProjectileType.MINE;
                WormController.activeWeapon = WeaponType.MINE;
                break;
            case 14:
                WormController.activeProjectile = ProjectileType.BANANA;
                WormController.activeWeapon = WeaponType.BANANA;
                break;
            default:
                WormController.activeProjectile = ProjectileType.ROCKET;
                WormController.activeWeapon = WeaponType.ROCKETLAUNCHER;
                break;
        }

        CloseInventory();
    }
}
