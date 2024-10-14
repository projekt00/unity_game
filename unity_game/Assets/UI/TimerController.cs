using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimerController : MonoBehaviour
{
    public TMP_Text Timer;
    public float startTime = 10f;
    private float timeRemaining;
    // Start is called before the first frame update
    void Start()
    {
        timeRemaining = startTime * 60;
        UpdateTimer();
    }

    // Update is called once per frame
    void Update()
    {
        timeRemaining -= Time.deltaTime;
        UpdateTimer();
    }

    private void UpdateTimer(){

        if (timeRemaining > 0){
            int minutes = Mathf.FloorToInt(timeRemaining / 60);
            int seconds = Mathf.FloorToInt(timeRemaining % 60);

            Timer.text = "Time: " + string.Format("{0:00}:{1:00}", minutes, seconds);
        } else {
            Timer.text = "Time has ended";
        }
    }
}
