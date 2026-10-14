using System;
using UnityEngine;

public class PlayerColManager : MonoBehaviour
{
    LampeScript lampeScript;
    
    void Start()
    {
        lampeScript = GetComponentInChildren<LampeScript>();
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
