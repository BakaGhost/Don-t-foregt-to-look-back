using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class MonsterStateMachine : MonoBehaviour
{
    public FirstPersonController playerController;
    public List<Intersection> intersections;
    
    [Header("Paramètres de déplacement")]
    public float speed;
    public float runSpeed;
    public float stunDuration;
    
    [Header("Paramètres de ciblage (Cône de détection)")]
    [SerializeField] private float rayDistance = 10f;
    [SerializeField] private float sphereRadius = 2f;
    [SerializeField] private float maxAngle = 60f;
    [SerializeField] private LayerMask hitLayers;
    
    [Header("States")]
    MonsterState currentState;
    MonsterState previousState;
    MonsterState nextState;
    public MonsterState firstState;
    MonsterState[] states;
    
    [Header("NavMesh")]
    [HideInInspector] public NavMeshAgent myAgent;
    
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        myAgent = GetComponent<NavMeshAgent>();
        states = GetComponentsInChildren<MonsterState>();
        
        foreach (MonsterState state in states)
        {
            state.Init();
        }
        
        if (firstState != null)
        {
            currentState = firstState;
        }
        else
        {
            currentState = states[0];
        }
        currentState.Enter();
    }

    // Update is called once per frame
    void Update()
    {
        CheckRaycast();
        currentState.UpdateState();
    }
    
    public void CheckRaycast()
    {
        Vector3 origin = transform.position;
        Vector3 rayDirection =transform.forward;
        RaycastHit[] hits = Physics.SphereCastAll(origin, sphereRadius, rayDirection, rayDistance, hitLayers);
        
        GameObject closestValidObject = null;
        float minDistance = float.MaxValue;

        foreach (RaycastHit hit in hits)
        {
            Vector3 targetPos = hit.collider.transform.position;
            if (targetPos == origin) continue;
            
            Vector3 directionToObject = (targetPos - origin).normalized;
            float angleToObject = Vector3.Angle(rayDirection, directionToObject);

            if (angleToObject <= maxAngle)
            {
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
            }
        }
    }

    public void Transition(MonsterState newState)
    {
        if (currentState != null)
        {
            currentState.Exit();
        }
        previousState = currentState;
        nextState = newState;
        currentState = newState;
        
        currentState.Enter();
    }
}
