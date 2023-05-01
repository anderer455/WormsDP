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

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
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
                float angle;

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
                if (rb.velocity.x < 0 && transform.eulerAngles.y != 180f) {
                    transform.Rotate(0f, 180f, 0f);
                }
                break;
            case ProjectileType.BULLET:
            case ProjectileType.BUCKSHOT:
                if (rb.velocity.x < 0 && transform.eulerAngles.y != 180f) {
                    transform.Rotate(0f, 180f, 0f);
                }
                break;
            default:
                break;
        }
    }
}
