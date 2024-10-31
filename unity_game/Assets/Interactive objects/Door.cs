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
    public GameObject[] connectedRooms = new GameObject[2];
    public float durability = 100f;
    public float waterLevelToDMG = 0.8f;
    public float DPS = 2f;
    public bool isEntryDoor = false;

    // Zdarzenia
    public UnityEvent onOpen;
    public UnityEvent onClose;
    public UnityEvent onStartMovement;
    public UnityEvent onEndMovement;

    private bool isFloodable = false;
    private bool isFlooded = false; //Sholud it be damaged or not
    void Start(){
        InitRotation = transform.localEulerAngles;
    }

    public void Update(){
        if (isFlooded){
            if (isFloodable){
            } else {
                if (durability <= 0f || isOpen){
                    isFloodable = true;
                    foreach (GameObject room in connectedRooms){
                        if (!room.GetComponent<InteriorFloodRegion>().getIsFlooding()){
                            room.GetComponent<InteriorFloodRegion>().setIsFlooding(true);
                            break;
                        }
                    }
                }
            }
        } else {

        }
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
    private void StartFloodingRoom(){
        foreach (GameObject room in connectedRooms){
            if (!room.GetComponent<InteriorFloodRegion>().getIsFlooding()){
                room.GetComponent<InteriorFloodRegion>().setIsFlooding(true);
            }
        }
    }
}
