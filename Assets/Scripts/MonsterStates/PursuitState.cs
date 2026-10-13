using UnityEngine;
using UnityEngine.AI;

public class PursuitState : MonsterState
{
    MonsterStateMachine myStateMachine;
    private NavMeshAgent myAgent;
    public Intersection myDes;

    private float t;
    
    public override void Init()
    {
        myStateMachine = GetComponentInParent<MonsterStateMachine>();
        t = 0f;
    }
    public override void Enter()
    {
        myAgent = myStateMachine.myAgent;
        t = 0f;
        myAgent.SetDestination(myStateMachine.playerController.transform.position);
    }
    public override void UpdateState()
    {
        t += Time.deltaTime;
        if (t < 0.2) return;
        myAgent.destination = myStateMachine.playerController.transform.position;
        t = 0;
    }
    public override void Exit()
    {
        return;
    }
}
