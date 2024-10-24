using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RisingWater : MonoBehaviour
{
    public float maxHeight;
    public Vector3 heightIncrement = new Vector3(0, 0.1f, 0);

    public float interval = 1.0f;

    void Start()
    {
        StartCoroutine(ScaleMarkers());
    }


    IEnumerator ScaleMarkers()
    {
        while (transform.position.y < 0)
        {
            transform.position += heightIncrement;
            yield return new WaitForSeconds(interval);
        }
    }
}