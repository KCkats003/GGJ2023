using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SeedButton : MonoBehaviour
{
    [SerializeField]
    private SeedGenerator seedGenerator;
    [SerializeField]
    public SeedType seedType;

    private Image fillCircle;
    private Text text;
    private int amount = 0;
    public void AddOne() { amount++; if (text) { text.text = amount.ToString();}}
    private float progress = 0;

    private bool visible = false;

    [SerializeField]
    private float requiredAmount = 10.0f;
    [SerializeField]
    private string requiredResource = "Water";

    void Start(){
        GetComponent<Image>().color = seedType.color;
        fillCircle = transform.GetChild(0).GetComponent<Image>();
        text = transform.GetChild(1).GetComponent<Text>();
        text.text = amount.ToString();
        if (!visible){
            text.enabled = false;
            fillCircle.enabled = false;
            GetComponent<Image>().enabled = false;
        }

    }

    public void Click()
    {
        if (amount > 0)
        {
            seedGenerator.BeginDrag(seedType);
            amount--;
            text.text = amount.ToString();
        }
    }

    public void AddResource(ResourceType rt){
        if (rt.name == requiredResource)
        {
            if (!visible)
            {
                BecomeVisible();
            }
            progress += rt.value * Time.deltaTime;
            fillCircle.fillAmount = progress / requiredAmount;
            if (progress >= requiredAmount)
            {
                amount++;
                progress = 0;
            }

            text.text = amount.ToString();
        }
    }

    public void BecomeVisible()
    {
        visible = true;
        GetComponent<Image>().enabled = true;
        transform.GetChild(0).GetComponent<Image>().enabled = true;
        transform.GetChild(1).GetComponent<Text>().enabled = true;

    }


}
