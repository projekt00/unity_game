using UnityEngine;

public class WaterSourceController : MonoBehaviour
{
    public WaterSpread waterSpreadPrefab; // Prefab skryptu odpowiedzialnego za rozprzestrzenianie się wody
    public float initialWaterLevel = 0.5f; // Początkowy poziom wody
    public float floodSpeed = 0.1f; // Jak szybko poziom wody wzrasta
    private WaterSpread waterSpreadInstance;

    private void Start()
    {
        // Inicjalizacja rozprzestrzeniania wody
        StartFlooding();
    }

    public void StartFlooding()
    {
        // Tworzymy instancję systemu rozprzestrzeniania wody
        if (waterSpreadPrefab != null)
        {
            // Umieszczamy woda w początkowej pozycji
            waterSpreadInstance = Instantiate(waterSpreadPrefab, transform.position, Quaternion.identity);
            waterSpreadInstance.spreadRate = floodSpeed; // Ustawiamy szybkość rozprzestrzeniania
            waterSpreadInstance.maxWaterLevel = initialWaterLevel; // Ustawiamy początkowy poziom wody
        }
    }
}