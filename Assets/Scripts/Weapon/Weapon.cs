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
    private float launchDelay = 0.1f;

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
        WeaponType activeWeapon;

        if (Gameplay.activeTeamColor == TeamColor.BLUE) {
            activeWeapon = WormController.activeWeapon;
        } else {
            activeWeapon = PlayerController.activeWeapon;
        }

        switch (activeWeapon)
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

    public void Launch(Vector2 direction, float distance, float maxDistance, ProjectileType projectileType, WeaponType weaponType) {
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

        if (Gameplay.TurnTimer > 5f) {
            Gameplay.TurnTimer = 5f;
        }

        if (weaponType == WeaponType.UZI) {
            StartCoroutine(LaunchMultipleProjectiles(direction, force));
        } else if (weaponType == WeaponType.SHOTGUN) {
            LaunchBuckshot(direction, force);
        } else {
            LaunchSingleProjectile(direction, force);
        }

        if (Gameplay.activeTeamColor == TeamColor.BLUE) {
            WormController.activeWeapon = WeaponType.ROCKETLAUNCHER;
            WormController.activeProjectile = ProjectileType.ROCKET;
        } else {
            PlayerController.activeWeapon = WeaponType.ROCKETLAUNCHER;
            PlayerController.activeProjectile = ProjectileType.ROCKET;
        }
    }

    private void LaunchBuckshot(Vector2 direction, float force) {
        int numberOfPellets = 5;
        float spreadAngle = 30f;
        float angleStep = spreadAngle / (numberOfPellets - 1);
        float startAngle = -spreadAngle / 2;

        for (int i = 0; i < numberOfPellets; i++) {
            float currentAngle = startAngle + angleStep * i;
            Quaternion pelletRotation = Quaternion.Euler(0, 0, currentAngle);
            Vector2 pelletDirection = pelletRotation * direction;
            pelletDirection.Normalize();
            GameObject pellet = Instantiate(buckshotPrefab, launchingPosition.position, Quaternion.identity).gameObject;
            Rigidbody2D rb = pellet.GetComponent<Rigidbody2D>();
            if (rb != null) {
                rb.AddForce(pelletDirection * force);
            }
        }
    }

    private void LaunchSingleProjectile(Vector2 direction, float force) {
        GameObject projectile = Instantiate(projectilePrefab, launchingPosition.position, Quaternion.identity).gameObject;
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        if (rb != null) {
            rb.AddForce(direction * force);
        }
    }

    private IEnumerator LaunchMultipleProjectiles(Vector2 direction, float force) {
        for (int i = 0; i < 3; i++) {
            GameObject projectile = Instantiate(projectilePrefab, launchingPosition.position, Quaternion.identity).gameObject;
            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
            if (rb != null) {
                rb.AddForce(direction * force);
            }
            yield return new WaitForSeconds(launchDelay);
        }
    }
}
