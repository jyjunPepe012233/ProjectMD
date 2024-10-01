using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;
using UnityEngine.Serialization;
using UnityEngine.TextCore.Text;

[RequireComponent(typeof(PlayerLocomotionHandler), typeof(PlayerAnimationHandler))]
[RequireComponent(typeof(PlayerAttributeHandler), typeof(EntityStatusFxHandler))]
public class Player : BaseEntity {

    [Header("[ Attributes ]")]
    public int curHp;
    
    [Header("Flags")]
    public bool isPerformingAction;
    public bool isGrounded;
    public bool isMoving;
    public bool isJumping;
    public bool canRotate;
    public bool canMove;
    
    [HideInInspector] public PlayerCamera camera;
    
    [HideInInspector] public PlayerLocomotionHandler locomotion;
    [HideInInspector] public PlayerAnimationHandler animation;
    [HideInInspector] public PlayerAttributeHandler attribute;
    [HideInInspector] public PlayerInventoryHandler inventory;
    [HideInInspector] public PlayerEquipmentHandler equipment;
    [HideInInspector] public EntityStatusFxHandler effect;
    



    void Awake() {

        base.Awake();

        camera = FindObjectOfType<PlayerCamera>();
        locomotion = GetComponent<PlayerLocomotionHandler>();
        animation = GetComponent<PlayerAnimationHandler>();
        attribute = GetComponent<PlayerAttributeHandler>();
        inventory = GetComponent<PlayerInventoryHandler>();
        equipment = GetComponent<PlayerEquipmentHandler>(); 
        
    }

    void Update() {
        
        locomotion.HandleAllLocomotion();

    }
}