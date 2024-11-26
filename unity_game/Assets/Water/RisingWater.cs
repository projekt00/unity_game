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
    private Tween waterRiseTween;
    private float initLevel;
    private Coroutine decreasingCoroutine;
    void Start()
    {   
        if (autoFlooding) {
            StartRising();
        }
        initLevel = transform.position.y;
    }

    IEnumerator ScaleMarkers()
    {
        while (transform.position.y < maxHeight)
        {
            waterRiseTween = transform.DOMove(transform.position + heightIncrement, interval).SetEase(Ease.Linear);
            yield return new WaitForSeconds(interval);
        }
    }
    IEnumerator DecreaseMarkers(){
        while (transform.position.y > initLevel){
            waterRiseTween = transform.DOMove(transform.position - heightIncrement*5, interval).SetEase(Ease.Linear);
            yield return new WaitForSeconds(interval);            
        }
    }
    public void StopRising(float equalizeTo = -1000000f){
        //Debug.Log("rising stopped");
        if (coroutineStarted){
            StopCoroutine(coroutine);
            waterRiseTween.Kill();
            coroutineStarted = false;
            if (equalizeTo != -1000000f){
                Vector3 newPos = transform.position;
                newPos.y = equalizeTo;
                transform.position = newPos;
            }
        }
    }
    public void StartRising(){
        Debug.Log(gameObject.name);
        if (!coroutineStarted){
            coroutine = StartCoroutine(ScaleMarkers());
            coroutineStarted = true;
        }
    }
    public void DecreaseWater(){
        StopRising();
        if (decreasingCoroutine == null){
            decreasingCoroutine = StartCoroutine(DecreaseMarkers());
        }
    }
}