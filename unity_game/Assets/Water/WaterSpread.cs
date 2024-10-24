using UnityEngine;

public class WaterSpread : MonoBehaviour
{
    public GameObject waterPlanePrefab;  // Prefab wody
    public float spreadRate = 0.5f;      // Szybkość rozprzestrzeniania się wody
    public float maxWaterLevel = 5f;     // Maksymalny poziom wody
    private float currentWaterLevel = 0f;  // Aktualny poziom wody
    
    public float spreadInterval = 2f;    // Czas pomiędzy rozprzestrzenianiem
    private float spreadTimer = 0f;      // Timer rozprzestrzeniania
    public LayerMask obstacleLayer;      // Warstwa przeszkód (np. ściany)

    private bool hasSpread = false;      // Czy woda już się rozprzestrzeniła?

    private void Start()
    {
        // Tworzymy pierwszy WaterPlane na pozycji WaterSource
        Instantiate(waterPlanePrefab, transform.position, Quaternion.identity);
    }

    private void Update()
    {
        // Stopniowo podnosimy poziom wody
        if (currentWaterLevel < maxWaterLevel)
        {
            currentWaterLevel += spreadRate * Time.deltaTime;
            transform.localScale = new Vector3(transform.localScale.x, currentWaterLevel, transform.localScale.z);
        }

        // Czas na rozprzestrzenianie wody
        spreadTimer += Time.deltaTime;
        if (spreadTimer >= spreadInterval && !hasSpread)
        {
            SpreadWater();
            spreadTimer = 0f;  // Reset timera
        }
    }

    private void SpreadWater()
    {
        // Kierunki rozprzestrzeniania wody
        Vector3[] directions = {
            Vector3.forward,  // Przód
            Vector3.back,     // Tył
            Vector3.left,     // Lewo
            Vector3.right     // Prawo
        };

        foreach (Vector3 direction in directions)
        {
            Vector3 newPosition = transform.position + direction * transform.localScale.x;

            // Sprawdzamy czy są przeszkody na nowej pozycji
            if (!Physics.CheckBox(newPosition, transform.localScale / 2, Quaternion.identity, obstacleLayer))
            {
                // Tworzymy nową instancję wody
                Instantiate(waterPlanePrefab, newPosition, Quaternion.identity);
            }
        }

        hasSpread = true; // Zapobiega wielokrotnemu rozprzestrzenianiu
    }
}