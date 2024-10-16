using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

public class Door : MonoBehaviour
{
    public bool isOpen = false;
    private Vector3 InitRotation;

    // Publiczny wektor obrotu
    public Vector3 rotationOffset = new Vector3(0, 90, 0);

    // Publiczna pr�dko�� ruchu
    public float movementDuration = 1f;

    // Zdarzenia
    public UnityEvent onOpen;
    public UnityEvent onClose;
    public UnityEvent onStartMovement;
    public UnityEvent onEndMovement;

    void Start(){
        InitRotation = transform.localEulerAngles;
    }
    public void OnInteract()
    {   
        if (isOpen)
        {
            onStartMovement.Invoke();
            transform.DORotate(InitRotation, movementDuration).OnComplete(() => {
                onClose.Invoke();
                onEndMovement.Invoke();
            });
            isOpen = false;
        }
        else
        {
            onStartMovement.Invoke();
            transform.DORotate(InitRotation + rotationOffset, movementDuration).OnComplete(() => {
                onOpen.Invoke();
                onEndMovement.Invoke();
            });
            isOpen = true;
        }
    }
}
