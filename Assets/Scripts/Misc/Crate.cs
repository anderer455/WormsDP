using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using URandom = UnityEngine.Random;

public class Crate : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject == Gameplay.activeWorm) {
            WormController wormController;
            PlayerController playerController;
            
            ProjectileType ammoType = (ProjectileType)URandom.Range(1, 14);
            int ammoAmount = 0;

            if (ammoType == ProjectileType.BULLET) {
                ammoAmount = URandom.Range(2, 5);
            } else if (ammoType == ProjectileType.BUCKSHOT) {
                ammoAmount = URandom.Range(1, 3);
            } else {
                ammoAmount = 1;
            }

            if (collision.collider.TryGetComponent<WormController>(out wormController)) {
                wormController.AddAmmo(ammoAmount, ammoType);
            } else if (collision.collider.TryGetComponent<PlayerController>(out playerController)) {
                playerController.AddAmmo(ammoAmount, ammoType);
            }

            Destroy(gameObject);
        }
    }
}
