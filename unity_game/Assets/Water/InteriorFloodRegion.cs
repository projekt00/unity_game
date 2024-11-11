using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class InteriorFloodRegion : MonoBehaviour
{
    public List<GameObject> neighbourRooms = new List<GameObject>(); //Rooms connected to this room

    private bool isFlooding = false;
    private bool isWaterLevelRising = false;
    private RisingWater risingWaterScript;
    private float initWaterLevel;
    void Start()
    {
        risingWaterScript = GetComponent<RisingWater>();
        initWaterLevel = transform.position.y;
    }
    void Update()
    {
        if (neighbourRooms.Count > 0){
            if (isFlooding) {
                foreach (GameObject room in neighbourRooms){
                    if (isWaterLevelRising){
                        if ((room.GetComponent<InteriorFloodRegion>().getIsFlooding() || room.GetComponent<InteriorFloodRegion>().startedToFlood()) && transform.position.y + 0.1f > room.transform.position.y){
                            isWaterLevelRising = false;
                            risingWaterScript.StopRising();
                            //Debug.Log("Stop rising");
                            break;
                        }
                    } else {
                        isWaterLevelRising = true;
                        if ((room.GetComponent<InteriorFloodRegion>().getIsFlooding() || room.GetComponent<InteriorFloodRegion>().startedToFlood()) && transform.position.y + 0.1f > room.transform.position.y){
                            isWaterLevelRising = false;
                            risingWaterScript.StopRising();
                            //Debug.Log("Stop rising");
                            break;
                        }
                    }
                }
                if (isWaterLevelRising){
                    risingWaterScript.StartRising();
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
}
