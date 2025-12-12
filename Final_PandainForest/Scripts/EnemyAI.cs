using UnityEngine;

public abstract class EnemyAI : MonoBehaviour
{
    // 1. เพิ่มตัวแปร Animator ให้ลูกหลานเรียกใช้ได้
    public Animator anim;

    public abstract void ProcessAI(Enemy enemy);
    public int hp = 100;

    // หา Animator อัตโนมัติเมื่อเริ่มเกม
    public virtual void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void TakeDamage(int damage)
    {
        hp -= damage;
        // (ถ้ามีท่าโดนตี ใส่ anim.SetTrigger("Hit"); ตรงนี้ได้)

        if (hp <= 0)
        {
            Destroy(gameObject);
        }
    }

    // 2. เพิ่มระบบชนผู้เล่น -> สั่ง RespawnScript ให้ทำงาน
    // แก้เป็น: ชนแล้วให้ลดเลือดผู้เล่น (ไม่วาร์ป)
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // 1. หาตัวผู้เล่น
            Player player = collision.gameObject.GetComponent<Player>();

            // 2. ถ้าเจอ ให้สั่งลดเลือด (สมมติลด 10)
            if (player != null)
            {
                player.TakeDamage(5); // เลขนี้คือดาเมจ ปรับได้
            }
        }
    }
}