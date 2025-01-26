using Unity.VisualScripting;
using UnityEngine;

public class SheepSpawner : MonoBehaviour
{
    // Reference to the Sheep prefab
    [SerializeField] private GameObject sheepPrefab;

    [SerializeField] private SheepManager sheepManager;

    // The boundaries within which the sheep will spawn
    [SerializeField] private float minX = -8f;
    [SerializeField] private float maxX = 8f;
    [SerializeField] private float minY = -4f;
    [SerializeField] private float maxY = 4f;

    //private string sheepTag = "Sheep"; // Tag to identify sheep
    private int maxSheepCount = 10; // Maximum number of sheep allowed in the scene
    private float spawnInterval = 5f; // Initial time interval for spawning sheep
    private float intervalDecrement = 0.1f; // How much the interval will decrease over time
    private float minInterval = 1f; // Minimum spawn interval

    private float currentInterval; // Current spawn interval
    private float timeSinceLastSpawn; // Time passed since the last spawn

    private void Start()
    {
        currentInterval = spawnInterval; // Set the initial interval
        timeSinceLastSpawn = 0f; // Initialize the timer

        if (!sheepManager) 
        {
            sheepManager = GameObject.FindGameObjectWithTag("SheepManager").GetComponent<SheepManager>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Count the number of sheep in the scene
        int sheepCount = sheepManager.GetNumberStateSheep(SheepBehaviour.SheepState.Graze);
        //Debug.Log(sheepCount);

        // If the number of sheep is less than the maximum, attempt to spawn a new one
        if (sheepCount < maxSheepCount)
        {
            timeSinceLastSpawn += Time.deltaTime;

            // If enough time has passed, spawn a new sheep
            if (timeSinceLastSpawn >= currentInterval)
            {
                SpawnSheep(1);
                timeSinceLastSpawn = 0f; // Reset the timer after spawning

                // Decrease the interval, but make sure it doesn't go below the minimum
                currentInterval = Mathf.Max(currentInterval - intervalDecrement, minInterval);
            }
        }
    }

    // Method to spawn a sheep at a random position
    public void SpawnSheep(int sheepToSpawn)
    {
        for (int i = 0; i < sheepToSpawn; i++)
        {
            // Generate a random position within the specified bounds
            float randomX = Random.Range(minX, maxX);
            float randomY = Random.Range(minY, maxY);

            // Instantiate the sheep prefab at the random position
            Vector2 spawnPosition = new Vector2(randomX, randomY);
            Instantiate(sheepPrefab, spawnPosition, Quaternion.identity);
        }
    }

    public GameObject SpawnSheepReturnSheep() 
    {
        // Generate a random position within the specified bounds
        float randomX = Random.Range(minX, maxX);
        float randomY = Random.Range(minY, maxY);

        // Instantiate the sheep prefab at the random position
        Vector2 spawnPosition = new Vector2(randomX, randomY);
        GameObject sheep = Instantiate(sheepPrefab, spawnPosition, Quaternion.identity);
        return sheep;
    }

    // Draw the boundaries in the editor
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green; // Set color for the boundaries

        // Draw a rectangle to show the spawn area
        Vector3 bottomLeft = new Vector3(minX, minY, 0);
        Vector3 topRight = new Vector3(maxX, maxY, 0);

        Gizmos.DrawLine(bottomLeft, new Vector3(maxX, minY, 0)); // Bottom line
        Gizmos.DrawLine(new Vector3(maxX, minY, 0), topRight); // Right line
        Gizmos.DrawLine(topRight, new Vector3(minX, maxY, 0)); // Top line
        Gizmos.DrawLine(new Vector3(minX, maxY, 0), bottomLeft); // Left line
    }
}
