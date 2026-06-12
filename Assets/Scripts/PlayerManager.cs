using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
public class PlayerManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [Header("Player Values")]
    public int playerHealth;
    public int playerMaxHealth;
    public float raycastLength = 3;
    private int mask = (1 << 6) | (1 << 7);
    public int points;
    public List<string> collectedItems = new List<string>()
    {
        "coin"
    };

    [Header("UI Elements")]
    public Slider healthBar;
    public RectTransform deathNotification;
    public TMP_Text deathMessage;

    [Header("Player Components")]
    [SerializeField] CharacterController characterController;
    [SerializeField] GameObject currentCheckPoint;
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
    void UpdateHealth(int HealthChange, int MaxHealthChange)
    {
        Debug.Log("oof -10");
        //Ensure that health does not go past MaxHealth (Doesnt give extra health)
        playerHealth = Mathf.Min(playerHealth + HealthChange, playerMaxHealth);

        playerHealth += MaxHealthChange;
        //Set new health amount
        playerMaxHealth += MaxHealthChange;

        //UpdateHealthbar UI after taking effects
        UpdateHealthBar();
    }

    //Reset player stats and bring to checkpoint (E.g. When death)
    void ResetPlayer()
    {
        //Reset health and any effects
        playerHealth = 100;
        playerMaxHealth = 100;

        UpdateHealthBar();

        //Tp playerback to checkpoint (Since the player is moved using character controller, it is important to disable the cc so that the character controller do not override the TP)
        characterController.enabled = false;
        transform.position = currentCheckPoint.transform.position;
        characterController.enabled = true;
    }    

    void InteractWithObject()
    {
        if(Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out RaycastHit hit, raycastLength, mask))
        {
            interactable = hit.collider.gameObject;

            interactableManager = interactable.GetComponent<InteractableManager>();
            interactableManager.player = this;
            interactableManager.RunInteraction();          
        }     
    }
}
