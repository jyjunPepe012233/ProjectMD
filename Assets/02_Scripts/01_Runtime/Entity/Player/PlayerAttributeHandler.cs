using MinD.Runtime.Managers;
using MinD.Structs;
using UnityEngine;

namespace MinD.Runtime.Entity {

public class PlayerAttributeHandler : MonoBehaviour {

    [HideInInspector] public Player owner;

    [Header("[ Debug ]")]
    public bool resetAttributes;
    
    
    [Header("[ Stats ]")]
    [Range(0, 99)] public int vitality, endurance, mind, intelligence, faith;

    [Header("[ Status Attributes ]")]
    public int maxHp, maxMp, maxStamina;
    [Space(5)]
    public float magicForceModifier;
    public float divine;
    [Space(5)]
    public DamageNegation damageNegation;

    
    [Header("[ Other Attributes ]")]
    public int memoryCapacity;
    public float staminaRecoverySpeed;
    public float staminaRecoveryDelay;

    private float staminaRecoveryTimer;
    private float staminaRecoveryFloatTemp;
    
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

        // FILL STAMINA
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
    } // 네스팅 꼬라지 진짜 ㅇㅏ오
}

}