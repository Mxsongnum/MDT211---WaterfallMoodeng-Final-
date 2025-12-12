using UnityEngine;

public class InteractPrompt : MonoBehaviour
{
    [Header("ลาก Game Object ที่เป็นข้อความ Press E มาใส่ตรงนี้")]
    public GameObject promptMessage;

    private bool canInteract = false; // เช็คว่าอยู่ในระยะกดปุ่มไหม

    void Start()
    {
       
        if (promptMessage != null)
            promptMessage.SetActive(false);
    }

    void Update()
    {
        // ถ้าอยู่ในระยะ และ กดปุ่ม E
        if (canInteract && Input.GetKeyDown(KeyCode.E))
        {
            DoAction();
        }
    }

    // ฟังก์ชันที่จะทำงานเมื่อกด E
    void DoAction()
    {
        Debug.Log("กด E แล้ว! ทำงานได้เลย");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            canInteract = true;
            if (promptMessage != null)
                promptMessage.SetActive(true);
        }
    }

    // เมื่อเดินออกห่าง
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            canInteract = false;
            if (promptMessage != null)
                promptMessage.SetActive(false); // ซ่อนข้อความ
        }
    }
}