using UnityEngine;

// À mettre sur le joueur (même objet que FirstPersonController).
public class CameraRotationBoost : MonoBehaviour
{
    [Tooltip("Le gros cube à accélérer.")]
    public MovingCube cube;

    [Tooltip("La caméra du joueur (la même que dans FirstPersonController).")]
    public Camera playerCamera;

    [Tooltip("Vitesse ajoutée au cube par degré tourné. Ex: 0.002 -> un tour complet (360°) = +0.72.")]
    public float speedPerDegree = 0.002f;

    private Quaternion lastRotation;

    private void Start()
    {
        if (playerCamera == null)
            playerCamera = GetComponentInChildren<Camera>();

        lastRotation = playerCamera.transform.rotation;
    }

    // LateUpdate : après que FirstPersonController a tourné la caméra dans Update.
    private void LateUpdate()
    {
        if (cube == null || playerCamera == null)
            return;

        Quaternion currentRotation = playerCamera.transform.rotation;
        float angle = Quaternion.Angle(lastRotation, currentRotation);
        lastRotation = currentRotation;

        if (angle > 0f)
            cube.Boost(angle * speedPerDegree);
    }
}
