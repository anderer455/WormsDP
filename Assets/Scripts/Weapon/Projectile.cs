using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ProjectileType { ROCKET, BULLET, BUCKSHOT, ROCKETBLUE, HOMINGROCKET, C4, GRENADE, CLUSTERBOMB, DYNAMITE, HOLYGRENADE, HOMINGCLUSTERBOMB, MBBOMB, MINE, BANANA }

public class Projectile : MonoBehaviour
{
    [SerializeField]
    public int damage;

    public ProjectileType activeProjectile;
    private Rigidbody2D rb;
    private float angle;
    private float radius;
    private bool hasHitGround = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        gameObject.layer = LayerMask.NameToLayer("Projectiles");
    }

    // Update is called once per frame
    void Update()
    {
        if (activeProjectile == ProjectileType.HOMINGROCKET)
        {
            GameObject target = null;
            float minXDistance = Mathf.Infinity;
            bool hasTurned = false;

            if (Gameplay.activeTeamColor == TeamColor.BLUE)
            {
                foreach (PlayerController player in FindObjectsOfType<PlayerController>())
                {
                    float xDistance = Mathf.Abs(transform.position.x - player.transform.position.x);
                    if (xDistance < minXDistance)
                    {
                        minXDistance = xDistance;
                        target = player.gameObject;
                    }
                }
            }
            else
            {
                foreach (WormController worm in FindObjectsOfType<WormController>())
                {
                    float xDistance = Mathf.Abs(transform.position.x - worm.transform.position.x);
                    if (xDistance < minXDistance)
                    {
                        minXDistance = xDistance;
                        target = worm.gameObject;
                    }
                }
            }

            if (target != null && !hasTurned) {
                hasTurned = TurnHomingRocket(minXDistance);
            }
        }
    }

    void FixedUpdate()
    {
        switch (activeProjectile)
        {
            case ProjectileType.C4:
            case ProjectileType.GRENADE:
            case ProjectileType.CLUSTERBOMB:
            case ProjectileType.DYNAMITE:
            case ProjectileType.HOLYGRENADE:
            case ProjectileType.HOMINGCLUSTERBOMB:
            case ProjectileType.MBBOMB:
            case ProjectileType.MINE:
            case ProjectileType.BANANA:
                if (activeProjectile == ProjectileType.C4) { radius = 2f; }
                else if (activeProjectile == ProjectileType.GRENADE) { radius = 1f; }
                else if (activeProjectile == ProjectileType.CLUSTERBOMB) { radius = 2f; }
                else if (activeProjectile == ProjectileType.DYNAMITE) { radius = 3f; }
                else if (activeProjectile == ProjectileType.HOLYGRENADE) { radius = 3f; }
                else if (activeProjectile == ProjectileType.HOMINGCLUSTERBOMB) { radius = 2f; }
                else if (activeProjectile == ProjectileType.MBBOMB) { radius = 2f; }
                else if (activeProjectile == ProjectileType.MINE) { radius = 1f; }
                else if (activeProjectile == ProjectileType.BANANA) { radius = 3f; }

                if (rb.velocity.x < 0) {
                    angle = 120f;
                } else if (rb.velocity.x > 0) {
                    angle = -120f;
                } else {
                    angle = 0f;
                }
                
                transform.Rotate(new Vector3(0f, 0f, angle) * Time.deltaTime);
                break;
            case ProjectileType.ROCKET:
            case ProjectileType.ROCKETBLUE:
            case ProjectileType.HOMINGROCKET:
                if (activeProjectile == ProjectileType.ROCKET) { radius = 1f; }
                else if (activeProjectile == ProjectileType.ROCKETBLUE) { radius = 2f; }
                else if (activeProjectile == ProjectileType.HOMINGROCKET) { radius = 2f; }
                if (rb.velocity.x < 0 && transform.eulerAngles.y != 180f) {
                    transform.Rotate(0f, 180f, 0f);
                }
                angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
                break;
            case ProjectileType.BULLET:
            case ProjectileType.BUCKSHOT:
                gameObject.layer = LayerMask.NameToLayer("Default");
                if (rb.velocity.x < 0 && transform.eulerAngles.y != 180f) {
                    transform.Rotate(0f, 180f, 0f);
                }
                angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
                break;
            default:
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.TryGetComponent<Bottom>(out Bottom bottom)) {
            Destroy(this.gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (activeProjectile != ProjectileType.BULLET && activeProjectile != ProjectileType.BUCKSHOT) {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Ground") && !hasHitGround) {
                hasHitGround = true;
                Vector2 center = collision.contacts[0].point;
                LayerMask layerMask = LayerMask.GetMask("Worms");

                Collider2D[] colliders = Physics2D.OverlapCircleAll(center, radius, layerMask);
                foreach (Collider2D col in colliders) {
                    if (col.TryGetComponent<WormController>(out WormController worm)) {
                        worm.TakeDamage(damage);
                    } else if (col.TryGetComponent<PlayerController>(out PlayerController playerWorm)) {
                        playerWorm.TakeDamage(damage);
                    }
                }
            }
        } else if (collision.gameObject.TryGetComponent<Bottom>(out Bottom bottom)) {
            Destroy(this.gameObject);
        } else if (collision.gameObject.TryGetComponent<WormController>(out WormController worm)) {
            worm.TakeDamage(damage);
            Destroy(this.gameObject);
        } else if (collision.gameObject.TryGetComponent<PlayerController>(out PlayerController playerWorm)) {
            playerWorm.TakeDamage(damage);
            Destroy(this.gameObject);
        }
    }

    bool TurnHomingRocket(float minXDistance) {
        float distanceThreshold = 0.25f; 
        if (minXDistance <= distanceThreshold) {
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            if (rb != null) {
                rb.velocity = Vector2.down;
                rb.gravityScale = 50;
                transform.rotation = Quaternion.Euler(0, 0, -90);
            }
            return true;
        }
        return false;
    }
}
