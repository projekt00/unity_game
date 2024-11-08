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
    public bool isOnFloodLevel = false;
    public GameObject waterParticlesPrefab;
    public bool isEntryDoor = false;
    [Header("Only if isEntryDoor = true")]
    public GameObject exteriorWater;
    // Zdarzenia
    public UnityEvent onOpen;
    public UnityEvent onClose;
    public UnityEvent onStartMovement;
    public UnityEvent onEndMovement;
    private GameObject waterParticles;

    private bool isFloodable = false;
    private GameObject targetRoom;
    private GameObject sourceRoom;
    private bool isFlooded = false; //Sholud it be damaged or not
    private Vector3 initPos;
    void Start(){
        InitRotation = transform.localEulerAngles;
        initPos = transform.position;
    }

    public void Update(){
        if (isOnFloodLevel){
            if (isFlooded){
                if (isFloodable){
                    if (durability > 0f && !isOpen){
                        targetRoom.GetComponent<InteriorFloodRegion>().setIsFlooding(false);
                        targetRoom = null;
                        sourceRoom = null;
                        Destroy(waterParticles);
                        waterParticles = null;
                    }
                } else {
                    if (durability <= 0f || isOpen){
                        isFloodable = true;
                        foreach (GameObject room in connectedRooms){
                            if (!room.GetComponent<InteriorFloodRegion>().getIsFlooding()){
                                room.GetComponent<InteriorFloodRegion>().setIsFlooding(true);
                                targetRoom = room;
                                if (!isEntryDoor){
                                    if (connectedRooms[0] == room){
                                        sourceRoom = connectedRooms[1];
                                    } else if(connectedRooms[1] == room){
                                        sourceRoom = connectedRooms[0];
                                    }
                                    break;
                                } else {
                                    sourceRoom = exteriorWater;
                                }
                            }
                        }
                    }
                }
                if (targetRoom != null){
                    if (targetRoom.transform.position.y + 0.1f >= sourceRoom.transform.position.y){
                        if (waterParticles != null){
                            Destroy(waterParticles);
                            waterParticles = null;
                            Debug.Log("Particles destroyed");
                        }
                    } else {
                        if (waterParticles == null){
                            Debug.Log("Created particles");
                            CreateWaterParticles(targetRoom, sourceRoom);
                        } else {
                            Debug.Log("Particles update");
                            UpdateWaterParticles(sourceRoom);
                        }
                    }
                }
            } else {
                if (isEntryDoor){
                    if (exteriorWater.transform.position.y + exteriorWater.GetComponent<Renderer>().bounds.extents.y * 2 >= transform.position.y + waterLevelToDMG - GetComponent<Renderer>().bounds.extents.y){
                            isFlooded = true;
                            StartCoroutine(DestroyingDoor());
                        }
                } else {
                    foreach(GameObject room in connectedRooms){
                        if (room.transform.position.y + room.GetComponent<Renderer>().bounds.extents.y >= transform.position.y + waterLevelToDMG - GetComponent<Renderer>().bounds.extents.y){
                            isFlooded = true;
                            StartCoroutine(DestroyingDoor());
                        }
                    }
                }
            }
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
    IEnumerator DestroyingDoor(){
        durability -= DPS;
        yield return new WaitForSeconds(1f);
    }
    private void CreateWaterParticles(GameObject floodingRoom, GameObject waterSourceRoom){
        if (waterParticlesPrefab != null){
            Vector3 particlesPosition = initPos;
            particlesPosition.x += GetComponent<Renderer>().bounds.extents.x;
            particlesPosition.z += GetComponent<Renderer>().bounds.extents.z;
            waterParticles = Instantiate(waterParticlesPrefab, particlesPosition, Quaternion.identity);
            //waterParticles.transform.LookAt(floodingRoom.transform);
            UpdateWaterParticles(waterSourceRoom);
        } else {
            Debug.Log("You didn't assign the water particles prefab");
        }
    }
    private void UpdateWaterParticles(GameObject waterSourceRoom){
        if (waterParticles != null){
            float praticlesHeight = (waterSourceRoom.transform.position.y + waterSourceRoom.GetComponent<Renderer>().bounds.extents.y)
                                    - (transform.position.y - GetComponent<Renderer>().bounds.extents.y); 
            Vector3 newScale = waterParticles.transform.localScale;
            Vector3 newPosition = waterParticles.transform.position;
            newScale.y = praticlesHeight/2;
            waterParticles.transform.localScale = newScale;
            //newPosition.y = waterSourceRoom.transform.position.y + waterSourceRoom.GetComponent<Renderer>().bounds.extents.y - praticlesHeight/2;
            newPosition.y = waterSourceRoom.transform.position.y + waterSourceRoom.GetComponent<Renderer>().bounds.extents.y - waterParticles.GetComponent<Renderer>().bounds.extents.y/2;
            /*
            if (newPosition.y + waterParticles.transform.localScale.y > transform.position.y + GetComponent<Renderer>().bounds.extents.y){ //if watersource is higher than doors
                newPosition.y = transform.position.y;
                waterParticles.transform.localScale = transform.localScale;
            }
            */
            waterParticles.transform.position = newPosition;
        } else {
            Debug.Log("Water particles haven't been created");
        }
    }
}
