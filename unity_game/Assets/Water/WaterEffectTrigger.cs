using UnityEngine;
using UnityEngine.Rendering;

public class WaterEffectTrigger : MonoBehaviour
{
    public Volume water; // Przypisz Volume z post-processingiem w inspektorze

    private void OnTriggerEnter(Collider other)
    {
        // Sprawdzamy, czy gracz wszedł w obszar wody (Collider, który jest Triggerem)
        if (other.gameObject == water.gameObject)
        {
            water.weight = 1; // Włącza efekt
            Debug.Log("Triggered");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Sprawdzamy, czy gracz opuścił obszar wody
        if (other.gameObject == water.gameObject)
        {
            water.weight = 0; // Wyłącza efekt
            Debug.Log("Turned off");
        }
    }
}