using UnityEngine;

public class SimpleEnemyAI : EnemyAI
{
    public float attackDistance = 1.5f;

    public override void ProcessAI(Enemy enemy)
    {
        if (enemy == null) return;

     
        if (enemy.player == null) return;

        float dist = Vector2.Distance(enemy.transform.position, enemy.player.position);

        if (dist > attackDistance)
        {
            MoveTowardsPlayer(enemy);
        }
        else
        {
           
            enemy.rb.velocity = Vector2.zero;

           
            if (Time.time > enemy.lastAttackTime + enemy.attackCooldown)
            {
               
                enemy.StartAttack();

         
                enemy.lastAttackTime = Time.time;
            }
        }
    }

    // ???????????????? (???????????????????)
    private void MoveTowardsPlayer(Enemy enemy)
    {
        Vector2 direction = (enemy.player.position - enemy.transform.position).normalized;
        enemy.rb.velocity = new Vector2(direction.x * enemy.moveSpeed, enemy.rb.velocity.y);

        // ????????????????
        if (direction.x > 0) enemy.transform.localScale = new Vector3(1, 1, 1);
        else if (direction.x < 0) enemy.transform.localScale = new Vector3(-1, 1, 1);
    }
}