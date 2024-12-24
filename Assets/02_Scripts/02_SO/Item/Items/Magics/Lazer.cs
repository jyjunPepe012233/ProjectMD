using System;
using System.Collections;
using System.Collections.Generic;
using MinD.Runtime.Object.Magics;
using MinD.SO.Item;
using Unity.Properties;
using UnityEngine;
using UnityEngine.Serialization;
using Vector3 = System.Numerics.Vector3;


namespace MinD.SO.Item.Items
{


[CreateAssetMenu(menuName = "MinD/Item/Items/Magics/Lazer")]
public class Lazer : Magic
{
    
     [SerializeField] private GameObject lazer;
     private Transform targetOption;

     [SerializeField] private UnityEngine.Vector3 createHigh = new UnityEngine.Vector3(0,0.5f,0);

     
    private LazerProjectile lazerProjectile;
    private GameObject copyLazer;

    /* ToDo :: 완료 후 마감하기 */

    public override void OnUse()
    {
        if (!castPlayer.isPerformingAction)
        {
            castPlayer.animation.PlayTargetAction("Lazer", true, true, true, false);
            targetOption = castPlayer.camera.currentTargetOption;
            copyLazer = Instantiate(lazer, castPlayer.transform.position + castPlayer.transform.forward * 1.25f + createHigh , castPlayer.transform.rotation);
            lazerProjectile = copyLazer.GetComponent<LazerProjectile>();
            lazerProjectile.SetPlayer(castPlayer);
        }
        
    }

    public override void Tick()
    {
        if (!castPlayer.isPerformingAction)
        {
            castPlayer.combat.ExitCurrentMagic();
        }
    }

    public override void OnReleaseInput()
    {
        //
    }

    public override void OnCancel()
    {
        //
    }

    public override void OnExit()
    {
        //
    }

    public override void OnSuccessfullyCast()
    {
        Debug.Log("start Coroutine");
        copyLazer.SetActive(true);
        if (castPlayer.isLockOn)
        {
            lazerProjectile.ShootCommonMagic(targetOption.position);
        }
        else
        {
            lazerProjectile.ShootCommonMagic();
        }
    }

}
}