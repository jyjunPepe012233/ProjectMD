using MinD.Runtime.Managers;
using MinD.Utility;
using MinD.Runtime.UI;
using MinD.SO.StatusFX.Effects;
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
    [HideInInspector] public PlayerDefenseMagic defenseMagic;
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
        camera.owner = this;
        defenseMagic = FindObjectOfType<PlayerDefenseMagic>();
        defenseMagic.owner = this;

    }
    void Start() {
        
        PhysicUtility.IgnoreCollisionUtil(this, defenseMagic.defenseCollider);
        
        
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
        inventory.HandleInventoryOpen();
        inventory.HandleQuickSlotSwapping();
        interaction.HandleInteraction();
        combat.HandleAllCombatAction();

    }

    public override void OnDamaged(TakeHealthDamage damage) {
        // HANDLE POISE BREAK AND CANCELING ACTION
		
        // IF PLAYER HAS IMMUNE OF POISE BREAK, DON'T GIVE POISE BREAK
        // AND PLAYER IS DIED AFTER DRAIN HP, DON'T GIVE POISE BREAK
        if (immunePoiseBreak || isDeath) {
            return;
        }

        // CANCEL ACTIONS
        combat.CancelMagicOnGetHit();
        locomotion.CancelBlink();
        
        // PLAY POISE BREAK ANIMATION
        int poiseBreakAmount = TakeHealthDamage.GetPoiseBreakAmount(damage.poiseBreakDamage, attribute.PoiseBreakResistance);
        animation.PlayTargetAction(animation.GetPoiseBreakAnimation(poiseBreakAmount, damage.attackAngle), true, true, false, false);
        
    }

    protected override void OnDeath() {
        isDeath = true;
        PhysicUtility.SetActiveChildrenColliders(transform, false, WorldUtility.damageableLayerMask);
        
        // CANCEL ACTIONS
        combat.CancelMagicOnGetHit();
        locomotion.CancelBlink();
        
        PlayerHUDManager.Instance.PlayBurstPopup(PlayerHUDManager.playerHUD.youDiedPopup, true);
        
        animation.PlayTargetAction("Death", true, true, false, false);
        
    }
    
}

}