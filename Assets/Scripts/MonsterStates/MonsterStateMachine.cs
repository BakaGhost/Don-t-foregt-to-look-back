using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

#if UNITY_EDITOR
using UnityEditor;
#endif

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
    [SerializeField] private float maxRayDistance = 10f;
    [SerializeField] private float sphereRadius = 2f;
    [SerializeField] private float angle = 60f;
    [SerializeField] private float maxAngle = 360f;
    [SerializeField] private LayerMask hitLayers;
    private float usedRayDistance;
    private float usedRayAngle;
    
    [Header("States")]
    MonsterState currentState;
    MonsterState previousState;
    MonsterState nextState;
    public MonsterState firstState;
    MonsterState[] states;
    
    [Header("NavMesh")]
    [HideInInspector] public NavMeshAgent myAgent;
    
    

    void Start()
    {
        usedRayAngle = angle;
        usedRayDistance = rayDistance;
        myAgent = GetComponent<NavMeshAgent>();
        states = GetComponentsInChildren<MonsterState>();
        
        if (states.Length < 2)
        {
            Debug.LogError("Attention : Il manque des états (WalkingState/PursuitState) sur ce GameObject ou ses enfants !");
            return;
        }
        
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

    void Update()
    {
        if (currentState == null) return; 
        
        CheckRaycast();
        currentState.UpdateState();
    }
    
    public void CheckRaycast()
    {
        Vector3 origin = transform.position;
        Vector3 rayDirection = transform.forward;
        
        Collider[] hits = Physics.OverlapSphere(origin, usedRayDistance, hitLayers);   
        
        bool playerInSight = false;
        
        foreach (Collider hitCollider in hits)
        {
            Vector3 targetPos = hitCollider.transform.position; 
            if (targetPos == origin) continue;
            
            Vector3 directionToObject = (targetPos - origin).normalized;
            float angleToObject = Vector3.Angle(rayDirection, directionToObject);

            if (angleToObject <= usedRayAngle && hitCollider.gameObject.CompareTag("Player"))
            {
                playerInSight = true;
                break;
            }
        }

        if (playerInSight && currentState != states[1])
        {
            usedRayDistance = maxRayDistance;
            usedRayAngle = maxAngle;
            Transition(states[1]);
        }
        else if (!playerInSight && currentState == states[1])
        {
            usedRayDistance = rayDistance;
            usedRayAngle = angle;
            Transition(states[0]);
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
    public void TriggerStun()
    {
        if (currentState != states[2])
        {
            Transition(states[2]); 
        }
    }
    public void ReleaseStun()
    {
        if (currentState == states[2])
        {
            Transition(states[0]);
        }
    }
    
    
    private void OnDrawGizmosSelected()
    {
        Vector3 origin = transform.position;
        Vector3 direction = transform.forward;

        Gizmos.color = new Color(1, 0, 0, 0.2f);
        Gizmos.DrawWireSphere(origin, usedRayDistance);

#if UNITY_EDITOR
        // CORRECTION : Ajout des signes "*" pour la multiplication des vecteurs
        Vector3 leftBoundary = Quaternion.Euler(0, -usedRayAngle, 0) * direction;
        Vector3 rightBoundary = Quaternion.Euler(0, usedRayAngle, 0) * direction;

        Handles.color = new Color(1f, 0.92f, 0.016f, 0.2f);
        Handles.DrawSolidArc(origin, Vector3.up, leftBoundary, usedRayAngle * 2, usedRayDistance);

        Handles.color = Color.yellow;
        Handles.DrawWireArc(origin, Vector3.up, leftBoundary, usedRayAngle * 2, usedRayDistance);

        // CORRECTION : Ajout des signes "*" pour la multiplication des distances
        Handles.DrawLine(origin, origin + leftBoundary * usedRayDistance);
        Handles.DrawLine(origin, origin + rightBoundary * usedRayDistance);

        GUIStyle textStyle = new GUIStyle();
        textStyle.normal.textColor = Color.white;
        textStyle.alignment = TextAnchor.MiddleCenter;
        textStyle.fontStyle = FontStyle.Bold;
        
        Handles.Label(origin + Vector3.up * 2.5f, "Angle actuel : " + usedRayAngle + "°", textStyle);
#endif
    }
}