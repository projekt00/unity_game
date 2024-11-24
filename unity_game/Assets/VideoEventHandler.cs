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
    public UnityEvent onVideoEnd; // Wywo�ywane po zako�czeniu filmu
    public UnityEvent onVideoSkip; // Wywo�ywane po pomini�ciu filmu

    private float skipHoldTime = 3f; // Czas trzymania spacji, aby pomin�� wideo
    private float spaceHeldTime = 0f; // Licznik trzymania spacji

    void Start()
    {
        // Pobierz komponent VideoPlayer
        videoPlayer = GetComponent<VideoPlayer>();

        // Sprawd�, czy VideoPlayer istnieje
        if (videoPlayer != null)
        {
            // Obs�uga zako�czenia filmu
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
            // Je�li spacja jest trzymana, zwi�ksz licznik
            spaceHeldTime += Time.deltaTime;

            // Sprawd�, czy czas trzymania spacji osi�gn�� pr�g
            if (spaceHeldTime >= skipHoldTime)
            {
                SkipVideo();
            }
        }
        else
        {
            // Reset licznika, je�li spacja nie jest trzymana
            spaceHeldTime = 0f;
        }
    }

    private void SkipVideo()
    {
        Debug.Log("Video skipped by holding space!");

        // Zatrzymanie odtwarzania wideo
        videoPlayer.Stop();

        // Wywo�anie eventu pomini�cia
        onVideoSkip?.Invoke();

        // Wywo�anie logiki ko�cowej
        HandleVideoEnd(videoPlayer);
    }

    private void HandleVideoEnd(VideoPlayer vp)
    {
        Debug.Log("Video has finished playing or was skipped!");

        // Wywo�anie eventu zako�czenia
        onVideoEnd?.Invoke();
    }

    void OnDestroy()
    {
        // Usu� listener, aby unikn�� b��d�w przy niszczeniu obiektu
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached -= HandleVideoEnd;
        }
    }
}
