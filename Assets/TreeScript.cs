using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TreeScript : MonoBehaviour
{
    [SerializeField]
    private Sprite[] images;

    public void UpdateSprite(int index){
        GetComponent<Image>().sprite = images[index];
        Debug.Log("UpdateSprite: " + index);
    }

}
