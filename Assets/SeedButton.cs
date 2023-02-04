using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedButton : MonoBehaviour
{
    [SerializeField]
    private SeedGenerator seedGenerator;
    public void Click()
    {
        seedGenerator.BeginDrag();
    }

}
