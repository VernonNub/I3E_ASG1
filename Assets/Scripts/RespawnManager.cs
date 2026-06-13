using UnityEditor.Rendering;
using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    //PlayerManager Script
    private PlayerManager player;

    //Trigger Enter --> assign our playermanager variable to the player's playermanager script
    void OnTriggerEnter(Collider other)
    {
        if(other.name == "Player")
        {
            player = other.gameObject.GetComponent<PlayerManager>();

            SetNewRespawnPoint();
        }
    }

    //changes the playermanager script variable for current checkpoint --> when dies will use this checkpoint transform.position instead.
    private void SetNewRespawnPoint()
    {
        player.currentCheckPoint = this.gameObject;
        //I also want my checkpoint to heal the player!
        player.playerHealth = player.playerMaxHealth;
    }
}
