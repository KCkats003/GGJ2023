using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedGenerator : MonoBehaviour
{
    // Used to drag and drop seeds from the sidebar into the game area
    
    private SpriteRenderer spriteRenderer; 
    private bool isDragging = false;

    void Start()
    {
        spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = false;
        isDragging = false;
    }
    public void BeginDrag()
    {
        // Creates new attached seeds
        spriteRenderer.enabled = true;
        // Update the position to the mouse position
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(mousePosition.x, mousePosition.y, 0);
        isDragging = true;
        Debug.Log("Begin Drag");
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
                    if (collider.gameObject.tag == "Seed")
                    {
                        // If there is a seed, attach the seed to the generator
                        collider.GetComponent<SeedScript>().Sprout(4);
                    }
                }
                isDragging = false;

            }
        }
    }
    

}
