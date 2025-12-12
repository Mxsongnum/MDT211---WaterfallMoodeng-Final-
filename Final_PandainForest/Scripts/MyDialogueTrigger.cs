using UnityEngine;
using TMPro; // สำคัญ! ต้องมีบรรทัดนี้เพื่อคุมข้อความ

public class MyDialogueTrigger : MonoBehaviour
{
    [Header("ตั้งค่า UI")]
    public GameObject dialogueBox; // เอากล่องข้อความมาใส่
    public TMP_Text dialogueText;  // เอาตัวหนังสือมาใส่

    [Header("ข้อความที่จะให้พูด")]
    [TextArea] // ทำให้ช่องพิมพ์ข้อความกว้างขึ้น
    public string message = "สวัสดีชาวโลก!";

    // เมื่อเริ่มเกม ซ่อนกล่องข้อความไว้ก่อน (กันพลาด)
    void Start()
    {
        if (dialogueBox != null)
            dialogueBox.SetActive(false);
    }

    // เมื่อเดินเข้า
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // เช็คว่าใช่ผู้เล่นไหม
        {
            dialogueBox.SetActive(true); // เปิดกล่อง
            dialogueText.text = message; // เปลี่ยนข้อความเป็นคำพูดของ NPC ตัวนี้
        }
    }

    // เมื่อเดินออก
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            dialogueBox.SetActive(false); // ปิดกล่อง
        }
    }
}