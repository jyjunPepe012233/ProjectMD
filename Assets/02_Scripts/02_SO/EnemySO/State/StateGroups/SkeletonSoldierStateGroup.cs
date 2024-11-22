using System;
using System.Collections;
using System.Collections.Generic;
using MinD.Runtime.Entity;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace MinD.SO.EnemySO.State.StateGroups
{

public class SkeletonSoldierStateGroup
{
    public enum States
    {
          PursueTarget
        , Chase
        , Idle
        , Attack1
        , DodgeBackAttack
        , LeapAttack
        , Rest
        
        , FackDath
        , Deth
        , GetHit
    }
    
    public enum GlobalStates
    {
        PursueDamege
    }
    
    
    public class PursueTarget : EnemyState
    {
        public override void Enter()
        {
            enemy.combat.target = enemy.combat.FindTargetBySight(80,18,3);
            enemy.stateMachine.ChangeStateByIndex((int)States.Rest);
        }

        public override void Tick()
        {
            
        //     if ()
        //     {
        //         //chase
        //     }
        //     else if ()
        //     {
        //         //Attack1
        //     }
        //     else if ()
        //     {
        //         //DabfeBackAttack
        //     }
        //     else if ()
        //     {
        //         //LeapAttack
        //     }
        //     else if ()
        //     {
        //         //Chase
        //     }
        //     else
        //     {
        //         //Idle
        //     }
        //     
        // }

        public override void Exit() { }
    }
    
    public class Chase : EnemyState
    {
        public override void Enter() { }

        public override void Tick()
        {
            enemy.navAgent.SetDestination(enemy.combat.target.transform.position);
            Vector3 dirx = enemy.transform.InverseTransformDirection(enemy.navAgent.desiredVelocity).normalized;

            float speed = 0.8735f;
            enemy.animation.SetBaseLocomotionParameter(dirx.x * speed, dirx.y * speed);

            if (enemy.combat.DistanceToTarget() <= 1.2f)
            {
                enemy.stateMachine.ChangeStateByIndex((int)States.PursueTarget);
            }
        }
        
        public override void Exit() { }
    }
    
    public class Idle : EnemyState // ===============================================================]
    {
        public override void Enter() { }
        public override void Tick()  { }
        public override void Exit()  { }
    }
    
    public class Attack1 : EnemyState
    {
        public override void Enter()
        {
            enemy.combat.target = enemy.combat.FindTargetBySight(80,17,3);
        }

        public override void Tick()
        {
            enemy.animation.PlayTargetAnimation("Combat Attack1");
        }

        public override void Exit()
        {
            if (Random.value <= 0.65f &&  enemy.combat.DistanceToTarget() <= 1.3f)
            {
                enemy.stateMachine.ChangeStateByIndex((int)States.DodgeBackAttack);
            }
            else
            {
                enemy.stateMachine.ChangeStateByIndex((int)States.PursueTarget);
            }
        }
    }
    
    public class DodgeBackAttack : EnemyState
    {
        public override void Enter()
        {
            enemy.combat.target = enemy.combat.FindTargetBySight(80,17,3);
        }

        public override void Tick()
        {
            enemy.animation.PlayTargetAnimation("Combat DobgeBackAttack",0.001f);
        }

        public override void Exit()
        {
            if (Random.value <= 0.65f)
            {
                enemy.stateMachine.ChangeStateByIndex((int)States.Rest);
            }
            else
            {
                enemy.stateMachine.ChangeStateByIndex((int)States.PursueTarget);
            }
        }
    }
    
    public class LeapAttack : EnemyState
    {
        public override void Enter()
        {
            enemy.combat.target = enemy.combat.FindTargetBySight(80, 17, 3);
        }

        public override void Tick()
        {
            enemy.animation.PlayTargetAnimation("Combat LeapAttack",0.001f);
        }

        public override void Exit()
        {
            if (enemy.combat.DistanceToTarget() <= 1.4f && Random.value <= 0.70f)
            {
                enemy.stateMachine.ChangeStateByIndex((int)States.Attack1);
            }
        }
    }
    
    public class Rest : EnemyState
    {
        float elapsedTime = 0;
        private float randTime = Random.value * 2 + 1;
        public override void Enter()
        {
            enemy.combat.target = enemy.combat.FindTargetBySight(80,17,5);
        }

        public override void Tick()
        {
            enemy.combat.target = enemy.combat.FindTargetBySight(80,17,5);
            enemy.animation.PlayTargetAnimation("Walk Combat",0.001f);
            elapsedTime += Time.deltaTime;
            if (elapsedTime > randTime)
            {
                enemy.stateMachine.ChangeStateByIndex((int)States.PursueTarget);
            }
        }

        public override void Exit() { }
    }
    
    public class FackDath : EnemyState
    {
        float elapsedTime = 0f;
        
        public override void Enter()
        {
            enemy.isDeath = true;
        }

        public override void Tick()
        {
            elapsedTime += Time.deltaTime;

            if (elapsedTime >= 7 /* or 네크로맨서 아군이 생기면 부활 인식*/)
            {
                enemy.isDeath = false;
                enemy.stateMachine.ChangeStateByIndex((int)States.PursueTarget);
            }

        }

        public override void Exit() { }
    }
    
    public class Deth : EnemyState
    {
        public override void Enter()
        {
            enemy.isDeath = true;
        }

        public override void Tick()
        {
            // 사망하는 코드 물어보기
        }

        public override void Exit() { }
    }
    
    public class GetHit : EnemyState
    {
        public override void Enter()
        {
            
        }

        public override void Tick()
        {
            
        }

        public override void Exit()
        {
            
        }
    }
    
    
    public class PursueDamege : EnemyState
    {
        public override void Enter()
        {
            
        }

        public override void Tick()
        {
            
        }

        public override void Exit()
        {
            
        }
    }
}
}