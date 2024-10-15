using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Door : MonoBehaviour, InteractiveObjects
{
    public bool isOpen = false;
    private Vector3 InitRotation;

    void Start(){
        InitRotation = transform.localEulerAngles;
    }
    public void OnInteract(){
        if (isOpen){
            transform.DORotate(InitRotation, 1f);
            isOpen = false;
        } else {
            transform.DORotate(InitRotation + new Vector3(0, 90, 0), 1f);
            isOpen = true;
        }
    }
}
