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
    public enum InteractableType
    {
        //Coins
        points,
        //Door
        Door,
        //Unique Collectibles are optional but cool achievements to find!
        UniqueCollectible,
        //Objectives --> keycard, fire extinguisher etc
        Objectives
    }

    public InteractableType interactableType;

    

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
        }
    }

    void Start()
    {
        //Add the player component
        player = GameObject.Find("Player").GetComponent<PlayerManager>();

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
}
