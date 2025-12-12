using System.Diagnostics;
using UnityEngine;

[System.Serializable]
public abstract class Ability : ScriptableObject
{
    public new string name;
    public int cost;
    public float cooldown;

    public abstract void Activate(Character user);
}
