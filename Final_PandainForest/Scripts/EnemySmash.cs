using UnityEngine;
using System.Collections;

// สืบทอดจาก Enemy (ตัวที่คุณส่งมา) ทำให้ได้ระบบ AI, Loot, HP มาฟรีๆ
public class EnemySmash : Enemy
{
    [Header("Smash Settings")]
    public float smashWindupTime = 0.5f; // เวลาง้าง (Animation: AttackSmashStart)
    public float smashLandTime = 0.5f;   // เวลาทุบ (Animation: AttackSmashLand)

    // Override ฟังก์ชัน Awake เพื่อตั้งค่าเฉพาะตัว
    protected override void Awake()
    {
        base.Awake(); // เรียกคำสั่งพื้นฐานจากตัวแม่ (หา Player, ตั้งค่า HP)

        characterName = "Smash Panda"; // ตั้งชื่อใหม่
        moveSpeed = 3f; // ปรับความเร็วให้เหมาะกับท่าบิน (FlyQuick)
    }

    // Override ฟังก์ชัน Attack เพื่อเปลี่ยนท่าตีธรรมดา เป็นท่า Smash 3 จังหวะ
    public override void Attack()
    {
        // สั่งหยุดการทำงานทับซ้อน
        StopCoroutine("SmashAttackRoutine");
        StartCoroutine(SmashAttackRoutine());
    }

    // Coroutine จัดการท่าโจมตี 3 สเต็ป
    IEnumerator SmashAttackRoutine()
    {
        isAttacking = true;

        // 1. เริ่มง้าง (Start) -> เชื่อมกับ Anim: AttackSmashStart
        if (anim != null) anim.SetTrigger("AttackStart");

        // หยุดเดินชั่วคราวตอนง้าง
        if (rb != null) rb.velocity = Vector2.zero;

        yield return new WaitForSeconds(smashWindupTime); // รอนานเท่าไหร่ตอนง้าง

        // 2. ทุบลงพื้น (Land) -> เชื่อมกับ Anim: AttackSmashLand
        if (anim != null) anim.SetTrigger("AttackLand");

        // เปิด Hitbox สร้างความเสียหายตอนทุบ
        if (attackHitboxObj != null) attackHitboxObj.SetActive(true);
        yield return new WaitForSeconds(0.2f); // เปิด hitbox แค่ 0.2 วิ
        if (attackHitboxObj != null) attackHitboxObj.SetActive(false);

        // รอจนจบท่าทุบ
        yield return new WaitForSeconds(smashLandTime - 0.2f);

        isAttacking = false;
        // AI จะสั่งให้เดินต่อเองหลังจากนี้
    }
}