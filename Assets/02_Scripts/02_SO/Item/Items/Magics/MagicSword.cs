using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using MinD.Runtime.Object.Magics;
using MinD.Runtime.System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MinD.SO.Item.Items {

[CreateAssetMenu(menuName = "MinD/Item/Items/Magics/Magic Sword")]
public class MagicSword : Magic
{
    [SerializeField] private GameObject magicSword;
    
    private List<GameObject> projectiles; // 발사체 저장 리스트
    private List<MagicSwordProjectile> swordProjectiles; // 발사체들 MagicSwordProjectile

    private float useElapsedTiem;
    
    public static readonly Vector3[] projectilePositions = new Vector3[3]
    {
        new Vector3(-3, -1, 0).normalized * 0.8f,
        new Vector3( 0,  0, 0),
        new Vector3( 3, -1, 0).normalized * 0.8f
    };
    
/* 데미지 콜라이더 작업중이었슴 */ 
    
    
    public override void OnUse()
    {
        Debug.Log("use Masic");
        useElapsedTiem = 0;
        
        if (!castPlayer.isPerformingAction){
            castPlayer.animation.PlayTargetAction("MagicSword",true, true, false, false);
        }
        projectiles = new List<GameObject>();
        swordProjectiles = new List<MagicSwordProjectile>();
    }

    /* 생성 각도 */ /*따라오기*/ /* 첫 생성 위치 문제있음 */
    
    
    public override void Tick()
    {
        useElapsedTiem += Time.deltaTime;
        
        Debug.Log("isPerformingAction: " + castPlayer.isPerformingAction);
        if (!castPlayer.isPerformingAction) // && useElapsedTiem >= 1.4f)
        {
            castPlayer.combat.ExitCurrentMagic();
        }
        
    }

    public override void OnReleaseInput()
    {
        Debug.Log("OnReleaseInput");
    }

    public override void OnCancel()
    {
        Debug.Log("OnCancel");
        for (int i = 0; i < 3; i++)
        {
            MagicSwordProjectile sword = swordProjectiles[i];
            sword.StartCoroutine(sword.Explode());
        }
        castPlayer.combat.ExitCurrentMagic();
    }

    public override void OnExit()
    {
        Debug.Log("OnExit");
    }

    /* 버그진행 순서
       애니메 이밴트
       set Position -> re anime --------------------------- set Position << 판독결과 얘 문제 아님
       re anime.evnet -> 에러
       NullReferenceException: Object reference not set to an instance of an object
       MinD.Runtime.Entity.PlayerCombatHandler.OnSuccessfullyCast () (at Assets/02_Scripts/01_Runtime/Entity/Player/PlayerCombatHandler.cs:215)
       판독결과 91번 줄이 문제임
       
       재시작 할때는 시전중이 아님
     */
    
    public override void OnSuccessfullyCast()
    {
        Debug.Log("OnSuccessfullyCast");
        
        for (int i = 0; i < 3; i++) // 마법검 생성 및 위치 조정
        {
            //  createSword
            projectiles.Add(Instantiate(magicSword, castPlayer.transform.position + new Vector3(0, 2.7f, 0),
                castPlayer.transform.rotation));
            Debug.Log("i: "+i);
            
            swordProjectiles.Add(projectiles[i].GetComponent<MagicSwordProjectile>()); // 이 측복받을 코드가 문제임. 왜?
            
            /*
             오브젝트 생성자체에는 문제없음
             GetComponent<MagicSwordProjectile>()에 문제가 있는것 같음 Add만 실행하니 잘됨 아래는 발생한 오류
             rgumentOutOfRangeException: Index was out of range. Must be non-negative and less than the size of the collection.
               Parameter name: index
               System.Collections.Generic.List`1[T].get_Item (System.Int32 index) (at <c816f303bdad4e9a8d8dabcc4fd172eb>:0)
               MinD.SO.Item.Items.MagicSword.OnCancel () (at Assets/02_Scripts/02_SO/Item/Items/Magics/MagicSword.cs:62)
              
             */
            

            //  set swordPosition
            Debug.Log("start SetCoroutine");
            swordProjectiles[i].StartCoroutine(swordProjectiles[i]
                .SetSwordPosition(castPlayer, castPlayer.combat.target, projectilePositions[i]));
            
        }

    }
}
}

