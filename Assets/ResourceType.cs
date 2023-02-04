using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ResourceType
{
    public Color color;
    public string name;
    public int value;
    public ResourceType(Color color, string name, int value)
    {
        this.color = color;
        this.name = name;
        this.value = value;
    }
    public override string ToString()
    {
        return name + ": " + value;
    }
    public ResourceType Clone()
    {
        return new ResourceType(color, name, value);
    }

}
