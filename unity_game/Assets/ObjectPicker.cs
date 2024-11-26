using System.Collections;
using System.Collections.Generic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPicker : MonoBehaviour
{
    public float pickupRange = 3f; // Zasi�g, w kt�rym mo�emy podnie�� obiekt
    public Transform holdPosition; // Pozycja, w kt�rej trzymamy obiekt
    public float smoothSpeed = 10f; // Szybko�� przesuwania obiektu

    private GameObject pickedObject; // Obiekt, kt�ry jest trzymany
    private UniversalObjectController objectController; // Kontroler interakcji obiektu
    private Collider pickedObjectCollider; // Referencja do komponentu Collider trzymanego obiektu

    private int targetLayer = 10; // Numer warstwy, na kt�rej musz� znajdowa� si� obiekty

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Lewy przycisk myszy
        {
            TryPickupObject();
        }

        if (pickedObject != null)
        {
            MoveObjectWithPlayer();
        }

        if (Input.GetMouseButtonDown(1)) // Prawy przycisk myszy
        {
            DropObject(); // Upuszczamy obiekt
        }
    }

    void TryPickupObject()
    {
        // Rzucenie promienia, aby sprawdzi�, czy jest w pobli�u obiekt do podniesienia
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, pickupRange))
        {
            // Sprawdzamy, czy trafili�my w obiekt z kontrolerem
            UniversalObjectController controller = hit.collider.GetComponent<UniversalObjectController>();
            if (controller != null && controller.interactionType == UniversalObjectController.InteractionType.Clickable)
            {
                // Sprawdzamy, czy obiekt znajduje si� w wymaganej warstwie (10)
                if (hit.collider.gameObject.layer == targetLayer)
                {
                    // Sprawdzamy, czy obiekt nie jest ju� podniesiony
                    if (pickedObject == null)
                    {
                        pickedObject = hit.collider.gameObject;
                        objectController = controller;

                        // Wy��czamy fizyk� i kolizje obiektu
                        Rigidbody rb = pickedObject.GetComponent<Rigidbody>();
                        if (rb != null)
                        {
                            rb.isKinematic = true;
                        }
                        pickedObjectCollider = pickedObject.GetComponent<Collider>();
                        if (pickedObjectCollider != null)
                        {
                            pickedObjectCollider.enabled = false;
                        }
                    }
                }
            }
        }
    }

    void MoveObjectWithPlayer()
    {
        // Przemieszczamy obiekt do pozycji "trzymania"
        if (pickedObject != null)
        {
            Vector3 targetPosition = holdPosition.position;
            pickedObject.transform.position = Vector3.Lerp(pickedObject.transform.position, targetPosition, smoothSpeed * Time.deltaTime);
            pickedObject.transform.rotation = Quaternion.Lerp(pickedObject.transform.rotation, holdPosition.rotation, smoothSpeed * Time.deltaTime);
        }
    }

    public void DropObject()
    {
        if (pickedObject != null)
        {
            // Przywracamy fizyk� i kolizje obiektu
            Rigidbody rb = pickedObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;
            }
            if (pickedObjectCollider != null)
            {
                pickedObjectCollider.enabled = true;
            }

            pickedObject = null;
            objectController = null;
            pickedObjectCollider = null;
        }
    }

    // Ta metoda pozwala innym skryptom uzyska� dost�p do aktualnie trzymanego obiektu
    public GameObject GetPickedObject()
    {
        return pickedObject;
    }
}
