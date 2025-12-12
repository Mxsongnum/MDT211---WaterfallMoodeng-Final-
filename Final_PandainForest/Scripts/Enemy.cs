using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy : Character
{
    [Header("AI & Target")]
    public EnemyAI ai;
    public Transform player;

    [Header("Combat Settings")]
    public float chaseRange = 5f;
    public float attackRange = 1.5f;
    public float attackCooldown = 3f;
    public int damage = 10;

    public float lastAttackTime;
    public bool isAttacking = false;

    [Header("Hitbox Settings")]
    public GameObject attackHitboxObj;

    [Header("Loot Drop System (ปรับความเยอะตรงนี้)")]
    public List<GameObject> possibleDrops; // ลาก Prefab ไอเทมมาใส่ (เช่น Coin, Silk)
    [Range(0, 100)] public float dropChance = 80f; // โอกาสดรอป (ปรับให้เยอะขึ้นเป็น 80%)
    public int minDrops = 3;  // ดรอปขั้นต่ำ 3 ชิ้น
    public int maxDrops = 6;  // ดรอปสูงสุด 6 ชิ้น
    public float dropForce = 6f; // แรงกระเด็นของไอเทม

    protected Animator anim;
    protected SpriteRenderer sr;
    protected Collider2D col;

    protected override void Awake()
    {
        base.Awake();
        characterName = "Enemy";
        moveSpeed = 2f;

        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();

        if (player == null)
        {
            GameObject p = GameObject.FindWithTag("Player");
            if (p != null) player = p.transform;
        }

        if (attackHitboxObj != null)
        {
            attackHitboxObj.SetActive(false);
            EnemyHitbox hitboxScript = attackHitboxObj.GetComponent<EnemyHitbox>();
            if (hitboxScript != null) hitboxScript.damage = this.damage;
        }
    }

    protected virtual void Update()
    {
        if (currentHealth <= 0 || isAttacking) return;

        if (ai != null && ai.enabled)
        {
            ai.ProcessAI(this);
        }
        else
        {
            ProcessBasicAI();
        }
    }

    protected virtual void ProcessBasicAI()
    {
        if (player == null) return;
        float distance = Vector2.Distance(transform.position, player.position);

        if (distance < chaseRange)
        {
            if (distance > attackRange)
            {
                MoveTowardsPlayer();
            }
            else
            {
                if (anim != null) anim.SetBool("IsWalking", false);
                if (Time.time > lastAttackTime + attackCooldown)
                {
                    lastAttackTime = Time.time;
                    StartAttack();
                }
            }
        }
        else
        {
            if (anim != null) anim.SetBool("IsWalking", false);
        }
    }

    void MoveTowardsPlayer()
    {
        if (isAttacking) return;
        Vector2 direction = (player.position - transform.position).normalized;
        transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);

        if (sr != null)
        {
            if (direction.x > 0) sr.flipX = false;
            else if (direction.x < 0) sr.flipX = true;
        }
        if (anim != null) anim.SetBool("IsWalking", true);
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        if (currentHealth > 0 && anim != null) anim.SetTrigger("Hit");
    }

    public override void Die()
    {
        base.Die();
        if (anim != null) anim.SetTrigger("Die");

        // เรียกฟังก์ชันดรอปของแบบใหม่
        DropLootExplosion();

        StopAllCoroutines();
        this.enabled = false;
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.isKinematic = true;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
        if (col != null) col.enabled = false;

        if (attackHitboxObj != null) attackHitboxObj.SetActive(false);

        gameObject.tag = "Untagged";
        Destroy(gameObject, 5f);
    }

    public override void Attack()
    {
        StartCoroutine(AttackRoutine());
    }

    protected IEnumerator AttackRoutine()
    {
        isAttacking = true;
        if (anim != null) anim.SetTrigger("Attack");

        yield return new WaitForSeconds(0.4f);

        if (attackHitboxObj != null) attackHitboxObj.SetActive(true);

        yield return new WaitForSeconds(0.2f);

        if (attackHitboxObj != null) attackHitboxObj.SetActive(false);

        yield return new WaitForSeconds(0.4f);
        isAttacking = false;
    }

    public void StartAttack() { Attack(); }

    // ✅ ฟังก์ชันใหม่: ดรอปของแบบระเบิดกระจาย (Loot Explosion)
    void DropLootExplosion()
    {
        // 1. เช็คโอกาสดรอป
        float randomVal = Random.Range(0f, 100f);
        if (randomVal > dropChance) return;

        if (possibleDrops == null || possibleDrops.Count == 0) return;

        // 2. สุ่มจำนวนชิ้นที่จะดรอป (Min - Max)
        int countToDrop = Random.Range(minDrops, maxDrops + 1);

        for (int i = 0; i < countToDrop; i++)
        {
            // 3. สุ่มไอเทมจากลิสต์ (ดรอปซ้ำได้)
            int randomIndex = Random.Range(0, possibleDrops.Count);
            GameObject itemToSpawn = possibleDrops[randomIndex];

            // 4. สร้างไอเทม
            GameObject spawnedItem = Instantiate(itemToSpawn, transform.position, Quaternion.identity);

            // 5. ใส่แรงกระเด็น (ต้องมี Rigidbody2D ที่ไอเทมนะ)
            Rigidbody2D itemRb = spawnedItem.GetComponent<Rigidbody2D>();
            if (itemRb != null)
            {
                // สุ่มทิศทางซ้ายขวา + เด้งขึ้นฟ้าเสมอ
                float randomX = Random.Range(-1f, 1f);
                float randomY = Random.Range(0.5f, 1.2f); // เน้นเด้งขึ้น

                Vector2 forceDir = new Vector2(randomX, randomY).normalized;

                // ดีดออกไป!
                itemRb.AddForce(forceDir * dropForce, ForceMode2D.Impulse);
            }
        }
    }
}