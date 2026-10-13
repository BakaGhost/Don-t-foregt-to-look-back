using UnityEngine;

public class CaughtState : MonsterState
{
    MonsterStateMachine myStateMachine;
    
    public override void Init()
    {
        myStateMachine = GetComponentInParent<MonsterStateMachine>();
    }
    public override void Enter()
    {
        return;
    }
    public override void UpdateState()
    {
        return;
    }
    public override void Exit()
    {
        return;
    }
}
