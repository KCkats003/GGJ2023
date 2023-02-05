using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinalObjective : MonoBehaviour
{
    [SerializeField]
    private Image[] circles;
    
    public CircleGrow circleGrow;
    private CanvasGroup canvasGroup;
    public TreeScript treeScript;

    [SerializeField]
    private float[] resourceValues = new float[5] {200f, 200f, 200f, 300f, 200f};
    private float[] progress = new float[5] {0f, 0f, 0f, 0f, 0f};
    private string[] resourceNames = new string[5] {"Water", "Leaf", "Flower", "Sugar", "Fruit"};

    public bool active = false;

    public void Start(){
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0f;
    }

    public void AddResource(ResourceType rt){
        if (!active){
            return;
        }
        int numDone = 0;
        for (int i=0; i<5; i++){
            if (rt.name == resourceNames[i]){
                progress[i] += (float)rt.value * Time.deltaTime;
                if (progress[i] >= resourceValues[i]){
                    progress[i] = resourceValues[i];
                }

                circles[i].fillAmount = progress[i]/resourceValues[i];
            }
        }
        for (int i=0; i<5; i++){
            if (progress[i] >= resourceValues[i]){
                numDone++;
            }
        }
        if (numDone >= 5){
            // End Game
            circleGrow.active = true;
            treeScript.UpdateSprite(5);
        }

    }

    public void Update(){
        if (active){
            canvasGroup.alpha += Time.deltaTime * 0.3f;
        }
    }


}
