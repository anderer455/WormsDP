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
                if (Gameplay.activeTeamColor == TeamColor.BLUE) {
                    WormController.activeProjectile = ProjectileType.ROCKET;
                    WormController.activeWeapon = WeaponType.ROCKETLAUNCHER;
                } else {
                    PlayerController.activeProjectile = ProjectileType.ROCKET;
                    PlayerController.activeWeapon = WeaponType.ROCKETLAUNCHER;
                }
                break;
            case 1:
                if (Gameplay.activeTeamColor == TeamColor.BLUE) {
                    WormController.activeProjectile = ProjectileType.BULLET;
                    WormController.activeWeapon = WeaponType.HANDGUN;
                } else {
                    PlayerController.activeProjectile = ProjectileType.BULLET;
                    PlayerController.activeWeapon = WeaponType.HANDGUN;
                }
                break;
            case 2:
                if (Gameplay.activeTeamColor == TeamColor.BLUE) {
                    WormController.activeProjectile = ProjectileType.BULLET;
                    WormController.activeWeapon = WeaponType.UZI;
                } else {
                    PlayerController.activeProjectile = ProjectileType.BULLET;
                    PlayerController.activeWeapon = WeaponType.UZI;
                }
                break;
            case 3:
                if (Gameplay.activeTeamColor == TeamColor.BLUE) {
                    WormController.activeProjectile = ProjectileType.BUCKSHOT;
                    WormController.activeWeapon = WeaponType.SHOTGUN;
                } else {
                    PlayerController.activeProjectile = ProjectileType.BUCKSHOT;
                    PlayerController.activeWeapon = WeaponType.SHOTGUN;
                }
                break;
            case 4:
                if (Gameplay.activeTeamColor == TeamColor.BLUE) {
                    WormController.activeProjectile = ProjectileType.ROCKETBLUE;
                    WormController.activeWeapon = WeaponType.ROCKETLAUNCHERB;
                } else {
                    PlayerController.activeProjectile = ProjectileType.ROCKETBLUE;
                    PlayerController.activeWeapon = WeaponType.ROCKETLAUNCHERB;
                }
                break;
            case 5:
                if (Gameplay.activeTeamColor == TeamColor.BLUE) {
                    WormController.activeProjectile = ProjectileType.HOMINGROCKET;
                    WormController.activeWeapon = WeaponType.ROCKETLAUNCHERB;
                } else {
                    PlayerController.activeProjectile = ProjectileType.HOMINGROCKET;
                    PlayerController.activeWeapon = WeaponType.ROCKETLAUNCHERB;
                }
                break;
            case 6:
                if (Gameplay.activeTeamColor == TeamColor.BLUE) {
                    WormController.activeProjectile = ProjectileType.C4;
                    WormController.activeWeapon = WeaponType.C4;
                } else {
                    PlayerController.activeProjectile = ProjectileType.C4;
                    PlayerController.activeWeapon = WeaponType.C4;
                }
                break;
            case 7:
                if (Gameplay.activeTeamColor == TeamColor.BLUE) {
                    WormController.activeProjectile = ProjectileType.GRENADE;
                    WormController.activeWeapon = WeaponType.GRENADE;
                } else {
                    PlayerController.activeProjectile = ProjectileType.GRENADE;
                    PlayerController.activeWeapon = WeaponType.GRENADE;
                }
                break;
            case 8:
                if (Gameplay.activeTeamColor == TeamColor.BLUE) {
                    WormController.activeProjectile = ProjectileType.DYNAMITE;
                    WormController.activeWeapon = WeaponType.DYNAMITE;
                } else {
                    PlayerController.activeProjectile = ProjectileType.DYNAMITE;
                    PlayerController.activeWeapon = WeaponType.DYNAMITE;
                }
                break;
            case 9:
                if (Gameplay.activeTeamColor == TeamColor.BLUE) {
                    WormController.activeProjectile = ProjectileType.CLUSTERBOMB;
                    WormController.activeWeapon = WeaponType.CLUSTERBOMB;
                } else {
                    PlayerController.activeProjectile = ProjectileType.CLUSTERBOMB;
                    PlayerController.activeWeapon = WeaponType.CLUSTERBOMB;
                }
                break;
            case 10:
                if (Gameplay.activeTeamColor == TeamColor.BLUE) {
                    WormController.activeProjectile = ProjectileType.HOLYGRENADE;
                    WormController.activeWeapon = WeaponType.HOLYGRENADE;
                } else {
                    PlayerController.activeProjectile = ProjectileType.HOLYGRENADE;
                    PlayerController.activeWeapon = WeaponType.HOLYGRENADE;
                }
                break;
            case 11:
                if (Gameplay.activeTeamColor == TeamColor.BLUE) {
                    WormController.activeProjectile = ProjectileType.HOMINGCLUSTERBOMB;
                    WormController.activeWeapon = WeaponType.HOMINGCLUSTERBOMB;
                } else {
                    PlayerController.activeProjectile = ProjectileType.HOMINGCLUSTERBOMB;
                    PlayerController.activeWeapon = WeaponType.HOMINGCLUSTERBOMB;
                }
                break;
            case 12:
                if (Gameplay.activeTeamColor == TeamColor.BLUE) {
                    WormController.activeProjectile = ProjectileType.MBBOMB;
                    WormController.activeWeapon = WeaponType.MBBOMB;
                } else {
                    PlayerController.activeProjectile = ProjectileType.MBBOMB;
                    PlayerController.activeWeapon = WeaponType.MBBOMB;
                }
                break;
            case 13:
                if (Gameplay.activeTeamColor == TeamColor.BLUE) {
                    WormController.activeProjectile = ProjectileType.MINE;
                    WormController.activeWeapon = WeaponType.MINE;
                } else {
                    PlayerController.activeProjectile = ProjectileType.MINE;
                    PlayerController.activeWeapon = WeaponType.MINE;
                }
                break;
            case 14:
                if (Gameplay.activeTeamColor == TeamColor.BLUE) {
                    WormController.activeProjectile = ProjectileType.BANANA;
                    WormController.activeWeapon = WeaponType.BANANA;
                } else {
                    PlayerController.activeProjectile = ProjectileType.BANANA;
                    PlayerController.activeWeapon = WeaponType.BANANA;
                }
                break;
            default:
                if (Gameplay.activeTeamColor == TeamColor.BLUE) {
                    WormController.activeProjectile = ProjectileType.ROCKET;
                    WormController.activeWeapon = WeaponType.ROCKETLAUNCHER;
                } else {
                    PlayerController.activeProjectile = ProjectileType.ROCKET;
                    PlayerController.activeWeapon = WeaponType.ROCKETLAUNCHER;
                }
                break;
        }

        CloseInventory();
    }
}
