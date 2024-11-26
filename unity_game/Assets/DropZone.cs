using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Events;

public class DropZone : MonoBehaviour
{
    public Transform dropPosition; // Pozycja, na kt�rej obiekt ma zosta� odstawiony
    public string requiredTag = "Pickable"; // Tag wymagany, by obiekt m�g� zosta� odstawiony
    public UnityEvent onObjectPlaced; // Zdarzenie wywo�ywane po umieszczeniu obiektu
    public bool isOccupied = false; // Czy strefa jest ju� zaj�ta

    private void OnMouseEnter()
    {
        if (!isOccupied)
        {
            // Opcjonalnie: wy�wietl komunikat lub pod�wietl stref�
        }
    }

    private void OnMouseExit()
    {
        // Opcjonalnie: ukryj komunikat lub usu� pod�wietlenie
    }

    private void OnMouseDown()
    {
        if (!isOccupied)
        {
            ObjectPicker picker = FindObjectOfType<ObjectPicker>();
            if (picker != null)
            {
                GameObject pickedObject = picker.GetPickedObject();
                if (pickedObject != null && pickedObject.CompareTag(requiredTag))
                {
                    PlaceObject(pickedObject, picker);
                }
            }
        }
    }

    private void PlaceObject(GameObject pickedObject, ObjectPicker picker)
    {
        // Ustaw obiekt w pozycji dropPosition
        pickedObject.transform.position = dropPosition.position;
        pickedObject.transform.rotation = dropPosition.rotation;

        // Przywr�� fizyk� obiektu
        Rigidbody rb = pickedObject.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
        }

        // Oznacz stref� jako zaj�t�
        isOccupied = true;

        // Upu�� obiekt
        picker.DropObject();

        // Wywo�aj zdarzenie onObjectPlaced
        onObjectPlaced?.Invoke();
    }
}

