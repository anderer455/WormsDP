using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField]
    public float forceMultiplier = 1000f;
    [SerializeField]
    public Projectile projectilePrefab;
    [SerializeField]
    private Transform launchingPosition;

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

        Sprite newSprite = Resources.Load<Sprite>("Sprites/Weapons/Explosives/MB bomb");

        if (newSprite != null)
        {
            spriteRenderer.sprite = newSprite;
        }
        else
        {
            Debug.LogError("Failed to load sprite from path: Sprites/Weapons/Explosives/MB bomb");
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

    public void Launch(Vector2 initialPosition, Vector2 currentPosition)
    {
        Vector2 direction = (currentPosition - initialPosition).normalized;
        float distance = Vector2.Distance(initialPosition, currentPosition);
        float force = distance * forceMultiplier;

        GameObject projectile = Instantiate(projectilePrefab, launchingPosition.position, Quaternion.identity).gameObject;
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.AddForce(direction * force);
        }
        else
        {
            Debug.LogError("Projectile prefab must have a Rigidbody component.");
        }
    }
}
