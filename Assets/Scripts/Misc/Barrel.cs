using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrel : MonoBehaviour
{
    [SerializeField] private GameObject explosionPrefab;
    private bool isDestroyed = false;

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.TryGetComponent<Projectile>(out Projectile projectile)) {
            DestroyBarrel();
        }
    }

    private void DestroyBarrel() {
        if (!isDestroyed) {
            isDestroyed = true;
            Instantiate(explosionPrefab, transform.position + new Vector3(0, -0.35f, 0), Quaternion.identity);
            Destroy(gameObject);
        }
    }
}