using UnityEngine;
using System.Collections.Generic;

public class MissionManager : MonoBehaviour
{
    public List<MissionObject> missionObjects; // Lista obiektów misji
    private int currentMissionIndex = 0; // Indeks aktualnej misji

    void Start()
    {
        // Zbieranie wszystkich obiektów MissionObject, które s¹ dzieæmi tego obiektu
        missionObjects = new List<MissionObject>(GetComponentsInChildren<MissionObject>());
       
    }


}
