using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedUnlocker : MonoBehaviour
{
    [System.Serializable]
    public class SeedUnlock
    {
        public SeedType seedType;
        public SeedButton seedButton;
        public string requiredResource;
        public float amount;
        


        public SeedUnlock(SeedType seedType, string requiredResource, float amount)
        {
            this.seedType = seedType;
            this.requiredResource = requiredResource;
            this.amount = amount;
        }
    }

    public CameraScript cameraScript;

    [SerializeField]
    private SeedUnlock[] seedUnlocks = {
        new SeedUnlock(new SeedType(SeedType.DefaultColor, SeedType.Types.Default), "Water", 5),
        new SeedUnlock(new SeedType(SeedType.DefaultColor, SeedType.Types.Transport), "Earth", 15),
        new SeedUnlock(new SeedType(SeedType.DefaultColor, SeedType.Types.Synthesis), "Light", 30),
        new SeedUnlock(new SeedType(SeedType.InputColor, SeedType.Types.Input), "Energy", 50)
    };

    private string[] stages = {
        "Water",
        "Leaf",
        "Flower",
        };

    public void CheckAdvanceStage(ResourceType rt)
    {
        if (rt.name == stages[stage])
        {
            NextStage();
        }
    }

    public int stage = 0;
    public SeedUnlock NextStage()
    {
        SeedUnlock output = seedUnlocks[stage];
        cameraScript.UpdateTarget(stage);
        stage++;
        return output;
    }

}
