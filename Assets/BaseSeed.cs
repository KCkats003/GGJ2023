using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseSeed : MonoBehaviour
{
    private SeedScript seedScript;
    public string resourceObjective;
    public float requiredAmount;
    public float currentAmount;

    public Image circle;

    public SeedGenerator seedGenerator;

    void Start()
    {
        seedScript = GetComponent<SeedScript>();
        circle = transform.GetChild(1).GetChild(0).GetComponent<Image>();
        seedGenerator = GameObject.Find("SeedGenerator").GetComponent<SeedGenerator>();
    }

    void Update()
    {
        foreach (ResourceType rt in seedScript.getInputResources())
        {
            seedGenerator.AddResource(rt);
            if (rt.name == resourceObjective)
            {
                currentAmount += rt.value * Time.deltaTime;
                circle.fillAmount = currentAmount / requiredAmount;
                if (currentAmount >= requiredAmount)
                {
                    Debug.Log("Objective Complete");
                }
            }
        }
    }

}
