using System;
using UnityEngine;

public class DamageManager : MonoBehaviour
{
    //Using enum to create multiple damaging types --> which will be selected based on which type I want
    public enum DamageType
    {
        //Instance of damage
        damage,
        //Damage over time
        DoT,
        //Decreases user's max health
        MaxHealthDecrease,
        //Increases user's max health
        MaxHealthIncrease,
        //Instance of heal
        Heal
    }

    //Clearing hazard methods e.g. Remove --> extinguish fire, resist --> gas mask for toxic gas
    public enum ClearanceMethod
    {
        Remove,
        Resist,
    }

    public ClearanceMethod clearanceMethod;

    public bool isClearable;

    [Header("Damage Info")]
    //Here is where we assign the damagetype --> through inspector --> this allows for the use of prefabs 
    //where all damagetype will use one script!
    public DamageType damage;

    //DamageDone --> done through inspector as well --> public in case I want other scripts to change this
    public float damageDone;

    //Ensures that the user does not heal/debuff more than once
    //applies to maxhealth boost, max health decrease and heal
    private bool HealUsed = false;

    //Trigger Stay --> assign our playermanager variable to the player's playermanager script --> for DoT --> Using time.deltatime
    void OnTriggerStay(Collider other)
    {
        if(other.name == "Player")
        {
            DamagePlayerOverTime(other.gameObject.GetComponent<PlayerManager>());
        }
    }
    
    //Trigger Enter --> assign our playermanager variable to the player's playermanager script
    void OnTriggerEnter(Collider other)
    {
        if(other.name == "Player")
        {
            DamagePlayer(other.gameObject.GetComponent<PlayerManager>());
        }
    }

    private void DamagePlayerOverTime(PlayerManager player)
    {
        //Based on my understanding + research swithc is basically an if else statement that 
        //cycles between code functions based on the variable value
        switch(damage)
        {
            //if its DoT --> damageDone * TIme.deltaTime so  that damage is done over time
            case DamageType.DoT:
                player.UpdateHealth(-damageDone * Time.deltaTime, 0);
                break;
        }
    }

    private void DamagePlayer(PlayerManager player)
    {
        switch(damage)
        {
            //All of my damagedone is handled wiith the update health method in my playermanager 
            //More details look at player manager script.
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

    public void ClearHazard()
    {
        switch(clearanceMethod)
        {
            case ClearanceMethod.Remove:
                gameObject.SetActive(false);
                break;
            
            case ClearanceMethod.Resist:
                damageDone = 0;
                break;
        }
    }
}
