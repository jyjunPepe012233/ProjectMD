using System.Collections;
using MinD.Runtime.Managers;
using UnityEngine;
using NotImplementedException = System.NotImplementedException;

namespace MinD.Runtime.Entity {

[RequireComponent(typeof(PlayerLocomotionHandler))]
[RequireComponent(typeof(PlayerAnimationHandler))]
[RequireComponent(typeof(PlayerInventoryHandler))]
[RequireComponent(typeof(PlayerEquipmentHandler))]
[RequireComponent(typeof(PlayerInteractionHandler))]
[RequireComponent(typeof(PlayerCombatHandler))]
public class Player : BaseEntity {
    
    [HideInInspector] public PlayerCamera camera;
    [HideInInspector] public PlayerAnimationHandler animation;
    [HideInInspector] public PlayerAttributeHandler attribute;
    [HideInInspector] public PlayerLocomotionHandler locomotion;
    [HideInInspector] public PlayerInventoryHandler inventory;
    [HideInInspector] public PlayerEquipmentHandler equipment;
    [HideInInspector] public PlayerInteractionHandler interaction;
    [HideInInspector] public PlayerCombatHandler combat;
    
    public override int CurHp {
        get => curHp;
        set {
            curHp = value;
            if (curHp <= 0) {
                OnDeath();
            }
            curHp = Mathf.Clamp(curHp, 0, attribute.MaxHp);
            PlayerHUDManager.Instance.RefreshHPBar();
        }
    }
    
    [SerializeField] private int curMp;
    [SerializeField] private int curStamina;
    public int CurMp {
        get => curMp;
        set {
            curMp = value;
            curMp = Mathf.Clamp(curMp, 0, attribute.maxMp);
            PlayerHUDManager.Instance.RefreshMPBar();
        }
    }
    public int CurStamina {
        get => curStamina;
        set {
            curStamina = value;
            curStamina = Mathf.Clamp(curStamina, 0, attribute.maxStamina);
            PlayerHUDManager.Instance.RefreshStaminaBar();
        }
    }


    [Header("Flags")]
    public bool isPerformingAction;
    public bool isGrounded;
    public bool isMoving;
    public bool isLockOn;
    public bool canRotate;
    public bool canMove;
    


    protected override void Awake() {

        base.Awake();
        
        animation = GetComponent<PlayerAnimationHandler>();
        attribute = GetComponent<PlayerAttributeHandler>();
        locomotion = GetComponent<PlayerLocomotionHandler>();
        inventory = GetComponent<PlayerInventoryHandler>();
        equipment = GetComponent<PlayerEquipmentHandler>();
        interaction = GetComponent<PlayerInteractionHandler>();
        combat = GetComponent<PlayerCombatHandler>();
        
        camera = FindObjectOfType<PlayerCamera>();
        combat.defenseMagic = FindObjectOfType<PlayerDefenseMagic>();

        camera.owner = this;
        combat.defenseMagic.owner = this;

    }
    void Start() {
        
        // RESET PROPERTIES
        getHitAction += locomotion.CancelBlink;
        
        inventory.LoadItemData();
        
        attribute.SetBaseAttributesAsPerStats();
        PlayerHUDManager.Instance.RefreshAllStatusBar();
    }
    
    protected override void Update() {
        
        base.Update();
        
        attribute.HandleStamina();

        camera.HandleCamera();
        locomotion.HandleAllLocomotion();
        animation.HandleAllParameter();
        inventory.HandleQuickSlotSwapping();
        interaction.HandleInteraction();
        combat.HandleAllCombatAction();

    }

    protected override void OnDeath() {

    }
    
}

}