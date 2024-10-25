    using System;
using System.Collections;
using System.Collections.Generic;
using MinD;
using MinD.Combat;
using MinD.UI;
using UnityEngine;

public class PlayerAttributeHandler : MonoBehaviour {

    [HideInInspector] public Player owner;

    [Header("[ Stats ]")]
    public int vitality;
    public int endurance;
    public int mind;
    public int intelligence;
    public int faith;

    [Header("[ Status Attributes ]")]
    public int maxHp;
    public int maxMp;
    public int maxStamina;
    [Space(7)]
    public float magicForceModifier;
    public float divine;
    [Space(7)]
    public DamageNegation damageNegation;

    [Header("[ Other Attributes ]")]
    public int memoryCapacity;
    public float staminaRecoverySpeed;
    public float staminaRecoveryDelay;

    private float staminaRecoveryTimer;
    private float staminaRecoveryFloatTemp;
    
    [Header("[ Debug ]")]
    public bool resetAttributes;
    
    void OnValidate() {
        if (resetAttributes) {
            SetBaseAttributesAsPerStats();
            owner.CurHp = maxHp;
            owner.CurMp = maxMp;
            owner.CurStamina = maxStamina;
            resetAttributes = false;
            PlayerHUDManager.Instance.RefreshAllStatusBar();
        }
    }


    public void Awake() {
        owner = GetComponent<Player>();
    }


    void SetBaseAttributesAsPerStats() {

        // Vitality
        maxHp = 100 + (vitality * 15);
        damageNegation.fire = vitality * 0.4f / 100;
        
        // Endurance
        maxStamina = 45 + (endurance * 2);
        
        // Mind
        maxMp = 100 + (mind * 2);
        damageNegation.magic = (mind * 0.25f) / 100;
        
        // Intelligence
        magicForceModifier = 1 + (intelligence * 0.035f);
        
        // Faith
        divine = faith * 0.04f;
    }

    void ModifyAttributesAsPerEquipment() {
        
        
    }



    public void HandleStamina() {

        if (owner.CurStamina < maxStamina) {
            
            // CHECK FLAGS AND CONTROL TIMER TO RECOVERY STAMINA
            if (!owner.isPerformingAction && !owner.locomotion.isSprinting) {
                
                staminaRecoveryTimer += Time.deltaTime;
                
            } else {
                staminaRecoveryTimer = 0;
            }
            
            
            if (staminaRecoveryTimer > staminaRecoveryDelay) {
                staminaRecoveryFloatTemp += staminaRecoverySpeed * Time.deltaTime;

                if (staminaRecoveryFloatTemp > 1) {

                    // TO ELIMINATE ERROR THAT OCCUR IN CONVERTING FLOAT TO INT
                    while (true) {
                        if (staminaRecoveryFloatTemp < 1)
                            break;
                        
                        owner.CurStamina += 1;
                        staminaRecoveryFloatTemp -= 1;
                    }
                    
                }
            }
        }

    }
}
