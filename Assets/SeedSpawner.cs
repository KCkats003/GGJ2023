using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject seed;
    [SerializeField]
    private GameObject baseSeed;

    [SerializeField]
    private SeedType seedType;

    void Start()
    {
        CreateBaseSeed(Vector2.zero);
    }
    public GameObject CreateSeed(Vector2 position, SeedType seedType)
    {
        GameObject newSeed = Instantiate(seed, position, Quaternion.identity);
        newSeed.transform.parent = transform;
        newSeed.GetComponent<SeedScript>().setSeedSpawner(this);
        newSeed.GetComponent<SeedScript>().setSeedType(seedType);
        return newSeed;
    }
    public GameObject CreateBaseSeed(Vector2 position)
    {
        GameObject newSeed = Instantiate(baseSeed, position, Quaternion.identity);
        newSeed.transform.parent = transform;
        newSeed.GetComponent<SeedScript>().setSeedSpawner(this);
        newSeed.GetComponent<SeedScript>().setSeedType(new SeedType(Color.white, SeedType.Types.Base));
        return newSeed;
    }
}
