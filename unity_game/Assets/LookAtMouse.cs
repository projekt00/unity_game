using UnityEngine;

public class LookAtMouse : MonoBehaviour
{
    public Transform headTransform; // Referencja do transformacji g³owy lub oczu modelu
    public Camera mainCamera;       // Kamera g³ówna (przypisana w Unity)

    void Start()
    {
        // Przypisanie g³ównej kamery, jeœli nie jest przypisana
        if (mainCamera == null)
            mainCamera = Camera.main;
    }

    void Update()
    {
        // Uzyskaj pozycjê kursora myszy w przestrzeni ekranu
        Vector3 mousePosition = Input.mousePosition;

        // Przekonwertuj pozycjê kursora z przestrzeni ekranu do przestrzeni œwiata
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, mainCamera.nearClipPlane + 1f));

        // Oblicz kierunek z g³owy do pozycji kursora
        Vector3 direction = worldPosition - headTransform.position;

        // Obróæ g³owê/oczy w kierunku kursora, zachowuj¹c oryginaln¹ orientacjê osi Y
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        headTransform.rotation = Quaternion.Slerp(headTransform.rotation, lookRotation, Time.deltaTime * 5f);
    }
}
