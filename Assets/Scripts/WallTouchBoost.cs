using UnityEngine;

// À mettre sur le joueur (même objet que FirstPersonController).
public class WallTouchBoost : MonoBehaviour
{
    [Tooltip("Le gros cube à accélérer.")]
    public MovingCube cube;

    [Tooltip("Tag des murs qui accélèrent le cube.")]
    public string wallTag = "Wall";

    [Tooltip("Temps minimum entre deux accélérations (évite les boosts en rafale quand on frotte un mur).")]
    public float cooldown = 0.5f;

    private float lastBoostTime = -999f;

    private void OnCollisionEnter(Collision collision)
    {
        TryBoost(collision.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        TryBoost(other.gameObject);
    }

    private void TryBoost(GameObject other)
    {
        if (cube == null || !other.CompareTag(wallTag))
            return;

        if (Time.time - lastBoostTime < cooldown)
            return;

        lastBoostTime = Time.time;
        cube.Boost();
    }
}
