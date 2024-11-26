using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Cork : MonoBehaviour
{
    public List<GameObject> interiorWaters = new List<GameObject>();
    public GameObject exteriorWater;

    public bool allTasksDone = false;
    private int phase = 0;
    private Rigidbody rb;

    void Update()
    {
        if (phase == 1){
            CheckWaterLevels();
        }
    }
    public void CheckWaterLevels(){
        foreach(GameObject room in interiorWaters){
            if (room.transform.position.y + room.GetComponent<Renderer>().bounds.extents.y >=
                exteriorWater.transform.position.y + exteriorWater.GetComponent<Renderer>().bounds.extents.y){
                    phase = 2;
                    DecreaseInteriorWater();
                    break;
                }
        }
    }
    public void onInteract(){
        if (allTasksDone && phase == 0){
            transform.DOShakePosition(5f, 0.1f, 30, 90f).
            OnComplete(() => {
                exteriorWater.GetComponent<RisingWater>().DecreaseWater();
                phase = 1;
                if (GetComponent<Rigidbody>() == null){
                    rb = gameObject.AddComponent<Rigidbody>();
                    rb.useGravity = true;
                }
                rb.AddForce(Vector3.up * 20f, ForceMode.Impulse);
            });           
        }
    }
    public void DecreaseInteriorWater(){
        foreach (GameObject room in interiorWaters){
            room.GetComponent<InteriorFloodRegion>().neighbourRooms = new List<GameObject>();
            room.GetComponent<RisingWater>().DecreaseWater();
        }
    }
}
