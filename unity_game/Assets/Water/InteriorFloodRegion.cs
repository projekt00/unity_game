using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class InteriorFloodRegion : MonoBehaviour
{
    public List<GameObject> neighbourRooms = new List<GameObject>(); //Rooms connected to this room

    private bool isFlooding = false;
    private bool isWaterLevelRising = false;
    private RisingWater risingWaterScript;
    private float initWaterLevel;
    private List<GameObject> source = new List<GameObject>();
    private List<GameObject> sourceTo = new List<GameObject>(); 
    private List<GameObject> equalTo = new List<GameObject>();
    void Start()
    {
        risingWaterScript = GetComponent<RisingWater>();
        initWaterLevel = transform.position.y;
    }
    void Update()
    {
        if (neighbourRooms.Count > 0){
            if (isFlooding) {
                if (isWaterLevelRising){
                    foreach (GameObject room in sourceTo){
                        if (!equalTo.Contains(room)){
                            //??         
                            if (transform.position.y  > room.transform.position.y){
                                isWaterLevelRising = false;
                                risingWaterScript.StopRising();
                                foreach (GameObject erom in equalTo){
                                    erom.GetComponent<InteriorFloodRegion>().equalToRemove(gameObject);
                                }
                                equalTo = new List<GameObject>();
                                /*
                                Vector3 newPos = room.transform.position;
                                newPos.y = transform.position.y;
                                room.transform.position = newPos;
                                */
                                break;
                            }
                        }       
                    }
                    if (isWaterLevelRising){
                        foreach (GameObject room in source){
                            if (!equalTo.Contains(room)){
                                if (transform.position.y + transform.GetComponent<Renderer>().bounds.extents.y * 2 > room.transform.position.y + room.transform.GetComponent<Renderer>().bounds.extents.y * 2){
                                    isWaterLevelRising = false;
                                    risingWaterScript.StopRising(room.transform.position.y);
                                    isFlooding = false;
                                    if (!equalTo.Contains(room)){
                                        equalTo.Add(room);
                                        if (!room.GetComponent<InteriorFloodRegion>().equalToContains(room)){
                                            room.GetComponent<InteriorFloodRegion>().equalToAdd(gameObject);
                                        }
                                    }
                                    /*
                                    Vector3 newPos = transform.position;
                                    newPos.y = room.transform.position.y;
                                    transform.position = newPos;
                                    isFlooding = false;
                                    */
                                    break;
                                }
                            }
                        }
                    }
                } else {
                    /*
                    if (shouldBeEqual.Count > 0){
                        foreach (GameObject room in shouldBeEqual){
                            Debug.Log(room.name + " " + gameObject.name);
                            if (room.transform.position.y == transform.position.y){
                                break;
                            } else {
                                Vector3 newPos = room.transform.position;
                                newPos.y = transform.position.y;
                                room.transform.position = newPos;
                            }
                        }
                    }
                    */
                    isWaterLevelRising = true;
                    foreach (GameObject room in neighbourRooms){
                        if (room.GetComponent<InteriorFloodRegion>().getIsFlooding() || room.GetComponent<InteriorFloodRegion>().startedToFlood()){
                            if (Math.Floor(room.transform.position.y * 100) / 100 < Math.Floor(transform.position.y * 100) / 100 && !equalTo.Contains(room)){
                                isWaterLevelRising = false;
                            }
                        }
                            
                    }
                    if (isWaterLevelRising){
                        risingWaterScript.StartRising();
                        foreach (GameObject erom in equalTo){
                            erom.GetComponent<InteriorFloodRegion>().risingWaterScript.StartRising();
                            erom.GetComponent<InteriorFloodRegion>().setIsWaterLevelRising(true);
                            erom.GetComponent<InteriorFloodRegion>().setIsFlooding(true);
                        }
                    }
                }
            } else {
                risingWaterScript.StopRising();
                isWaterLevelRising = false;
            }
        }
    }

    public bool getIsFlooding(){
        return isFlooding;
    }
    public void setIsFlooding(bool value){
        isFlooding = value;
    }
    public bool startedToFlood(){
        return transform.position.y > initWaterLevel;
    }
    public void addToSource(GameObject sourceToRoom){
        if (!sourceTo.Contains(sourceToRoom)){
            sourceTo.Add(sourceToRoom);
        }
    }
    public void removeFromSource(GameObject sourceToRoom){
        sourceTo.Remove(sourceToRoom);
    }
    public void addSource(GameObject sourceRoom){
        if (!source.Contains(sourceRoom)){
            source.Add(sourceRoom);
        }
    }
    public void removeSource(GameObject sourceRoom){
        source.Remove(sourceRoom);
    }
    public bool equalToContains(GameObject room){
        return equalTo.Contains(room);
    }
    public void equalToAdd(GameObject room){
        equalTo.Add(room);
    }
    public void setIsWaterLevelRising(bool val){
        isWaterLevelRising = val;
    }
    public void equalToRemove(GameObject room){
        equalTo.Remove(room);
    }
}
