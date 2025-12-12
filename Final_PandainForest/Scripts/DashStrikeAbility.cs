using UnityEngine;

// อย่าลืม! [CreateAssetMenu] ต้องอยู่บนสุด
[CreateAssetMenu(menuName = "Abilities/Dash Strike", fileName = "New Dash Strike")]
public class DashStrikeAbility : Ability
{
    [Header("Dash Settings")]
    public float dashSpeed = 20f;
    public float dashDuration = 0.2f;
    public float damage = 10f; // ✅ 1. กำหนดความแรงตรงนี้ (เช่น 10)
    public LayerMask enemyLayer;

    [Header("Visual Effects")]
    public GameObject dashVFXPrefab;
    public Vector3 vfxOffset = Vector3.zero;

    public override void Activate(Character user)
    {
        Rigidbody2D rb = user.GetComponent<Rigidbody2D>();
        if (rb == null) return;

        // แสดงเอฟเฟกต์ (ถ้ามี)
        if (dashVFXPrefab != null)
        {
            GameObject vfx = Instantiate(dashVFXPrefab, user.transform.position + vfxOffset, Quaternion.identity);
            Destroy(vfx, 1f);
        }

        user.StartCoroutine(DashCoroutine(user, rb));
    }

    private System.Collections.IEnumerator DashCoroutine(Character user, Rigidbody2D rb)
    {
        float timeLeft = dashDuration;
        float direction = user.transform.localScale.x > 0 ? 1 : -1;

        // ปิดแรงโน้มถ่วงชั่วคราว
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0;

        // สร้าง List เพื่อจำว่าชนตัวไหนไปแล้วบ้าง (กันดาเมจเด้งรัวๆ ใส่ตัวเดิมใน 1 ครั้งที่พุ่ง)
        System.Collections.Generic.List<GameObject> hitEnemies = new System.Collections.Generic.List<GameObject>();

        while (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;

            // สั่งพุ่ง
            rb.velocity = new Vector2(direction * dashSpeed, 0);

            // ✅ 2. สร้างวงกลมตรวจสอบการชน (Hitbox) ระหว่างพุ่ง
            RaycastHit2D[] hits = Physics2D.CircleCastAll(
                user.transform.position,
                0.5f, // รัศมีวงกลม
                Vector2.right * direction,
                0.1f, // ระยะทางเช็ค
                enemyLayer
            );

            // ✅ 3. วนลูปเช็คศัตรูที่โดนชน
            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider != null)
                {
                    // ถ้าศัตรูตัวนี้ยังไม่เคยโดนชนในรอบนี้
                    if (!hitEnemies.Contains(hit.collider.gameObject))
                    {
                        Enemy enemy = hit.collider.GetComponent<Enemy>();
                        if (enemy != null)
                        {
                            enemy.TakeDamage(damage); // ลดเลือด 10
                            hitEnemies.Add(hit.collider.gameObject); // จำไว้ว่าตัวนี้โดนแล้ว

                            // (Optional) ใส่ Effect เลือดสาด หรือเสียงชนตรงนี้เพิ่มได้
                            Debug.Log("พุ่งชน " + enemy.name + " ดาเมจ " + damage);
                        }
                    }
                }
            }

            yield return null;
        }

        // คืนค่าเดิม
        rb.gravityScale = originalGravity;
        rb.velocity = Vector2.zero;
    }
}