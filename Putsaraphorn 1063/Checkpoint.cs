using UnityEngine;

public class CheckpointScript : MonoBehaviour
{
    private RespawnScript playerRespawn;

    void Awake() 
    {
       
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");

        if (playerObj == null)
        {
       
        }
        else
        {
           
            playerRespawn = playerObj.GetComponent<RespawnScript>();
            if (playerRespawn == null)
            {
               
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Player") && playerRespawn != null)
        {
            playerRespawn.SetRespawnPoint(this.transform.position);
        }
    }
}