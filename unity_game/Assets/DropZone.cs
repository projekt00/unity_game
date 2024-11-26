using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Events;

public class DropZone : MonoBehaviour
{
    public Transform dropPosition; // Pozycja, na której obiekt ma zostaæ odstawiony
    public string requiredTag = "Pickable"; // Tag wymagany, by obiekt móg³ zostaæ odstawiony
    public UnityEvent onObjectPlaced; // Zdarzenie wywo³ywane po umieszczeniu obiektu
    public bool isOccupied = false; // Czy strefa jest ju¿ zajêta

    private void OnMouseEnter()
    {
        if (!isOccupied)
        {
            // Opcjonalnie: wyœwietl komunikat lub podœwietl strefê
        }
    }

    private void OnMouseExit()
    {
        // Opcjonalnie: ukryj komunikat lub usuñ podœwietlenie
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

        // Przywróæ fizykê obiektu
        Rigidbody rb = pickedObject.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
        }

        // Oznacz strefê jako zajêt¹
        isOccupied = true;

        // Upuœæ obiekt
        picker.DropObject();

        // Wywo³aj zdarzenie onObjectPlaced
        onObjectPlaced?.Invoke();
    }
}

