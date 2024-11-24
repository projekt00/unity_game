using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Video;

public class VideoEventHandler : MonoBehaviour
{
    private VideoPlayer videoPlayer;

    [Header("Events")]
    public UnityEvent onVideoEnd; // Wywo³ywane po zakoñczeniu filmu
    public UnityEvent onVideoSkip; // Wywo³ywane po pominiêciu filmu

    private float skipHoldTime = 3f; // Czas trzymania spacji, aby pomin¹æ wideo
    private float spaceHeldTime = 0f; // Licznik trzymania spacji

    void Start()
    {
        // Pobierz komponent VideoPlayer
        videoPlayer = GetComponent<VideoPlayer>();

        // SprawdŸ, czy VideoPlayer istnieje
        if (videoPlayer != null)
        {
            // Obs³uga zakoñczenia filmu
            videoPlayer.loopPointReached += HandleVideoEnd;
        }
        else
        {
            Debug.LogError("Brak komponentu VideoPlayer na tym obiekcie!");
        }
    }

    void Update()
    {
        HandleSkipInput();
    }

    private void HandleSkipInput()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            // Jeœli spacja jest trzymana, zwiêksz licznik
            spaceHeldTime += Time.deltaTime;

            // SprawdŸ, czy czas trzymania spacji osi¹gn¹³ próg
            if (spaceHeldTime >= skipHoldTime)
            {
                SkipVideo();
            }
        }
        else
        {
            // Reset licznika, jeœli spacja nie jest trzymana
            spaceHeldTime = 0f;
        }
    }

    private void SkipVideo()
    {
        Debug.Log("Video skipped by holding space!");

        // Zatrzymanie odtwarzania wideo
        videoPlayer.Stop();

        // Wywo³anie eventu pominiêcia
        onVideoSkip?.Invoke();

        // Wywo³anie logiki koñcowej
        HandleVideoEnd(videoPlayer);
    }

    private void HandleVideoEnd(VideoPlayer vp)
    {
        Debug.Log("Video has finished playing or was skipped!");

        // Wywo³anie eventu zakoñczenia
        onVideoEnd?.Invoke();
    }

    void OnDestroy()
    {
        // Usuñ listener, aby unikn¹æ b³êdów przy niszczeniu obiektu
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached -= HandleVideoEnd;
        }
    }
}
