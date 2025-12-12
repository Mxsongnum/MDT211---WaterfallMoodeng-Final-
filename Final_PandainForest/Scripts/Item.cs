using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item
{
    public string Name;
    public string Description;

    public virtual void Use(Character character)
    {
        Debug.Log($"Used item: {Name}");
    }
}
