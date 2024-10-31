using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;
using UnityEngine.Events; // Importujemy przestrzeñ nazw dla eventów

public class TriggerEvent : MonoBehaviour
{
    public UnityEvent onPlayerEnter; // Event wywo³ywany przy wejœciu gracza

    // Metoda wywo³ywana przy wejœciu gracza do triggera
    private void OnTriggerEnter(Collider other)
    {
        // Sprawdzenie, czy obiekt, który wszed³ w strefê, jest graczem
        if (other.CompareTag("Player")) // Upewnij siê, ¿e obiekt gracza ma tag "Player"
        {
            onPlayerEnter.Invoke(); // Wywo³anie eventu
        }
    }
}
