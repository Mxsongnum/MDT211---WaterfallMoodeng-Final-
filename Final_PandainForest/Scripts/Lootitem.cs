using UnityEngine;

public class Lootitem : MonoBehaviour
{
    public float popForce = 5f;
    public float floatSpeed = 2f;
    public float floatHeight = 0.25f;

    private Rigidbody2D rb;
    private Collider2D col;
    private Vector3 startPos;
    private bool isGrounded = false;
    private float creationTime;
    private Collider2D playerCollider;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
    }

    void Start()
    {
        creationTime = Time.time;

        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            playerCollider = player.GetComponent<Collider2D>();
            if (playerCollider != null && col != null)
            {
                Physics2D.IgnoreCollision(col, playerCollider, true); 
            }
        }

        if (rb != null)
        {
            float randomX = Random.Range(-1f, 1f);
            Vector2 forceDir = new Vector2(randomX, 1f).normalized;
            rb.AddForce(forceDir * popForce, ForceMode2D.Impulse);
        }
    }

    void Update()
    {
        if (isGrounded)
        {
            float newY = startPos.y + Mathf.Sin(Time.time * floatSpeed) * floatHeight;
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (Time.time < creationTime + 0.5f) return;

        if (collision.gameObject.CompareTag("Player") ||
            collision.gameObject.CompareTag("Enemy")) return;

        if (!isGrounded)
        {
            isGrounded = true;
            startPos = transform.position;

            if (rb != null)
            {
                rb.velocity = Vector2.zero;
                rb.isKinematic = true;
                rb.freezeRotation = true;
            }

            if (col != null)
            {
                col.isTrigger = true; 
            }

            if (playerCollider != null && col != null)
            {
                Physics2D.IgnoreCollision(col, playerCollider, false); 
            }
        }
    }
}