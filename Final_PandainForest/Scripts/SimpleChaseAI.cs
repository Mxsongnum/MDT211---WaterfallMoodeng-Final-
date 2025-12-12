using UnityEngine;

public class SimpleChasingAI : EnemyAI
{
    public Transform target;
    public float moveSpeed = 2f;
    public float attackRange = 4f;


    private Vector3 originalScale;

    public override void Awake()
    {
        base.Awake();
        originalScale = transform.localScale;

        if (target == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) target = p.transform;
        }
    }

    public override void ProcessAI(Enemy enemy)
    {
        if (target == null) return;

        float distance = Vector2.Distance(transform.position, target.position);

        if (distance <= attackRange)
        {
            if (anim != null) anim.SetBool("isRunning", false);
            enemy.Attack();
        }
        else
        {
            if (anim != null) anim.SetBool("isRunning", true);
            if (target.position.x > transform.position.x)
            {
                transform.localScale = new Vector3(-Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
            }
            else
            {
               
                transform.localScale = new Vector3(Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
            }
            
            Vector3 targetPos = new Vector3(target.position.x, target.position.y, transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
        }
    }
}