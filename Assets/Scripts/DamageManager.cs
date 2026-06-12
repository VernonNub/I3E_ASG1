using System;
using UnityEngine;

public class DamageManager : MonoBehaviour
{
    public enum DamageType
    {
        damage,
        DoT,
        MaxHealthDecrease,
        MaxHealthIncrease,
        Heal
    }
    public DamageType damage;
    public float damageDone;
    private bool HealUsed = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnTriggerStay(Collider other)
    {
        if(other.name == "Player")
        {
            DamagePlayerOverTime(other.gameObject.GetComponent<PlayerManager>());
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.name == "Player")
        {
            DamagePlayer(other.gameObject.GetComponent<PlayerManager>());
        }
    }

    private void DamagePlayerOverTime(PlayerManager player)
    {
        switch(damage)
        {
            case DamageType.DoT:
                player.UpdateHealth(-damageDone * Time.deltaTime, 0);
                break;
        }
    }

    private void DamagePlayer(PlayerManager player)
    {
        switch(damage)
        {
            case DamageType.damage:
                player.UpdateHealth(-damageDone, 0);
                break;

            case DamageType.MaxHealthDecrease:
                if(!HealUsed)
                    player.UpdateHealth(0, -damageDone);
                    
                HealUsed = true;
                break;

            case DamageType.MaxHealthIncrease:
                if(!HealUsed)
                    player.UpdateHealth(0, damageDone);

                HealUsed = true;
                break; 

            case DamageType.Heal:
                if(!HealUsed)
                    player.UpdateHealth(damageDone, 0);
                    
                HealUsed = true;
                break; 
        }
    }
}
