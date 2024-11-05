using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class testbox : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Move());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Move()
    {   Debug.Log("smack");
        transform.DOMove(transform.position + new Vector3(1f, 0, 0), 1.0f).SetEase(Ease.Linear);
        yield return new WaitForSeconds(1.0f);
    }
}
