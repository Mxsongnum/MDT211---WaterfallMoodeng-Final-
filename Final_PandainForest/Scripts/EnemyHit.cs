using UnityEngine;

public class EnemyHitbox : MonoBehaviour
{
    public int damage = 10; 

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>(); 

            if (player != null)
            {
                player.TakeDamage(damage);
              
            }
        }
    }
}