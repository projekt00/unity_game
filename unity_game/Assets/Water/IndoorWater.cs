using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndoorWater : MonoBehaviour
{
    public List<GameObject> entranceRooms = new List<GameObject>(); //All rooms with home entrance doors
    public List<GameObject> rooms = new List<GameObject>(); //Rest of rooms
    public float floodingSpeed = 0.1f;
    private float[] entranceRoomsArea;
    private float[] roomsArea;

    void Start()
    {
        entranceRoomsArea = new float[entranceRooms.Count];
        roomsArea = new float[rooms.Count];

        for (int i = 0; i < entranceRooms.Count; i++){
            MeshFilter meshFilter = entranceRooms[i].GetComponent<MeshFilter>();
            if (meshFilter != null && meshFilter.mesh != null){
                Mesh mesh = meshFilter.sharedMesh; // Użyj sharedMesh z MeshCollidera
                Vector3[] vertices = mesh.vertices;
                int[] triangles = mesh.triangles;
                float totalArea = 0f;

                for (int j = 0; j < triangles.Length; j += 3)
                {
                    Vector3 v0 = vertices[triangles[j]];
                    Vector3 v1 = vertices[triangles[j + 1]];
                    Vector3 v2 = vertices[triangles[j + 2]];

                    v0 = entranceRooms[i].transform.TransformPoint(v0);
                    v1 = entranceRooms[i].transform.TransformPoint(v1);
                    v2 = entranceRooms[i].transform.TransformPoint(v2);

                    Vector3 side1 = v1 - v0;
                    Vector3 side2 = v2 - v0;

                    totalArea += Vector3.Cross(side1, side2).magnitude * 0.5f;
                    }
                entranceRoomsArea[i] = totalArea;

            }
        }
        foreach(float item in entranceRoomsArea){
            Debug.Log(item);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
