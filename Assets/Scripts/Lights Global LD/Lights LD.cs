using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Light))]
public class LightsLD : MonoBehaviour
{
    [Header("Temps entre les séries de coupures")]
    [Min(0.01f)] public float intervalleMin = 1f;
    [Min(0.01f)] public float intervalleMax = 5f;

    [Header("Nombre de coupures par série")]
    [Min(1)] public int coupuresMin = 1;
    [Min(1)] public int coupuresMax = 5;

    [Header("Durée des coupures")]
    [Min(0.01f)] public float dureeMin = 0.03f;
    [Min(0.01f)] public float dureeMax = 0.25f;

    private Light lumiere;
    private Coroutine routine;
    private bool etatInitial;

    private void Awake()
    {
        lumiere = GetComponent<Light>();
        etatInitial = lumiere.enabled;
    }

    private void OnEnable()
    {
        routine = StartCoroutine(FauxContact());
    }

    private IEnumerator FauxContact()
    {
        while (true)
        {
            // Attend un délai différent avant chaque série.
            yield return new WaitForSeconds(
                Random.Range(intervalleMin, intervalleMax)
            );

            int nombreCoupures = Random.Range(
                coupuresMin, coupuresMax + 1
            );

            for (int i = 0; i < nombreCoupures; i++)
            {
                lumiere.enabled = false;

                yield return new WaitForSeconds(
                    Random.Range(dureeMin, dureeMax)
                );

                lumiere.enabled = etatInitial;

                // Bref retour de la lumière entre deux coupures.
                yield return new WaitForSeconds(
                    Random.Range(0.03f, 0.2f)
                );
            }
        }
    }

    private void OnDisable()
    {
        if (routine != null)
            StopCoroutine(routine);

        lumiere.enabled = etatInitial;
    }

    private void OnValidate()
    {
        intervalleMax = Mathf.Max(intervalleMin, intervalleMax);
        coupuresMax = Mathf.Max(coupuresMin, coupuresMax);
        dureeMax = Mathf.Max(dureeMin, dureeMax);
    }
}