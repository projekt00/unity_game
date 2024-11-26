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
    private Vector3 initRight;
    private Vector3 initForward;
    private bool destroyed = false;
    private bool openingAnimation = false;
    private Quaternion rotation;
    private Coroutine destroyingCoroutine = null;
    void Start(){
        if (isOpen){
            transform.rotation = Quaternion.Euler(transform.eulerAngles + rotationOffset);
            rotation = transform.rotation;
            initPos = transform.position;
            initRight = transform.right;
            initForward = transform.forward;
            transform.rotation = Quaternion.Euler(transform.eulerAngles - rotationOffset);
        } else {
            rotation = transform.rotation;
            initPos = transform.position;
            initRight = transform.right;
            initForward = transform.forward;
        }
    }

    public void Update(){
        if (isOnFloodLevel){
            if (isFlooded){
                if (isFloodable){
                    if (durability > 0f && !isOpen){
                        Debug.Log("Smacked");
                        targetRoom.GetComponent<InteriorFloodRegion>().setIsFlooding(false);
                        if (sourceRoom != exteriorWater){
                            sourceRoom.GetComponent<InteriorFloodRegion>().removeFromSource(targetRoom);
                        }
                        if (targetRoom != exteriorWater){
                            targetRoom.GetComponent<InteriorFloodRegion>().removeSource(sourceRoom);
                        }
                        targetRoom = null;
                        sourceRoom = null;
                        Destroy(waterParticles);
                        waterParticles = null;
                        isFloodable = false;
                    }
                } else {
                    if (durability <= 0f || isOpen){
                        for (int i = 0; connectedRooms.Length > i; i++){
                            int i2 = (i == 0) ? 1 : 0;
                            if (!connectedRooms[i].GetComponent<InteriorFloodRegion>().startedToFlood()){
                                isFloodable = true;
                                if (!connectedRooms[i].GetComponent<InteriorFloodRegion>().getIsFlooding()){
                                    connectedRooms[i].GetComponent<InteriorFloodRegion>().setIsFlooding(true);
                                    targetRoom = connectedRooms[i];
                                    if (!isEntryDoor){
                                        sourceRoom = connectedRooms[i2];
                                        sourceRoom.GetComponent<InteriorFloodRegion>().addToSource(targetRoom);
                                    } else {
                                        sourceRoom = exteriorWater;
                                    }
                                    targetRoom.GetComponent<InteriorFloodRegion>().addSource(sourceRoom);
                                    break;
                                }
                            } else {
                                if (!isEntryDoor){
                                    if (connectedRooms[i2].transform.position.y < connectedRooms[i].transform.position.y){
                                        isFloodable = true;
                                        connectedRooms[i2].GetComponent<InteriorFloodRegion>().setIsFlooding(true);
                                        targetRoom = connectedRooms[i2];
                                        sourceRoom = connectedRooms[i];
                                        sourceRoom.GetComponent<InteriorFloodRegion>().addToSource(targetRoom);
                                        targetRoom.GetComponent<InteriorFloodRegion>().addSource(sourceRoom);
                                        break;
                                    }
                                } else {
                                    if (connectedRooms[0].transform.position.y + connectedRooms[0].GetComponent<Renderer>().bounds.extents.y * 2 <
                                        exteriorWater.transform.position.y + exteriorWater.GetComponent<Renderer>().bounds.extents.y * 2){
                                        isFloodable = true;
                                        Debug.Log("Exterior water flood");
                                        connectedRooms[0].GetComponent<InteriorFloodRegion>().setIsFlooding(true);
                                        targetRoom = connectedRooms[0];
                                        sourceRoom = exteriorWater;
                                    }
                                }
                                /*
                                if (!connectedRooms[i].GetComponent<InteriorFloodRegion>().getIsFlooding()){
                                    if (!isEntryDoor){
                                        if (connectedRooms[i2].transform.position.y > connectedRooms[i].transform.position.y){
                                            isFloodable = true;
                                            connectedRooms[i].GetComponent<InteriorFloodRegion>().setIsFlooding(true);
                                            targetRoom = connectedRooms[i];
                                            sourceRoom = connectedRooms[i2];
                                            break;
                                        }
                                    } else {
                                        if (connectedRooms[0].transform.position.y + connectedRooms[0].GetComponent<Renderer>().bounds.extents.y * 2 <
                                            exteriorWater.transform.position.y + exteriorWater.GetComponent<Renderer>().bounds.extents.y * 2){
                                            isFloodable = true;
                                            Debug.Log("Exterior water flood");
                                            connectedRooms[0].GetComponent<InteriorFloodRegion>().setIsFlooding(true);
                                            targetRoom = connectedRooms[0];
                                            sourceRoom = exteriorWater;
                                        }
                                    }
                                }
                            */
                            }
                        }
                    }
                }
                if (targetRoom != null){
                    if (targetRoom.transform.position.y + targetRoom.GetComponent<Renderer>().bounds.extents.y + 0.1f >=
                         sourceRoom.transform.position.y + sourceRoom.GetComponent<Renderer>().bounds.extents.y){
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
                            UpdateWaterParticles(sourceRoom);
                        }
                    }
                }
            } else {
                if (isEntryDoor){
                    if (exteriorWater.transform.position.y + exteriorWater.GetComponent<Renderer>().bounds.extents.y * 2 >= transform.position.y + waterLevelToDMG - GetComponent<Renderer>().bounds.extents.y){
                            isFlooded = true;
                            if (destroyingCoroutine == null){
                                destroyingCoroutine = StartCoroutine(DestroyingDoor());
                            }
                        }
                } else {
                    foreach(GameObject room in connectedRooms){
                        if (room.transform.position.y + room.GetComponent<Renderer>().bounds.extents.y >= transform.position.y + waterLevelToDMG - GetComponent<Renderer>().bounds.extents.y){
                            isFlooded = true;
                            if (destroyingCoroutine == null){
                                destroyingCoroutine = StartCoroutine(DestroyingDoor());
                            }
                        }
                    }
                }
            }
        }
    }
    public void OnInteract()
    {   
        if (openingAnimation == false){
            if (isOpen)
            {
                onStartMovement.Invoke();
                openingAnimation = true;
                transform.DORotate(transform.eulerAngles - rotationOffset, movementDuration).OnComplete(() => {
                    onClose.Invoke();
                    onEndMovement.Invoke();
                    openingAnimation = false;
                });
                isOpen = false;
            }
            else
            {
                onStartMovement.Invoke();
                openingAnimation = true;
                transform.DORotate(transform.eulerAngles + rotationOffset, movementDuration).OnComplete(() => {
                    onOpen.Invoke();
                    onEndMovement.Invoke();
                    openingAnimation = false;
                });
                isOpen = true;
            }
        }
    }
    IEnumerator DestroyingDoor(){
        while (durability > 0){
            durability -= DPS;
            Debug.Log(durability);
            if (durability <= 0 && !destroyed && !isOpen){
                Vector3 doorPosition = transform.position;
                Vector3 roomPosition;
                if (targetRoom != null){
                    roomPosition = targetRoom.transform.position;
                }
                else {
                    if (!isEntryDoor){
                        roomPosition = connectedRooms[0].transform.position.y >= connectedRooms[1].transform.position.y ?
                                        connectedRooms[0].transform.position : connectedRooms[1].transform.position;
                    } else {
                        roomPosition = connectedRooms[0].transform.position;
                    }
                }

                Vector3 directionToRoom = (roomPosition - doorPosition).normalized;


                Vector3 hitDirection = -directionToRoom;
                if (isEntryDoor){
                    hitDirection = -hitDirection;
                }

                Rigidbody rb = GetComponent<Rigidbody>();
                if (rb != null)
                {   
                    rb.useGravity = true;
                    rb.AddForce(hitDirection * 10f, ForceMode.Impulse);
                    destroyed = true;
                }
                else
                {
                    Debug.LogWarning("Drzwi nie mają komponentu Rigidbody");
                }
            }
            yield return new WaitForSeconds(1f);
        }
    }
    private void CreateWaterParticles(GameObject floodingRoom, GameObject waterSourceRoom){
        if (waterParticlesPrefab != null){
            Vector3 particlesPosition = initPos;
            Vector3 floodingRoomDirection = floodingRoom.transform.position - transform.position;
            floodingRoomDirection.Normalize();

            float dotX = Vector3.Dot(initRight, floodingRoomDirection);
            float dotZ = Vector3.Dot(initForward, floodingRoomDirection);

            if (dotX < 0.5f){
                rotation = rotation * Quaternion.Euler(180, 0, 0);
            } else if (dotZ < 0.5f){
                rotation = rotation * Quaternion.Euler(0, 0, 180);
            }

            particlesPosition.x += GetComponent<Renderer>().bounds.extents.x;
            particlesPosition.z += GetComponent<Renderer>().bounds.extents.z;
            waterParticles = Instantiate(waterParticlesPrefab, particlesPosition, rotation);
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
            newPosition.y = waterSourceRoom.transform.position.y + waterSourceRoom.GetComponent<Renderer>().bounds.extents.y - waterParticles.transform.localScale.y/2;
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
