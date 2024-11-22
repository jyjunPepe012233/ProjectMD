using System.Collections;
using System.Collections.Generic;
using MinD.Runtime.Entity;
using MinD.SO.EnemySO.State;
using MinD.SO.EnemySO.State.StateGroups;
using UnityEditor;
using UnityEngine;
using NotImplementedException = System.NotImplementedException;


namespace MinD.Runtime.Entity.Enemies {


    public class skeltonSoldier : Enemy
    {
        protected override void SetupStates()
        {
            states = new EnemyState[10];
            states[(int)SkeletonSoldierStateGroup.States.Idle] = new SkeletonSoldierStateGroup.PursueTarget();
            states[(int)SkeletonSoldierStateGroup.States.Idle] = new SkeletonSoldierStateGroup.Chase();
            states[(int)SkeletonSoldierStateGroup.States.Idle] = new SkeletonSoldierStateGroup.Idle();
            states[(int)SkeletonSoldierStateGroup.States.Idle] = new SkeletonSoldierStateGroup.Attack1();
            states[(int)SkeletonSoldierStateGroup.States.Idle] = new SkeletonSoldierStateGroup.DodgeBackAttack();
            states[(int)SkeletonSoldierStateGroup.States.Idle] = new SkeletonSoldierStateGroup.LeapAttack();
            states[(int)SkeletonSoldierStateGroup.States.Idle] = new SkeletonSoldierStateGroup.Rest();
            states[(int)SkeletonSoldierStateGroup.States.Idle] = new SkeletonSoldierStateGroup.FackDath();
            states[(int)SkeletonSoldierStateGroup.States.Idle] = new SkeletonSoldierStateGroup.Deth();
            states[(int)SkeletonSoldierStateGroup.States.Idle] = new SkeletonSoldierStateGroup.GetHit();
        }
        
        protected override IEnumerator Die()
        {
            animation.PlayTargetAnimation("Death",0.01f);
            
            yield return new WaitForSeconds(5.5f);
            
            Destroy(gameObject);
        }
    }
}