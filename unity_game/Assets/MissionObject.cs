using UnityEngine;
using TMPro; // U¿ywamy TextMeshPro
using UnityEngine.Events; // Przestrzeñ nazw dla eventów
using System.Collections.Generic; // Przestrzeñ nazw dla list

public class MissionObject : MonoBehaviour
{
    public enum State { Aktywny, Ukonczony } // Enum dla stanów
    public State currentState;

    public TMP_Text messageText; // Przypisanie elementu UI TextMeshPro
    public string activeMessage = "Obiekt jest aktywny!"; // Wiadomoœæ, któr¹ mo¿na ustawiæ w edytorze
    public List<UnityEvent> onCompleteEvents; // Lista eventów do wywo³ania w stanie ukoñczonym

    void Start()
    {
        UpdateState(State.Aktywny); // Ustawienie pocz¹tkowego stanu
    }

    void UpdateState(State newState)
    {
        currentState = newState;

        switch (currentState)
        {
            case State.Aktywny:
                messageText.text = activeMessage; // Wyœwietlanie aktywnej wiadomoœci
                break;
            case State.Ukonczony:
                messageText.text = ""; // Ukrycie wiadomoœci
                foreach (var unityEvent in onCompleteEvents)
                {
                    unityEvent.Invoke(); // Wywo³anie eventów
                }
                gameObject.SetActive(false); // Dezaktywacja obiektu
                break;
        }
    }

    // Publiczna metoda, która pozwala na zewnêtrzne wywo³anie zmiany stanu na ukoñczony
    public void Complete()
    {
        UpdateState(State.Ukonczony); // Przejœcie do stanu ukoñczonego
    }

    // Publiczna metoda do aktywacji obiektu
    public void Activate()
    {
        UpdateState(State.Aktywny); // Przejœcie do stanu aktywnego
    }
}

