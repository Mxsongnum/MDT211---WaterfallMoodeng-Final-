using UnityEngine;
using System.Collections;

public class MushroomEnemy : Enemy
{
    [Header("Jump Attack Settings")]
    public float jumpForceX = 5f;
    public float jumpForceY = 8f;

    protected override void Awake()
    {
        base.Awake();
        moveSpeed = 3f;
    }

    public override void Attack()
    {
        if (currentHealth <= 0) return;

        if (Time.time > lastAttackTime + attackCooldown && !isAttacking)
        {
            lastAttackTime = Time.time;
            StartCoroutine(JumpAttackRoutine());
        }
    }

    protected virtual IEnumerator JumpAttackRoutine()
    {
        isAttacking = true;

        if (rb != null) rb.velocity = Vector2.zero;
        FlipTowardsPlayer();

        if (anim != null) anim.SetTrigger("Attack");

        yield return new WaitForSeconds(0.3f);

        if (rb != null && player != null)
        {
            float jumpDirection = (player.position.x > transform.position.x) ? 1 : -1;
            rb.AddForce(new Vector2(jumpDirection * jumpForceX, jumpForceY), ForceMode2D.Impulse);
        }

        yield return new WaitForSeconds(0.2f);
        yield return new WaitUntil(() => IsGrounded());

        if (rb != null) rb.velocity = Vector2.zero;

        OnLand();

        yield return new WaitForSeconds(1f);
        isAttacking = false;
    }

    protected virtual void OnLand()
    {
      
    }

    protected bool IsGrounded()
    {
        return Physics2D.Raycast(transform.position, Vector2.down, 0.2f);
    }

    protected void FlipTowardsPlayer()
    {
        if (player != null)
        {
            Vector3 scale = transform.localScale;
            scale.x = (player.position.x > transform.position.x) ? -Mathf.Abs(scale.x) : Mathf.Abs(scale.x);
            transform.localScale = scale;
        }
    }
}