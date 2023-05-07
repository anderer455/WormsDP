using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstAid : MonoBehaviour
{
    public int healAmount = 20;

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
            if (collision.collider.TryGetComponent<WormController>(out wormController)) {
                wormController.Heal(healAmount);
            } else if (collision.collider.TryGetComponent<PlayerController>(out playerController)) {
                playerController.Heal(healAmount);
            }
            
            Destroy(gameObject);
        }
    }
}
