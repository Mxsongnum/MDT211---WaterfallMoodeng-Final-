using UnityEngine;

[CreateAssetMenu(menuName = "Charms/Effects/Heal Effect")]
public class HealEffect : CharmEffect 
{
    public int amount = 5; 

    public override void Apply(Player player)
    {
        player.RestoreHealth(amount);
    }
}