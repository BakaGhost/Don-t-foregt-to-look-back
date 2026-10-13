using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class LampeScript : MonoBehaviour
{
    public Light lampLight;

    private InputActionMap playerMap;
    public InputActionReference toggleLamp;
    
    [Header("Paramètres de ciblage (Cône de détection)")]
    [SerializeField] private float rayDistance = 10f;
    [SerializeField] private float sphereRadius = 2f;
    [SerializeField] private float maxAngle = 60f;
    [SerializeField] private LayerMask hitLayers;

    public bool lampActivated = true;
    
    private void OnEnable()
    {
        toggleLamp.action.performed += OnToggleLamp;
        toggleLamp.action.Enable();
    }

    private void OnDisable()
    {
        toggleLamp.action.performed -= OnToggleLamp;
        toggleLamp.action.Disable();
    }

    void Start()
    {
        lampLight = GetComponentInChildren<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        if (lampActivated) CheckRaycast();
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
                Debug.Log("hit " +  hit.collider.name);
            }
        }
    }

    private void OnToggleLamp(InputAction.CallbackContext context)
    {
        if (lampLight != null)
        {
            lampLight.enabled = !lampLight.enabled;
            lampActivated = !lampActivated;
        }
    }
    
    private void OnDrawGizmosSelected()
    {
        // 1. Dessiner le SphereCast (bleu)
        Gizmos.color = new Color(0, 1, 1, 0.5f);
        Vector3 origin = transform.position;
        Vector3 direction = transform.forward;

        Gizmos.DrawWireSphere(origin, sphereRadius);
        Vector3 endPosition = origin + direction * rayDistance;
        Gizmos.DrawWireSphere(endPosition, sphereRadius);
    
        Gizmos.DrawLine(origin + transform.up * sphereRadius, endPosition + transform.up * sphereRadius);
        Gizmos.DrawLine(origin - transform.up * sphereRadius, endPosition - transform.up * sphereRadius);
        Gizmos.DrawLine(origin + transform.right * sphereRadius, endPosition + transform.right * sphereRadius);
        Gizmos.DrawLine(origin - transform.right * sphereRadius, endPosition - transform.right * sphereRadius);

        // 2. Dessiner le cône de vision (jaune)
        Gizmos.color = Color.yellow;
    
        // Calcule les directions extrêmes gauche et droite basées sur maxAngle
        Quaternion leftRayRotation = Quaternion.AngleAxis(-maxAngle, Vector3.up);
        Quaternion rightRayRotation = Quaternion.AngleAxis(maxAngle, Vector3.up);
    
        Vector3 leftRayDirection = leftRayRotation * direction;
        Vector3 rightRayDirection = rightRayRotation * direction;

        // Calcule les points d'arrivée exacts des deux rayons
        Vector3 leftEndPosition = origin + leftRayDirection * rayDistance;
        Vector3 rightEndPosition = origin + rightRayDirection * rayDistance;

        // Dessine les lignes partant du centre
        Gizmos.DrawLine(origin, leftEndPosition);
        Gizmos.DrawLine(origin, rightEndPosition);
    
        // Relie les deux extrémités pour fermer le triangle
        Gizmos.DrawLine(leftEndPosition, rightEndPosition);
    }
}
