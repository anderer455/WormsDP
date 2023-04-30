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
    public Projectile hoomingRocketPrefab;
    [SerializeField]
    public Projectile c4Prefab;
    [SerializeField]
    public Projectile clusterBombPrefab;
    [SerializeField]
    public Projectile dynamitePrefab;
    [SerializeField]
    public Projectile grenadePrefab;
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

    public WeaponType activeWeapon;
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

        switch (activeWeapon)
        {
            case WeaponType.ROCKETLAUNCHER:
                newSprite = Resources.Load<Sprite>("Sprites/Weapons/Rocket launcher");
                projectilePrefab = rocketPrefab;
                break;
            case WeaponType.HANDGUN:
                newSprite = Resources.Load<Sprite>("Sprites/Weapons/Handgun");
                projectilePrefab = bulletPrefab;
                break;
            case WeaponType.UZI:
                newSprite = Resources.Load<Sprite>("Sprites/Weapons/Uzi");
                projectilePrefab = bulletPrefab;
                break;
            case WeaponType.SHOTGUN:
                newSprite = Resources.Load<Sprite>("Sprites/Weapons/Shotgun");
                projectilePrefab = buckshotPrefab;
                break;
            case WeaponType.ROCKETLAUNCHERB:
                newSprite = Resources.Load<Sprite>("Sprites/Weapons/Rocket launcher blue");
                projectilePrefab = rocketBluePrefab;
                break;
            case WeaponType.C4:
                newSprite = Resources.Load<Sprite>("Sprites/Weapons/Explosives/C4");
                projectilePrefab = c4Prefab;
                break;
            case WeaponType.CLUSTERBOMB:
                newSprite = Resources.Load<Sprite>("Sprites/Weapons/Explosives/Cluster bomb");
                projectilePrefab = clusterBombPrefab;
                break;
            case WeaponType.DYNAMITE:
                newSprite = Resources.Load<Sprite>("Sprites/Weapons/Explosives/Dynamite");
                projectilePrefab = dynamitePrefab;
                break;
            case WeaponType.GRENADE:
                newSprite = Resources.Load<Sprite>("Sprites/Weapons/Explosives/Grenade");
                projectilePrefab = grenadePrefab;
                break;
            case WeaponType.HOLYGRENADE:
                newSprite = Resources.Load<Sprite>("Sprites/Weapons/Explosives/Holy grenade");
                projectilePrefab = holyGrenadePrefab;
                break;
            case WeaponType.HOMINGCLUSTERBOMB:
                newSprite = Resources.Load<Sprite>("Sprites/Weapons/Explosives/Homing cluster bomb");
                projectilePrefab = homingClusterBombPrefab;
                break;
            case WeaponType.MBBOMB:
                newSprite = Resources.Load<Sprite>("Sprites/Weapons/Explosives/MB bomb");
                projectilePrefab = mbbombPrefab;
                break;
            case WeaponType.MINE:
                newSprite = Resources.Load<Sprite>("Sprites/Weapons/Explosives/Mine");
                projectilePrefab = minePrefab;
                break;
            case WeaponType.BANANA:
                newSprite = Resources.Load<Sprite>("Sprites/Weapons/Misc/Banana");
                projectilePrefab = bananaPrefab;
                break;
            default:
                newSprite = Resources.Load<Sprite>("Sprites/Weapons/Rocket launcher");
                projectilePrefab = rocketPrefab;
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

    public void Launch(Vector2 direction, float distance)
    {
        float force = distance * forceMultiplier;
        GameObject projectile = Instantiate(projectilePrefab, launchingPosition.position, Quaternion.identity).gameObject;
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.AddForce(direction * force);
        }
    }
}
