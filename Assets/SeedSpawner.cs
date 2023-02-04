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
    private GameObject transportSeed;

    [SerializeField]
    private SeedType seedType;

    public bool hasWater = false;

    void Start()
    {
        GameObject baseSeed = CreateBaseSeed(Vector2.zero);
        //baseSeed.GetComponent<SeedScript>().Sprout(3, new SeedType(SeedType.DefaultColor, SeedType.Types.Default), new Vector2(0, 1));
    }
    public GameObject CreateSeed(Vector2 position, SeedType seedType)
    {
        GameObject newSeed;
        if (seedType.type == SeedType.Types.Transport)
        {
            newSeed = Instantiate(transportSeed, position, Quaternion.identity);
        }
        else
        {
            newSeed = Instantiate(seed, position, Quaternion.identity);
        }
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
