using UnityEngine;

public class SheepSpawner : MonoBehaviour
{
    // Reference to the Sheep prefab
    [SerializeField] private GameObject sheepPrefab;

    // The boundaries within which the sheep will spawn
    [SerializeField] private float minX = -8f;
    [SerializeField] private float maxX = 8f;
    [SerializeField] private float minY = -4f;
    [SerializeField] private float maxY = 4f;

    // Update is called once per frame
    void Update()
    {
        // Check if the "M" key is pressed
        if (Input.GetKeyDown(KeyCode.M))
        {
            SpawnSheep();
        }
    }

    // Method to spawn a sheep at a random position
    void SpawnSheep()
    {
        // Generate a random position within the specified bounds
        float randomX = Random.Range(minX, maxX);
        float randomY = Random.Range(minY, maxY);

        // Instantiate the sheep prefab at the random position
        Vector2 spawnPosition = new Vector2(randomX, randomY);
        Instantiate(sheepPrefab, spawnPosition, Quaternion.identity);
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
