using Unity.VisualScripting.ReorderableList.Element_Adder_Menu;
using UnityEngine;

public class InteractableManager : MonoBehaviour
{
    public int points;

    public string itemName;

    public PlayerManager player;

    public void RunInteraction()
    {
        //Collectibles layer is 7 (E.g. coins) --> Run Collection of Items
        if(gameObject.layer == 7)
        {
            CollectItem();
        }
        //Door layer is 6 --> so run door interaction
        else if(gameObject.layer == 6)
        {
            DoorInteraction();
        }
    }

    void DoorInteraction()
    {
        
    }

    void CollectItem()
    {
        if(player.collectedItems.Contains(itemName))
        {
            player.points += points;
        }
        else
        {
            player.collectedItems.Add(itemName);
        }
    }
}
