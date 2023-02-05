using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CircleGrow : MonoBehaviour
{
    public Text text;
    public bool active = true;
    private float speed = 0f;
    private float textPercent = 0f;

    private string fullString = "       Thanks For Playing!       \n       Programming: Thomas Horne      \n       Art: Katie Chen, Emily Chen       \n       Music: Connor Grail      ";
    void Update(){
        if (active){
            text.enabled = true;
            speed += Time.deltaTime;
            transform.localScale += new Vector3(1f, 1f, 1f) * Time.deltaTime * speed;
            if (transform.localScale.x > 16f){
                textPercent += Time.deltaTime * 0.15f;
            }
            if (textPercent > 1f){
                textPercent = 1f;
            }
            text.text = fullString.Substring(0, (int)(fullString.Length * textPercent));

        }
    }
}
