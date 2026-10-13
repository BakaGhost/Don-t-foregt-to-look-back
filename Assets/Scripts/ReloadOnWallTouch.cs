using UnityEngine;
using UnityEngine.SceneManagement;

// À mettre sur le joueur (le même objet que FirstPersonController, avec Rigidbody + Collider).
public class ReloadOnWallTouch : MonoBehaviour
{
    [Tooltip("Tag des objets qui provoquent le rechargement de la scène.")]
    public string wallTag = "Wall";

    [Tooltip("Délai avant le rechargement (en secondes). 0 = immédiat.")]
    public float reloadDelay = 0f;

    private bool isReloading = false;

    // Pour des murs avec un Collider normal (non Trigger)
    private void OnCollisionEnter(Collision collision)
    {
        TryReload(collision.gameObject);
    }

    // Pour des murs avec un Collider en "Is Trigger"
    private void OnTriggerEnter(Collider other)
    {
        TryReload(other.gameObject);
    }

    private void TryReload(GameObject other)
    {
        if (isReloading || !other.CompareTag(wallTag))
            return;

        isReloading = true;

        if (reloadDelay > 0f)
            Invoke(nameof(ReloadScene), reloadDelay);
        else
            ReloadScene();
    }

    private void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
