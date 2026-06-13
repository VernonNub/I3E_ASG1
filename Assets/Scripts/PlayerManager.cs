using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
public class PlayerManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [Header("Player Values")]
    public float playerHealth;
    public float playerMaxHealth;
    public float raycastLength = 3;
    private int mask = (1 << 6) | (1 << 7);
    
    [Header("Player Items")]
    public List<string> collectedItems = new List<string>();
    public List<string> uniqueItems = new List<string>();

    [Header("Points")]
    public int points;
    public int uniqueItemsCount;
    public int maxPoints;
    public int maxUniqueItems;

    [Header("UI Elements")]
    public Slider healthBar;
    public RectTransform deathNotification;
    public TMP_Text deathMessage;

    [Header("Player Components")]
    [SerializeField] CharacterController characterController;
    public GameObject currentCheckPoint;
    public GameObject playerCamera;

    [Header("Interactions")]
    [SerializeField] GameObject interactable;
    private InteractableManager interactableManager;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        ResetPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        if(playerHealth <= 0)
        {
            deathNotification.gameObject.SetActive(true);
            ResetPlayer();
        }

        UpdateHealthBar();
    }

    void OnClick()
    {
        deathNotification.gameObject.SetActive(false);
    }

    //Update Slider UI for health
    void UpdateHealthBar()
    {
        healthBar.maxValue = playerMaxHealth;

        healthBar.value = playerHealth;
    }

    void OnInteract()
    {
        InteractWithObject();
    }

    //For taking damage, maxhealth boosts, healing etc. Parameters --> Health change - amount of health given/minus, maxhealthchange - amount of health given/minus
    public void UpdateHealth(float HealthChange, float MaxHealthChange)
    {
        //Ensure that health does not go past MaxHealth (Doesnt give extra health)
        playerHealth = Mathf.Min(playerHealth + HealthChange, playerMaxHealth);

        //Ensure player max health is not 0 or does not become negative
        if(playerMaxHealth + MaxHealthChange <= 0)
        {
            playerMaxHealth = 1;
        }
        else
        {
            playerMaxHealth += MaxHealthChange;
        }    

        //Set new health amount
        playerMaxHealth += MaxHealthChange;
    }

    //Reset player stats and bring to checkpoint (E.g. When death)
    public void ResetPlayer()
    {
        //Reset health and any effects
        playerHealth = 100;
        playerMaxHealth = 100;

        //Tp playerback to checkpoint (Since the player is moved using character controller, it is important to disable the cc so that the character controller do not override the TP)
        characterController.enabled = false;
        transform.position = currentCheckPoint.transform.position;
        characterController.enabled = true;
    }    

    //Handles all interaction
    public void InteractWithObject()
    {
        //Casts raycast on object --> using camera since thats how players will interact
        if(Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out RaycastHit hit, raycastLength, mask))
        {
            //Gets the game object
            interactable = hit.collider.gameObject;

            //gets the manager and runs it --> it will then leave to the script to run the action accordingly
            interactableManager = interactable.GetComponent<InteractableManager>();
            interactableManager.RunInteraction();          
        }     
    }
}
