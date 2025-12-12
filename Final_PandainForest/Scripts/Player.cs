using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class Player : Character
{
    public HealthBar healthBar;
    public GameObject deathUIPanel;
    public string mainMenuSceneName = "MainMenu"; 
    private bool isDead = false;
     public int silk = 10;
    public int damage = 10;
    private bool isInvincible = false;
    private SpriteRenderer sr;
    public GameObject attackHitbox;
    public float comboWindow = 0.8f;
    private int comboStep = 0;
    private float lastAttackTime;
    public LayerMask Ground;
    public Vector2 groundCheckSize = new Vector2(0.5f, 0.2f);
    public float groundCheckOffset = 0f;
    [Range(0, 1)] public float jumpCutMultiplier = 0.5f;
    private float jumpCooldown = 0.2f;
    private float lastJumpTime;
    public int extraJumpsValue = 1;
    private int extraJumps;
    public float doubleJumpForce = 16f;

 
    public float interactRange = 2f;
    public LayerMask interactLayer;

    public List<Ability> abilities = new();

    private Animator anim;
    private bool isGrounded;

    protected override void Awake()
    {
        base.Awake();
        characterName = "Panda";
        moveSpeed = 5f;
        jumpForce = 12f;
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        currentHealth = maxHealth;
        if (deathUIPanel != null) deathUIPanel.SetActive(false);
        if (attackHitbox != null) attackHitbox.SetActive(false);

        DashStrikeAbility dashAbility = ScriptableObject.CreateInstance<DashStrikeAbility>();
        {
            dashAbility.name = "Dash Strike";
            dashAbility.cost = 1;
            dashAbility.cooldown = 0.2f;
            dashAbility.dashSpeed = 25f;
            dashAbility.damage = 25f;
            dashAbility.enemyLayer = LayerMask.GetMask("Enemy");

            abilities.Add(dashAbility);
        }
    }

    private void Update()
    {
      
        if (isDead) return;

       
        Vector2 boxCenter = (Vector2)transform.position + Vector2.down * groundCheckOffset;
        isGrounded = Physics2D.OverlapBox(boxCenter, groundCheckSize, 0f, Ground) != null;

   
        if (Time.time - lastAttackTime > comboWindow) { comboStep = 0; }

        if (anim != null)
        {
            anim.SetBool("isGrounded", isGrounded);
            anim.SetFloat("yVelocity", rb.velocity.y);
            anim.SetBool("isRunning", Mathf.Abs(rb.velocity.x) > 0.1f);
        }

        HandleInput();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector2 boxCenter = (Vector2)transform.position + Vector2.down * groundCheckOffset;
        Gizmos.DrawWireCube(boxCenter, groundCheckSize);
    }

    private void HandleInput()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(moveX * moveSpeed, rb.velocity.y);

        if (moveX != 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x) * Mathf.Sign(moveX);
            transform.localScale = scale;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGrounded && Time.time - lastJumpTime > jumpCooldown)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                lastJumpTime = Time.time;
            }
            else if (extraJumps > 0 && !isGrounded)
            {
                rb.velocity = new Vector2(rb.velocity.x, doubleJumpForce);
                extraJumps--;
            }
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            if (rb.velocity.y > 0)
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * jumpCutMultiplier);
        }

        if (isGrounded) extraJumps = extraJumpsValue;

        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Q)) Attack();

        if (Input.GetKeyDown(KeyCode.F))
        {
            UseAbility("Dash Strike");
            if (anim != null) anim.SetTrigger("Roll");
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            CheckInteraction();
        }
    }

    public override void Attack()
    {
        lastAttackTime = Time.time;
        comboStep++;
        if (comboStep == 1) { if (anim != null) anim.SetTrigger("Attack1"); }
        else if (comboStep >= 2) { if (anim != null) anim.SetTrigger("Attack2"); comboStep = 0; }
        StopCoroutine(AttackRoutine());
        StartCoroutine(AttackRoutine());
    }

    private IEnumerator AttackRoutine()
    {
        yield return new WaitForSeconds(0.1f);
        if (attackHitbox != null) attackHitbox.SetActive(true);
        yield return new WaitForSeconds(0.15f);
        if (attackHitbox != null) attackHitbox.SetActive(false);
    }

    public override void TakeDamage(float damageAmount)
    {
        if (isDead || isInvincible) return;

        currentHealth -= damageAmount;

        if (healthBar != null)
        {
            healthBar.currentHealth = currentHealth;
            healthBar.UpdateBar();
        }

        StartCoroutine(InvincibilityRoutine());

        if (currentHealth <= 0) Die();
    }

    IEnumerator InvincibilityRoutine()
    {
        isInvincible = true;
        for (int i = 0; i < 2; i++)
        {
            if (sr != null) sr.color = new Color(1f, 1f, 1f, 0f);
            yield return new WaitForSeconds(0.1f);
            if (sr != null) sr.color = Color.white;
            yield return new WaitForSeconds(0.1f);
        }
        if (sr != null) sr.color = Color.white;
        isInvincible = false;
    }

    public void Heal() { if (silk > 0) { currentHealth = Mathf.Min(maxHealth, currentHealth + 20); silk--; } }

    public void UseAbility(string n)
    {
        Ability a = abilities.Find(x => x != null && x.name == n);
        if (a != null) a.Activate(this);
    }

    public void CheckInteraction()
    {
        Collider2D[] hitObjects = Physics2D.OverlapCircleAll(transform.position, interactRange, interactLayer);
        foreach (Collider2D obj in hitObjects)
        {
            IInteractable interactable = obj.GetComponent<IInteractable>();
            if (interactable != null) { interactable.Interact(this); return; }
        }
    }

  

    void ResetPlayerStatus()
    {
        isInvincible = false;
        if (sr != null) sr.color = Color.white;
        StopAllCoroutines();

        this.enabled = true;
        if (rb != null) rb.bodyType = RigidbodyType2D.Dynamic;
    }

   
    public override void Die()
    {
        if (isDead) return;
        isDead = true;

        if (anim != null) anim.SetTrigger("Die");

        if (rb != null)
        {
            rb.velocity = Vector2.zero; 
            rb.bodyType = RigidbodyType2D.Dynamic; 
        }
        this.enabled = false;

        StartCoroutine(ShowDeathScreenRoutine());
    }

    IEnumerator ShowDeathScreenRoutine()
    {
      
        yield return new WaitForSeconds(2f);

        if (deathUIPanel != null)
        {
            deathUIPanel.SetActive(true);

            Time.timeScale = 0f;
        }
    }


    public void RestartGame()
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); 
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuSceneName); 
    }

    
    public void Respawn()
    {
        if (deathUIPanel != null) deathUIPanel.SetActive(false);
        Time.timeScale = 1f;
        isDead = false;
        currentHealth = maxHealth;
        ResetPlayerStatus();
        if (RespawnScript.instance != null) RespawnScript.instance.RespawnPlayer();
        else SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void RestoreHealth(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth) currentHealth = maxHealth;
        if (healthBar != null)
        {
            healthBar.Heal(amount); 
        }
    }
}