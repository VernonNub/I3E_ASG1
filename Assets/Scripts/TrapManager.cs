using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class TrapManager : MonoBehaviour
{
    //For moving objects (E.g. block player, move platform etc)
    public Vector3 targetPosition;

    //Object to move/delete
    public GameObject targetObject;

    //For object to return to initial state after player passes the trap
    [Header("Initial Stats")] 
    private Vector3 startPosition;
    public float moveTime;

    //Same as DamageManager --> uses enum to decide which code to run based on the trap type
    //E.g. removeObject trap --> setactive(false), moveobject trap --> transform.position = targetposition etc
    public enum TrapType
    {
        RemoveObject,
        MoveObject
    }
    public TrapType trap;

    void Start()
    {
        startPosition = targetObject.transform.localPosition;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.name == "Player")
        {
            RunTrap();
        }
    }

    //Reset the trap when player passes the trap or dies and goes back to the check point
    void OnTriggerExit(Collider other)
    {
        if(other.name == "Player")
        {
            ResetTrap();
        }
    }

    //trap mechanic where it runs based on the varible trap
    private void RunTrap()
    {
        switch (trap)
        {
            //Moves object
            case TrapType.MoveObject:
                StartCoroutine(MoveObject());
                break;
            
            //Set object's active to false --> basically removes it
            case TrapType.RemoveObject:
                targetObject.SetActive(false);
                break;
        }
    }

    //Makes object return to original position & makes object turn normal
    private void ResetTrap()
    {
        targetObject.SetActive(true);
        targetObject.transform.localPosition = startPosition;
    }

    //Co routine for moving the object over a specified time frame
    //Researched --> coroutine is basically a function that runs over multiple frame. Its a function that can run asynchronously in the background.
    //Therefore its perfect for doing like a simple animation here
    IEnumerator MoveObject()
    {
        //Track the time
        float elapsedTime = 0;

        while(elapsedTime < moveTime)
        {
            //Count towards the movetime
            elapsedTime += Time.deltaTime;

            //Lerp basically returns a position between 2 values using the third value as a fraction (third value is from 0-1)
            //E.g. Lerp(0, 10, 0.5) returns 5 since the third value is a fraction of half, it returns the value half from 0 to 10
            //Its like a journey where firstvalue is start point, second is end point, third is the percentage completed
            //Lerp is used here to move the object based on the time elapsed. elapsedTime/moveTime --> basically as more time passes the fraction become higher, the object moves closer
            //Initally made a mistake where i use the object current position in lerp --> causes it to move more than before because the distance is smaller (Asked AI with help cause idk how to fix at the time)
            targetObject.transform.localPosition = Vector3.Lerp(startPosition, targetPosition, elapsedTime/moveTime);

            //To stop the coroutine
            yield return null;
        }
    }
}
