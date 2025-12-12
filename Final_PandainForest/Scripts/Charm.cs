using UnityEngine;

[System.Serializable]

[CreateAssetMenu(fileName = "New Charm", menuName = "Charms/Charm Item")]
public class Charm : ScriptableObject
{
    public string charmName;
    public Sprite icon;       
    public int cost;          
    public CharmEffect effect; 

    public void ApplyEffect(Player player)
    {
        if (effect != null)
        {
            effect.Apply(player);
        }
    }
}

public abstract class CharmEffect : ScriptableObject
{
    public abstract void Apply(Player player);
}
