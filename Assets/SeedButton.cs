using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedButton : MonoBehaviour
{
    [SerializeField]
    private SeedGenerator seedGenerator;
    [SerializeField]
    private SeedType seedType;

    public void Click()
    {
        seedGenerator.BeginDrag(seedType);
    }

}
