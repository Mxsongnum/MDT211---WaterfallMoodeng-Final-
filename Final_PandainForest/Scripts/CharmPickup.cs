using UnityEngine;

public class CharmPickup : MonoBehaviour
{
    public Charm charmData;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();

            if (player != null && charmData != null)
            {
                charmData.ApplyEffect(player);
                Destroy(gameObject);
            }
        }
    }
}