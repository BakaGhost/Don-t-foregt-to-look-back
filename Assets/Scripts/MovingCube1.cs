using UnityEngine;
using UnityEngine.SceneManagement;

// À mettre sur le gros cube (un Rigidbody sera ajouté automatiquement).
[RequireComponent(typeof(Rigidbody))]
public class MovingCube : MonoBehaviour
{
    [Header("Déplacement")]
    [Tooltip("Axe de déplacement (en monde). Ex: (0,0,1) = avance sur Z, (-1,0,0) = recule sur X.")]
    public Vector3 direction = Vector3.forward;

    [Tooltip("Vitesse de départ (unités par seconde).")]
    public float baseSpeed = 2f;

    [Header("Accélération")]
    [Tooltip("Vitesse ajoutée à chaque appel de Boost().")]
    public float speedBoostPerTouch = 1f;

    [Tooltip("Vitesse maximale que le cube ne dépassera pas.")]
    public float maxSpeed = 15f;

    [Tooltip("Rapidité avec laquelle le cube rejoint sa nouvelle vitesse (plus grand = plus brusque).")]
    public float acceleration = 5f;

    [Header("Rechargement")]
    [Tooltip("Tag du joueur. Si le cube le touche, la scène est rechargée.")]
    public string playerTag = "Player";

    [Tooltip("Délai avant le rechargement (en secondes). 0 = immédiat.")]
    public float reloadDelay = 0f;

    private Rigidbody rb;
    private float currentSpeed;
    private float targetSpeed;
    private bool isReloading = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.interpolation = RigidbodyInterpolation.Interpolate;

        currentSpeed = baseSpeed;
        targetSpeed = baseSpeed;
    }

    private void FixedUpdate()
    {
        currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, acceleration * Time.fixedDeltaTime);
        rb.MovePosition(rb.position + direction.normalized * currentSpeed * Time.fixedDeltaTime);
    }

    // Appelé par WallTouchBoost quand le joueur touche un mur.
    public void Boost()
    {
        Boost(speedBoostPerTouch);
    }

    // Appelé par CameraRotationBoost (petites accélérations continues).
    public void Boost(float amount)
    {
        targetSpeed = Mathf.Min(targetSpeed + amount, maxSpeed);
    }

    // Si le Collider du cube est normal
    private void OnCollisionEnter(Collision collision)
    {
        TryReload(collision.gameObject);
    }

    // Si le Collider du cube est en "Is Trigger"
    private void OnTriggerEnter(Collider other)
    {
        TryReload(other.gameObject);
    }

    private void TryReload(GameObject other)
    {
        if (isReloading || !other.CompareTag(playerTag))
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
