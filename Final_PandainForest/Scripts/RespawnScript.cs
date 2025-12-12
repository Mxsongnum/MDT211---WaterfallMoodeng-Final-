using UnityEngine;
using System.Collections.Generic;

public class RespawnScript : MonoBehaviour
{
    public static RespawnScript instance;

    public GameObject player;
    public Transform startPoint;     
    private Vector3 savedRespawnPos;

    [System.Serializable]
    public class DeadEnemyData { public GameObject enemyObj; public Vector3 originalPos; }
    public List<DeadEnemyData> deadEnemiesList = new List<DeadEnemyData>();

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        if (player == null) player = this.gameObject;
    }

    private void Start()
    {
       
        if (startPoint != null)
        {
            savedRespawnPos = startPoint.position;
            player.transform.position = startPoint.position;
        }
        else
        {
            GameObject foundStart = GameObject.Find("RespawnPoint");
            if (foundStart != null)
            {
                savedRespawnPos = foundStart.transform.position;
                player.transform.position = foundStart.transform.position;
            }
            else
            {
                savedRespawnPos = player.transform.position;
            }
        }
    }

    public void SetRespawnPoint(Vector3 newPos)
    {
        savedRespawnPos = newPos;
    }

 
    public void RespawnPlayer()
    {
        if (player != null)
        {  
            player.transform.position = savedRespawnPos;

            Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
            if (rb != null) rb.velocity = Vector2.zero;
        }
    }

    public void RegisterDeadEnemy(GameObject enemy, Vector3 originalPosition)
    {
        if (enemy != null)
        {
            enemy.SetActive(false);
            deadEnemiesList.Add(new DeadEnemyData { enemyObj = enemy, originalPos = originalPosition });
        }
    }

    private void RespawnAllEnemies()
    {
        foreach (var data in deadEnemiesList)
        {
            if (data.enemyObj != null)
            {
                data.enemyObj.transform.position = data.originalPos;
                data.enemyObj.SetActive(true);
            }
        }
        deadEnemiesList.Clear();
    }
}