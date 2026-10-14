using System;
using UnityEngine;

public class PlayerColManager : MonoBehaviour
{
    [HideInInspector]public LampeScript lampeScript;
    
    void Start()
    {
        lampeScript = GetComponentInChildren<LampeScript>();
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
         
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Monster"))
        {
            Debug.Log(other.gameObject.name);
            lampeScript.Discharge();
        }   
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Recharge"))
        {
            lampeScript.Recharge();
            Destroy(other.gameObject);
        }
    }
}
