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

    public AudioSource[] songs;

    public CameraScript cameraScript;

    [SerializeField]
    private TreeScript treeScript;

    [SerializeField]
    private FinalObjective finalObjective;

    [SerializeField]
    private SeedUnlock[] seedUnlocks = {
        new SeedUnlock(new SeedType(SeedType.DefaultColor, SeedType.Types.Default), "Water", 5),
        new SeedUnlock(new SeedType(SeedType.DefaultColor, SeedType.Types.Transport), "Earth", 15),
        new SeedUnlock(new SeedType(SeedType.DefaultColor, SeedType.Types.Synthesis), "Light", 30),
        new SeedUnlock(new SeedType(SeedType.InputColor, SeedType.Types.Input), "Energy", 50)
    };
    public int stage = 0;

    private string[] stages = {
        "Water",
        "Leaf",
        "Flower",
        "Sugar",
        "None"
        };

    void Start(){
        treeScript.UpdateSprite(stage);
    }
    public void CheckAdvanceStage(ResourceType rt)
    {
        if (rt.name == stages[stage])
        {
            NextStage();
        }
    }

    void Update(){
        for (int i=1;i<=stage; i++){
            if (i>=5) break;
            if (songs[i].volume < 1f){
                songs[i].volume += Time.deltaTime * 0.1f;
            }
        }
    }
    public SeedUnlock NextStage()
    {
        SeedUnlock output = seedUnlocks[stage];
        cameraScript.UpdateTarget(stage);
        stage++;
        treeScript.UpdateSprite(stage);

        if (stage==4){
            finalObjective.active = true;
        }

        return output;
    }

}
