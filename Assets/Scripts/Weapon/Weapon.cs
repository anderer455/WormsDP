using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType { ROCKETLAUNCHER, HANDGUN, UZI, SHOTGUN, ROCKETLAUNCHERB, C4, CLUSTERBOMB, DYNAMITE, GRENADE, HOLYGRENADE, HOMINGCLUSTERBOMB, MBBOMB, MINE, BANANA }

public class Weapon : MonoBehaviour
{
    [SerializeField]
    public float forceMultiplier;
    [SerializeField]
    public Projectile rocketPrefab;
    [SerializeField]
    public Projectile bulletPrefab;
    [SerializeField]
    public Projectile buckshotPrefab;
    [SerializeField]
    public Projectile rocketBluePrefab;
    [SerializeField]
    public Projectile homingRocketPrefab;
    [SerializeField]
    public Projectile c4Prefab;
    [SerializeField]
    public Projectile grenadePrefab;
    [SerializeField]
    public Projectile clusterBombPrefab;
    [SerializeField]
    public Projectile dynamitePrefab;
    [SerializeField]
    public Projectile holyGrenadePrefab;
    [SerializeField]
    public Projectile homingClusterBombPrefab;
    [SerializeField]
    public Projectile mbbombPrefab;
    [SerializeField]
    public Projectile minePrefab;
    [SerializeField]
    public Projectile bananaPrefab;
    [SerializeField]
    private Transform launchingPosition;

    private Projectile projectilePrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetSprite()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        }

        Sprite newSprite;

        switch (WormController.activeWeapon)
        {
            case WeaponType.ROCKETLAUNCHER:
                newSprite = Resources.Load<Sprite>("Sprites/Weapons/Rocket launcher");
                break;
            case WeaponType.HANDGUN:
                newSprite = Resources.Load<Sprite>("Sprites/Weapons/Handgun");
                break;
            case WeaponType.UZI:
                newSprite = Resources.Load<Sprite>("Sprites/Weapons/Uzi");
                break;
            case WeaponType.SHOTGUN:
                newSprite = Resources.Load<Sprite>("Sprites/Weapons/Shotgun");
                break;
            case WeaponType.ROCKETLAUNCHERB:
                newSprite = Resources.Load<Sprite>("Sprites/Weapons/Rocket launcher blue");
                break;
            case WeaponType.C4:
                newSprite = Resources.Load<Sprite>("Sprites/Weapons/Explosives/C4");
                break;
            case WeaponType.CLUSTERBOMB:
                newSprite = Resources.Load<Sprite>("Sprites/Weapons/Explosives/Cluster bomb");
                break;
            case WeaponType.DYNAMITE:
                newSprite = Resources.Load<Sprite>("Sprites/Weapons/Explosives/Dynamite");
                break;
            case WeaponType.GRENADE:
                newSprite = Resources.Load<Sprite>("Sprites/Weapons/Explosives/Grenade");
                break;
            case WeaponType.HOLYGRENADE:
                newSprite = Resources.Load<Sprite>("Sprites/Weapons/Explosives/Holy grenade");
                break;
            case WeaponType.HOMINGCLUSTERBOMB:
                newSprite = Resources.Load<Sprite>("Sprites/Weapons/Explosives/Homing cluster bomb");
                break;
            case WeaponType.MBBOMB:
                newSprite = Resources.Load<Sprite>("Sprites/Weapons/Explosives/MB bomb");
                break;
            case WeaponType.MINE:
                newSprite = Resources.Load<Sprite>("Sprites/Weapons/Explosives/Mine");
                break;
            case WeaponType.BANANA:
                newSprite = Resources.Load<Sprite>("Sprites/Weapons/Misc/Banana");
                break;
            default:
                newSprite = Resources.Load<Sprite>("Sprites/Weapons/Rocket launcher");
                break;
        }

        
        if (newSprite != null)
        {
            spriteRenderer.sprite = newSprite;
        }
    }

    public void ClearSprite()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = null;
        }
    }

    public void Launch(Vector2 direction, float distance, float maxDistance, ProjectileType projectileType)
    {
        float force = distance * forceMultiplier;

        switch (projectileType)
        {
            case ProjectileType.C4:
                projectilePrefab = c4Prefab;
                break;
            case ProjectileType.CLUSTERBOMB:
                projectilePrefab = clusterBombPrefab;
                break;
            case ProjectileType.DYNAMITE:
                projectilePrefab = dynamitePrefab;
                break;
            case ProjectileType.GRENADE:
                projectilePrefab = grenadePrefab;
                break;
            case ProjectileType.HOLYGRENADE:
                projectilePrefab = holyGrenadePrefab;
                break;
            case ProjectileType.HOMINGCLUSTERBOMB:
                projectilePrefab = homingClusterBombPrefab;
                break;
            case ProjectileType.MBBOMB:
                projectilePrefab = mbbombPrefab;
                break;
            case ProjectileType.MINE:
                projectilePrefab = minePrefab;
                break;
            case ProjectileType.BANANA:
                projectilePrefab = bananaPrefab;
                break;
            case ProjectileType.ROCKET:
                projectilePrefab = rocketPrefab;
                break;
            case ProjectileType.ROCKETBLUE:
                projectilePrefab = rocketBluePrefab;
                break;
            case ProjectileType.HOMINGROCKET:
                projectilePrefab = homingRocketPrefab;
                break;
            case ProjectileType.BULLET:
                projectilePrefab = bulletPrefab;
                force = maxDistance * forceMultiplier;
                break;
            case ProjectileType.BUCKSHOT:
                projectilePrefab = buckshotPrefab;
                force = maxDistance * forceMultiplier;
                break;
            default:
                projectilePrefab = rocketPrefab;
                break;
        }

        GameObject projectile = Instantiate(projectilePrefab, launchingPosition.position, Quaternion.identity).gameObject;
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.AddForce(direction * force);
        }

        Gameplay.TurnTimer = 5f;
    }
}
