using UnityEngine;

public class PlayerColManager : MonoBehaviour
{
    LampeScript lampeScript;
    
    void Start()
    {
        lampeScript = GetComponentInChildren<LampeScript>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Recharge")
        {
            lampeScript.Recharge();
            Destroy(collision.gameObject);
        }
    }
    
}
