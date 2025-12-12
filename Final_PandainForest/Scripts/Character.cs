using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    public string characterName;

    [Header("Stats")]
    public float maxHealth = 100f;
    public float currentHealth;
    public Vector2 velocity;
    public CharacterState state;

    [Header("Movement")]
    public float moveSpeed;
    public float jumpForce;
    public Rigidbody2D rb;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
    }

    public virtual void Move(Vector2 direction)
    {
        rb.velocity = new Vector2(direction.x * moveSpeed, rb.velocity.y);

        if (direction.x != 0)
            transform.localScale = new Vector3(Mathf.Sign(direction.x), 1, 1);
    }


    public virtual void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    public virtual void TakeDamage(float amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
            Die();
    }

    public abstract void Attack();

    public virtual void Die()
    {
        Debug.Log($"{characterName} has died.");
    }
}

public enum CharacterState
{
    Idle,
    Moving,
    Jumping,
    Attacking,
    Hurt,
    Dead
}