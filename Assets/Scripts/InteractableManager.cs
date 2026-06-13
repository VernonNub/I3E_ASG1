using System.Collections;
using Unity.VisualScripting;
using Unity.VisualScripting.ReorderableList.Element_Adder_Menu;
using UnityEngine;
using UnityEngine.Rendering;

public class InteractableManager : MonoBehaviour
{
    private PlayerManager player;

    [Header("Interactable Info")]
    public int points;
    public string itemName;
    public string requiredItem;
    public enum InteractableType
    {
        //Coins
        points,
        //Door
        Door,
        //Unique Collectibles are optional but cool achievements to find!
        UniqueCollectible,
        //Objectives --> keycard, fire extinguisher etc
        Objectives,
        //Hazards --> Extinguish fire etc
        hazard
    }

    public InteractableType interactableType;
    private Animator animator;

    

    public void RunInteraction()
    {
        //Depending on the interactible, run the function (Door --> animate door, coin --> add points and destroy etc)
        switch(interactableType)
        {
            case InteractableType.points:
                CollectPoints();
                break;
            
            case InteractableType.Objectives:
                CollectObjective();
                break;

            case InteractableType.UniqueCollectible:
                CollectUnique();
                break;

            case InteractableType.Door:
                DoorInteraction();
                break;

            case InteractableType.hazard:
                RemoveHazard();
                break;
        }
    }

    void OnEnable()
    {
        //Add the player component
        player = GameObject.Find("Player").GetComponent<PlayerManager>();

        //Add animator
        animator = gameObject.GetComponent<Animator>();
    }
    void Start()
    {
        //Set player's max amt of points (for UI and winning purposes)
        //Uses code so that i dont have to manually change the max points everytime i duplicate my objects or add a prefab ;-;
        switch(interactableType)
        {
            case InteractableType.points:
                player.maxPoints += points;
                break;

            case InteractableType.UniqueCollectible:
                player.maxUniqueItems += 1;
                break;
        }
    }

    //Update --> mainly for closing door when player walks away
    void Update()
    {
        switch(interactableType)
        {
            case InteractableType.Door:
            //Check if player is far from door by comparing both vector3s & door is open --> closes the door
                if(Vector3.Distance(gameObject.transform.position, player.gameObject.transform.position) >= 20.0f && animator.GetBool("isOpen") == true)
                {
                    DoorInteraction();
                }
                break;
        }
    }

    void DoorInteraction()
    {
        
    }

    //Collecting points
    void CollectPoints()
    {
        player.points += points;
        Destroy(gameObject);
    }

    //Collecting unique object
    void CollectUnique()
    {
        player.uniqueItems.Add(itemName);
        Destroy(gameObject);
    }

    //Collecting objective object
    void CollectObjective()
    {
        player.collectedItems.Add(itemName);
        Destroy(gameObject);
    }

    //Remove hazard
    void RemoveHazard()
    {
        //Check if clearable & item is collected
        if((player.collectedItems.Contains(requiredItem) || requiredItem == "") && gameObject.GetComponent<DamageManager>().isClearable)
        {
            gameObject.GetComponent<DamageManager>().ClearHazard();
        }
    }
}
