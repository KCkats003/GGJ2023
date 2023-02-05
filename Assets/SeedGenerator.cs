using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedGenerator : MonoBehaviour
{
    // Used to drag and drop seeds from the sidebar into the game area
    
    private SpriteRenderer spriteRenderer; 
    private bool isDragging = false;
    private SeedType seedType;
    [SerializeField]
    private SeedButton[] seedButtons;
    [SerializeField]
    private SeedUnlocker seedUnlocker;
    
    [SerializeField]
    private FinalObjective finalObjective;

    private Transform previewA;
    private bool hasWater = false;

    void Start()
    {
        spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        previewA = transform.GetChild(1);

        spriteRenderer.enabled = false;
        isDragging = false;
        seedButtons[0].AddOne();
        seedButtons[0].BecomeVisible();
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
            DisplayPreview(transform.position);
            if (Input.GetMouseButtonUp(0))
            {
                Vector3 p = previewA.position;
                previewA.position = new Vector3(1000, 1000, 0);
                isDragging = false;
                // Disable the sprite renderer
                spriteRenderer.enabled = false;
                // Check for collision with a seed
                GameObject seed = GetNearestSeed(transform.position, 4.0f);
                if (seed != null){

                    // prevent softlock
                    if (hasWater == false && GetNearestCrystal(p) == null){
                        seedButtons[0].AddOne();
                        isDragging = false;
                        return;
                    }
                    if (hasWater == false){
                        hasWater = true;
                        GetComponent<AudioSource>().Play();
                    }
                    
                    
                    
                    SeedScript ss = seed.GetComponent<SeedScript>();
                    if (seedType.type == SeedType.Types.Transport && ss.getSeedType().type == SeedType.Types.Transport){
                        ReturnSeedToButton();
                        return;
                    }
                    ss.Sprout(seedType, transform.position);
                    
                } else {
                    ReturnSeedToButton();
                }
            }
        }
    }

    void ReturnSeedToButton(){
        foreach (SeedButton seedButton in seedButtons){
            if (seedButton.seedType == seedType){
                seedButton.AddOne();
            }
        }
    }
    void DisplayPreview(Vector3 mousePosition){
        // Display the preview
        GameObject nearestSeed = GetNearestSeed(mousePosition, 4.0f);
        if (nearestSeed != null){
            SeedScript ss = nearestSeed.GetComponent<SeedScript>();
            Vector3 offset = ss.GetSeedPosition(mousePosition);
            previewA.position = nearestSeed.transform.position + offset;
            GameObject crystal = GetNearestCrystal(previewA.position);
            if (crystal != null){
                Crystal c = crystal.GetComponent<Crystal>();
                previewA.GetComponent<SpriteRenderer>().color = c.resourceType.color;
            } else {
                previewA.GetComponent<SpriteRenderer>().color = Color.white;
            }

        } else {
            previewA.position = new Vector3(1000, 1000, 0);
        }

        
    }

    public static GameObject GetNearestCrystal(Vector2 position, float d = 1.0f){
        Collider2D[] colliders = Physics2D.OverlapCircleAll(position, d);
        GameObject closestCrystal = null;
        float closestDistance = 1000000;

        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject.tag != "Crystal") {continue;}
            if (collider.GetComponent<Crystal>().isFull()){ continue; }
            float distance = Vector3.Distance(position, collider.transform.position);
            if (distance < closestDistance){
                closestCrystal = collider.gameObject;
                closestDistance = distance;
            }
        }
        return closestCrystal;
    }

    public static GameObject GetNearestSeed(Vector2 position, float d = 3.0f, bool includeFull = false){
        Collider2D[] colliders = Physics2D.OverlapCircleAll(position, d);
        GameObject closestSeed = null;
        float closestDistance = 1000000;

        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject.tag != "Seed") {continue;}
            if (!includeFull && collider.GetComponent<SeedScript>().isFull()){ continue; }

            float distance = Vector3.Distance(position, collider.transform.position);
            if (distance < closestDistance){
                closestSeed = collider.gameObject;
                closestDistance = distance;
            }
        }
        return closestSeed;
    }
    


    public void AddResource(ResourceType rt){
        foreach (SeedButton sb in seedButtons){
            sb.AddResource(rt);
        }
        seedUnlocker.CheckAdvanceStage(rt);
        finalObjective.AddResource(rt);
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
