using UnityEngine;
using System.Collections;

public class LizardEnemy : Enemy
{
    public Transform tonguePoint;   
    public float tongueRange = 1.5f; 
    public int damageAmount = 10;   
    public float attackAnimDuration = 1.0f; 

    protected override void Awake()
    {
        base.Awake();
        moveSpeed = 2.5f; 
        attackCooldown = 2f; 
    }

    public override void Attack()
    {
        if (currentHealth <= 0) return;

        if (Time.time > lastAttackTime + attackCooldown && !isAttacking)
        {
            lastAttackTime = Time.time;
            StartCoroutine(TongueAttackRoutine());
        }
    }

    protected IEnumerator TongueAttackRoutine()
    {
        isAttacking = true;

        if (rb != null) rb.velocity = Vector2.zero;

 
        if (anim != null) anim.SetTrigger("Attack");
        yield return new WaitForSeconds(attackAnimDuration);

        isAttacking = false;
    }

   
    public void AnimationEvent_TongueHit()
    {
        Vector2 attackPos = tonguePoint != null ? tonguePoint.position : transform.position;

        Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(attackPos, tongueRange);

        foreach (Collider2D playerCollider in hitPlayers)
        {
            if (playerCollider.CompareTag("Player"))
            {
                Player player = playerCollider.GetComponent<Player>();

                if (player != null)
                {
                    player.TakeDamage(damageAmount);
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (tonguePoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(tonguePoint.position, tongueRange);
    }
}