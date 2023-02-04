using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject seed;

    void Start()
    {
        CreateSeed(Vector2.zero);
    }
    public GameObject CreateSeed(Vector2 position)
    {
        GameObject newSeed = Instantiate(seed, position, Quaternion.identity);
        newSeed.transform.parent = transform;
        newSeed.GetComponent<SeedScript>().setSeedSpawner(this);
        return newSeed;
    }
}
