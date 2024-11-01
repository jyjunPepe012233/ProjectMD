using MinD.Runtime.Managers;
using UnityEditor.Playables;
using UnityEngine;

namespace MinD.Runtime.Entity {

[RequireComponent(typeof(PlayerLocomotionHandler))]
[RequireComponent(typeof(PlayerAnimationHandler))]
[RequireComponent(typeof(PlayerAttributeHandler))]
[RequireComponent(typeof(PlayerInventoryHandler))]
[RequireComponent(typeof(PlayerEquipmentHandler))]
[RequireComponent(typeof(PlayerInteractionHandler))]
[RequireComponent(typeof(PlayerCombatHandler))]
public class Player : BaseEntity {
    
    [HideInInspector] public PlayerCamera camera;

    [HideInInspector] public PlayerLocomotionHandler locomotion;
    [HideInInspector] public PlayerAnimationHandler animation;
    [HideInInspector] public PlayerAttributeHandler attribute;
    [HideInInspector] public PlayerInventoryHandler inventory;
    [HideInInspector] public PlayerEquipmentHandler equipment;
    [HideInInspector] public PlayerInteractionHandler interaction;
    [HideInInspector] public PlayerCombatHandler combat;

    [Space(15)]
    [SerializeField] private bool infiniteAttribute;
    

    [Header("[ Attributes ]")]
    [SerializeField] private int curHp;
    [SerializeField] private int curMp;
    [SerializeField] private int curStamina;
    public int CurHp {
        get => curHp;
        set {
            // INFINITY ATTRIBUTES TO DEBUGGING
            if (infiniteAttribute) {
                curMp = attribute.maxMp;
                return;
            }
            
            curHp = value;
            if (curHp <= 0) {
                // DIE
            }

            curHp = Mathf.Clamp(curHp, 0, attribute.maxHp);

            PlayerHUDManager.Instance.RefreshHPBar();
        }
    }
    public int CurMp {
        get => curMp;
        set {
            // INFINITY ATTRIBUTES TO DEBUGGING
            if (infiniteAttribute) {
                curMp = attribute.maxMp;
                return;
            }
            
            curMp = value;
            curMp = Mathf.Clamp(curMp, 0, attribute.maxMp);

            PlayerHUDManager.Instance.RefreshMPBar();
        }
    }
    public int CurStamina {
        get => curStamina;
        set {
            // INFINITY ATTRIBUTES TO DEBUGGING
            if (infiniteAttribute) {
                curMp = attribute.maxMp;
                return;
            }
            
            curStamina = value;
            curStamina = Mathf.Clamp(curStamina, 0, attribute.maxStamina);

            PlayerHUDManager.Instance.RefreshStaminaBar();
        }
    }


    [Header("Flags")]
    public bool isPerformingAction;
    public bool isGrounded;
    public bool isMoving;
    public bool isJumping;
    public bool isLockOn;
    public bool canRotate;
    public bool canMove;
    


    protected override void Awake() {

        base.Awake();

        camera = FindObjectOfType<PlayerCamera>();
        locomotion = GetComponent<PlayerLocomotionHandler>();
        animation = GetComponent<PlayerAnimationHandler>();
        attribute = GetComponent<PlayerAttributeHandler>();
        inventory = GetComponent<PlayerInventoryHandler>();
        equipment = GetComponent<PlayerEquipmentHandler>();
        interaction = GetComponent<PlayerInteractionHandler>();
        combat = GetComponent<PlayerCombatHandler>();


        camera.owner = this;
        locomotion.owner = this;
        animation.owner = this;
        attribute.owner = this;
        inventory.owner = this;
        equipment.owner = this;
        interaction.owner = this;
        combat.owner = this;

    }

    void Start() {

        // LOAD DATA
        inventory.LoadItemData();
        
        attribute.SetBaseAttributesAsPerStats();
        PlayerHUDManager.Instance.RefreshAllStatusBar();
        
        
        // BASIC SETTINGS
        combat.getHitAction += combat.CancelMagicOnGetHit;
        
    }
    
    

    protected override void Update() {
        
        base.Update();

        camera.HandleCamera();
        locomotion.HandleAllLocomotion();
        attribute.HandleStamina();
        inventory.HandleQuickSlotSwapping();
        interaction.HandleInteraction();
        combat.HandleAllCombatAction();

    }


    public void CanRotate(bool active) {
        canRotate = active;
    }

    public void CanMove(bool active) {
        canMove = active;
    }
}

}