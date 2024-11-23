using MinD.SO.EnemySO.State;
using MinD.SO.EnemySO.State.StateGroups;


namespace MinD.Runtime.Entity.Enemies {


    public class SkeletonSoldier : Enemy
    {
        protected override void SetupStatesArray()
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
    }
}