using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedGenerator : MonoBehaviour
{
    // Used to drag and drop seeds from the sidebar into the game area
    
    private SpriteRenderer spriteRenderer; 
    private bool isDragging = false;
    private SeedType seedType;

    void Start()
    {
        spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = false;
        isDragging = false;
    }
    public void BeginDrag(SeedType seedType)
    {
        this.seedType = seedType;
        // Creates new attached seeds
        spriteRenderer.enabled = true;
        spriteRenderer.color = seedType.color;
        // Update the position to the mouse position
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(mousePosition.x, mousePosition.y, 0);
        isDragging = true;
    }

    void Update()
    {
        if (isDragging){
            // Update the position to the mouse position
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(mousePosition.x, mousePosition.y, 0);



            if (Input.GetMouseButtonUp(0))
            {
                // Disable the sprite renderer
                spriteRenderer.enabled = false;
                // Check for collision with a seed
                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.5f);
                foreach (Collider2D collider in colliders)
                {
                    if (collider.gameObject.tag != "Seed") {continue;}
                    SeedScript ss = collider.GetComponent<SeedScript>();
                    if (ss.isFull()){ continue; }
                    if (seedType.type == SeedType.Types.Default){
                        ss.Sprout(4, seedType);
                    }
                    else if (seedType.type == SeedType.Types.Input){
                        ss.Sprout(1, seedType);

                    }
                    

                    break;
                }
                isDragging = false;

            }
        }
    }

    public static GameObject GetNearestCrystal(Vector2 position, float d = 3.0f){
        Collider2D[] colliders = Physics2D.OverlapCircleAll(position, d);
        GameObject closestCrystal = null;
        float closestDistance = 1000000;

        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject.tag != "Crystal") {continue;}
            float distance = Vector3.Distance(position, collider.transform.position);
            if (distance < closestDistance){
                closestCrystal = collider.gameObject;
                closestDistance = distance;
            }
        }
        return closestCrystal;
    }
    
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        GameObject nearestCrystal = GetNearestCrystal(transform.position);
        if (nearestCrystal != null){
            Gizmos.DrawLine(transform.position, nearestCrystal.transform.position);
        }
    }

}
