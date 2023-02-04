using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal : MonoBehaviour
{
    public ResourceType resourceType;
    private int numTappers = 0;
    private int maxTappers = 3;
    public void AddTapper()
    {
        numTappers++;
        if (numTappers >= maxTappers)
        {
            full = true;
        }
    }

    private bool full = false;
    public bool isFull() { return full; }
    
}
