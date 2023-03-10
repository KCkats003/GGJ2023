using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransportResources : MonoBehaviour
{
    // Additional script for transport seed

    public SeedScript seedScript;
    public SeedScript inputSeed;

    private LineRenderer lineRenderer;
    private bool isDragging = false;

    void Start(){
        lineRenderer = GetComponentInChildren<LineRenderer>();
    }

    void OnMouseDown()
    {
        print("Mouse Down on traqnsporty");
        isDragging = true;
        if (inputSeed != null){
            inputSeed.AddOutputSeed(inputSeed.getParentSeed());
            inputSeed = null;
        }
    }

    void Update(){
        if (isDragging){
            Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
            Vector3 objPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            lineRenderer.SetPosition(0, (objPosition-transform.position)*2f);

            if (Input.GetMouseButtonUp(0)){
                isDragging = false;
                // Check for collision with a seed
                GameObject seed = SeedGenerator.GetNearestSeed(objPosition, 4.0f, true);
                if (seed != null && seed != gameObject){
                    if (CheckIfInOutputChain(seed.GetComponent<SeedScript>())){
                        return;
                    }
                    seed.GetComponent<SeedScript>().AddOutputSeed(seedScript);
                    inputSeed = seed.GetComponent<SeedScript>();
                }
            }
        } else if (inputSeed != null){
            lineRenderer.SetPosition(0, (inputSeed.transform.position-transform.position)*2f);
        } else {
            lineRenderer.SetPosition(0, Vector3.zero);
        }
    }

    bool CheckIfInOutputChain(SeedScript seed){
        // attempting to set the output to a seed that this will flow into will cause stack overflow
        // this function checks for that and prevents it
        SeedScript currentSeed = seedScript;
        while (currentSeed != null){
            if (currentSeed == seed){
                return true;
            }
            currentSeed = currentSeed.getOutputSeed();
        }
        return false;
    }

    

}
