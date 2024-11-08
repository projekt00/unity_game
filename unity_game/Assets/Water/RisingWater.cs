using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RisingWater : MonoBehaviour
{
    public float maxHeight;
    public Vector3 heightIncrement = new Vector3(0, 0.1f, 0);
    public bool autoFlooding = true; //If set to true it will start flooding and game begining

    public float interval = 1.0f;
    private bool coroutineStarted = false;
    private Coroutine coroutine;
    void Start()
    {   
        if (autoFlooding) {
            StartRising();
        }
    }

    IEnumerator ScaleMarkers()
    {
        while (transform.position.y < maxHeight)
        {
            transform.DOMove(transform.position + heightIncrement, interval).SetEase(Ease.Linear);
            yield return new WaitForSeconds(interval);
        }
    }
    public void StopRising(){
        //Debug.Log("rising stopped");
        if (coroutineStarted){
            StopCoroutine(coroutine);
            coroutineStarted = false;
        }
    }
    public void StartRising(){
        if (!coroutineStarted){
            coroutine = StartCoroutine(ScaleMarkers());
            coroutineStarted = true;
        }
    }
}