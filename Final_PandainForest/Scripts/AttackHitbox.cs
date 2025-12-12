using UnityEngine;

public class Attackhitbox : MonoBehaviour
{
    public int damage = 10; 

    private void OnTriggerEnter2D(Collider2D collision)
    {
       
        if (collision.CompareTag("Enemy"))
        {
            Enemy enemy = collision.GetComponent<Enemy>();

            if (enemy != null)
            {
                enemy.TakeDamage(damage); 
            }
        }
    }
}