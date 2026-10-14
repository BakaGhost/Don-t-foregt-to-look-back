using UnityEngine;
using UnityEngine.AI;

public class StunState : MonsterState
{
    MonsterStateMachine myStateMachine;
    private NavMeshAgent myAgent;
    public Intersection myDes;

    private float t;
    private Intersection currentDes = null;
    
    public override void Init()
    {
        myStateMachine = GetComponentInParent<MonsterStateMachine>();
    }
    public override void Enter()
    {
        myAgent = myStateMachine.myAgent;
        myAgent.isStopped = true;
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
