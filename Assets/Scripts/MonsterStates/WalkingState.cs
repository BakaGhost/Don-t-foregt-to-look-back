using UnityEngine;
using UnityEngine.AI;

public class WalkingState : MonsterState
{
    MonsterStateMachine myStateMachine;
    private NavMeshAgent myAgent;
    public Intersection myDes;

    private float t;
    private Intersection previousDes = null;
    
    public override void Init()
    {
        myStateMachine = GetComponentInParent<MonsterStateMachine>();
    }
    public override void Enter()
    {
        myAgent = myStateMachine.myAgent;
        myAgent.isStopped = false;
        ChangeDestination();
    }
    public override void UpdateState()
    {
        t += Time.deltaTime;
        if (t < 0.2) return;
        if (!myAgent.pathPending && myAgent.remainingDistance <= myAgent.stoppingDistance)
        {
            if (!myAgent.hasPath || myAgent.velocity.sqrMagnitude == 0f)
            {
                ChangeDestination();
                // myAgent.SetDestination(myDes.transform.position); 
            }
        }
        myAgent.SetDestination(myDes.transform.position);
        t = 0f;
    }
    public override void Exit()
    {
        return;
    }

    public void ChangeDestination()
    {
        if (myStateMachine.intersections.Count <= 1) return;

        Intersection newDes = myDes;

        while (newDes == myDes || newDes == previousDes)
        {
            int randNbr = Random.Range(0, myStateMachine.intersections.Count);
            newDes = myStateMachine.intersections[randNbr];
        }
        previousDes = myDes;
        myDes = newDes;

    }
}
