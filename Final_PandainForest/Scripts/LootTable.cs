using System;
using UnityEngine;

[Serializable]
public class LootItem
{
    public GameObject itemPrefab;
    [Range(0f, 1f)]
    public float dropChance = 0.5f;
}

public class LootTable : MonoBehaviour
{
    public LootItem[] lootItems;

    public void DropLoot(Vector3 position)
    {
        foreach (var loot in lootItems)
        {
            if (UnityEngine.Random.value <= loot.dropChance)
            {
                Instantiate(loot.itemPrefab, position, Quaternion.identity);
            }
        }
    }
}