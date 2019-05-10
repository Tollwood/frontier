using System;
using UnityEngine;

[System.Serializable]
public class MenuAction
{
    public Color color;
    public Sprite sprite;
    public string hint;

    internal Action interaction;
}
