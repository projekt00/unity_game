using UnityEngine;
using UnityEngine.Events;

public class BoxTrigger : MonoBehaviour
{
    // Referencja do konkretnego obiektu, na który pud³o ma reagowaæ
    public GameObject specificTrigger; // Przypisz obiekt w Inspektorze

    // Event do wywo³ania, gdy pud³o trafi na konkretny trigger
    public UnityEvent onSpecificTriggerActivated;

    // Funkcja wywo³ywana automatycznie przez Unity, gdy obiekt wchodzi w trigger
    private void OnTriggerEnter(Collider other)
    {
        // Sprawdzanie, czy obiekt, z którym kolidujemy, jest tym konkretnym triggerem
        if (other.gameObject == specificTrigger) // Porównujemy z konkretnym obiektem
        {
            // Wywo³anie eventu, gdy pud³o wchodzi w obszar konkretnego triggera
            if (onSpecificTriggerActivated != null)
            {
                onSpecificTriggerActivated.Invoke();
            }
        }
    }
}
