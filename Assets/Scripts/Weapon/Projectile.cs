using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ProjectileType { ROCKET, BULLET, BUCKSHOT, ROCKETBLUE, HOOMINGROCKET, C4, CLUSTERBOMB, DYNAMITE, GRENADE, HOLYGRENADE, HOMINGCLUSTERBOMB, MBBOMB, MINE, BANANA }

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
        float angle;

        if (rb.velocity.x < 0) {
            angle = 120f;
        } else if (rb.velocity.x > 0) {
            angle = -120f;
        } else {
            angle = 0;
        }
        Debug.Log(angle + rb.velocity.x);
        transform.Rotate(new Vector3(0f, 0f, angle) * Time.deltaTime);
    }
}
