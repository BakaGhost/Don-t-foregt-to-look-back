using UnityEngine;

public class PlayerColManager : MonoBehaviour
{
    LampeScript lampeScript;
    
    void Start()
    {
        lampeScript = GetComponentInChildren<LampeScript>();
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Debug.Log(hit.gameObject.name);
    
        if (hit.collider.CompareTag("Recharge"))
        {
            lampeScript.Recharge();
            Destroy(hit.gameObject);
        }
    }
    
}
