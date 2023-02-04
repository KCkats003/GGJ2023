using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SeedType
{
    public enum Types {Default, Input, Base, Transport, Synthesis};
    public static Color DefaultColor = new Color(.65f,.65f,.65f);
    public static Color InputColor = new Color(.97f,.61f,.17f);


    [SerializeField]
    public Color color;
    public Types type = Types.Default;

    public SeedType(Color color, Types type)
    {
        this.color = color;
        this.type = type;
    }
    public SeedType(Color color)
    {
        this.color = color;
        this.type = Types.Default;
    }

}