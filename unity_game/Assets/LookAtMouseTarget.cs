using UnityEngine;

public class LookAtMouseTarget : MonoBehaviour
{
    public Camera mainCamera;
    public Transform lookTarget;       // Pusty obiekt LookTarget
    public BoxCollider trackingArea;   // Collider okreœlaj¹cy obszar œledzenia
    public float followSpeed = 5f;     // Prêdkoœæ interpolacji
    public Vector3 defaultPosition;    // Domyœlna pozycja LookTarget, gdy kursor jest poza obszarem

    private bool isInsideArea = false; // Flaga sprawdzaj¹ca, czy kursor jest w colliderze

    void Start()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        // Zapisz domyœln¹ pozycjê LookTarget
        defaultPosition = lookTarget.position;
    }

    void Update()
    {
        // Pobierz pozycjê kursora w przestrzeni œwiata
        Vector3 mousePosition = Input.mousePosition;
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, mainCamera.nearClipPlane + 2f));

        // SprawdŸ, czy kursor znajduje siê wewn¹trz Box Collidera
        if (IsWithinBounds(worldPosition))
        {
            // Aktualizuj pozycjê LookTarget, aby œledzi³ myszkê
            lookTarget.position = Vector3.Lerp(lookTarget.position, worldPosition, Time.deltaTime * followSpeed);
            isInsideArea = true;
        }
        else if (isInsideArea)
        {
            // Gdy kursor opuœci obszar, wróæ do domyœlnej pozycji
            lookTarget.position = Vector3.Lerp(lookTarget.position, defaultPosition, Time.deltaTime * followSpeed);
            isInsideArea = false;
        }
    }

    // Funkcja sprawdzaj¹ca, czy punkt znajduje siê wewn¹trz Box Collidera
    private bool IsWithinBounds(Vector3 point)
    {
        return trackingArea.bounds.Contains(point);
    }
}

