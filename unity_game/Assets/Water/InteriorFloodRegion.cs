using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteriorFloodRegion : MonoBehaviour
{
    public List<GameObject> neighbourRooms = new List<GameObject>(); //Rooms connected to this room
    public RisingWater risingWaterScript;
    private bool isFlooding = false;
    private bool isWaterLevelRising = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (neighbourRooms.Count > 0){
            if (isFlooding) {
                foreach (GameObject room in neighbourRooms){
                    isWaterLevelRising = true;
                    if (room.GetComponent<InteriorFloodRegion>().isFlooding && transform.position.y > room.transform.position.y){
                        isWaterLevelRising = false;
                        risingWaterScript.StopRising();
                        break;
                    }
                }
                if (isWaterLevelRising){
                    risingWaterScript.StartRising();
                }
                
            }
        }
    }

    public bool getIsFlooding(){
        return isFlooding;
    }
    public void setIsFlooding(bool value){
        isFlooding = value;
    }
}
