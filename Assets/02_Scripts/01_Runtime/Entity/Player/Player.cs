using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MinD.UI;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.VirtualTexturing;
using UnityEngine.Serialization;
using UnityEngine.TextCore.Text;

#region RequireComponent
[RequireComponent(typeof(PlayerLocomotionHandler))]
[RequireComponent(typeof(PlayerAnimationHandler))]
[RequireComponent(typeof(PlayerAttributeHandler))]
[RequireComponent(typeof(PlayerInventoryHandler))]
[RequireComponent(typeof(PlayerEquipmentHandler))]
[RequireComponent(typeof(PlayerInteractionHandler))]
[RequireComponent(typeof(PlayerCombatHandler))]
#endregion

public class Player : BaseEntity {
    
    [Header("[ Attributes ]")]
    private int curHp; 
    private int curMp;
    private int curStamina;

    public int CurHp {
        get => curHp;
        set {
            curHp = value;
            if (curHp <= 0) {
                curHp = 0;
                
                // DIE
                return;
            }
            
            PlayerHUDManager.Instance.RefreshHPBar();
        }
    }
    public int CurMp {
        get => curMp;
        set {
            curMp = value;
            curMp = Mathf.Max(curMp, 0); // IF CUR MP IS LOWER THAN 0, RETURN 0
            
            PlayerHUDManager.Instance.RefreshMPBar();
        }
    }
    public int CurStamina {
        get => curStamina;
        set {
            curStamina = value;
            curStamina = Mathf.Max(curStamina, 0);
            
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
    
    [HideInInspector] public PlayerCamera camera;
    
    [HideInInspector] public PlayerLocomotionHandler locomotion;
    [HideInInspector] public PlayerAnimationHandler animation;
    [HideInInspector] public PlayerAttributeHandler attribute;
    [HideInInspector] public PlayerInventoryHandler inventory;
    [HideInInspector] public PlayerEquipmentHandler equipment;
    [HideInInspector] public PlayerInteractionHandler interaction;
    [HideInInspector] public PlayerCombatHandler combat;
    



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

    void OnEnable() {
        
        inventory.LoadItemData();
        
        PlayerHUDManager.Instance.RefreshAllStatusBar();
        
    }

    void Update() {
        
        camera.HandleCamera();
        locomotion.HandleAllLocomotion();
        inventory.HandleQuickSlotSwapping();
        interaction.HandleInteraction();
        combat.HandleAllCombatAction();

    }
}