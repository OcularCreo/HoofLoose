using System.Collections;
using UnityEngine;

public class TextBubbleSpawner : MonoBehaviour
{
    public GameObject prefab; // Reference to the prefab to spawn
    public float timeInterval = 5f; // Time interval before spawning
    private GameObject[] sheepList; // List to hold sheep GameObjects
    public string sheepState = "Dance"; // State to look for

    private void Start()
    {
        // Start the coroutine that handles the spawn logic
        StartCoroutine(SpawnSheepCoroutine());
    }

    private IEnumerator SpawnSheepCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeInterval); // Wait for the set interval

            // Find all sheep with the "Sheep" tag
            sheepList = GameObject.FindGameObjectsWithTag("Sheep");

            // Create a list of sheep that are in the "Dance" state
            var dancingSheep = new System.Collections.Generic.List<Transform>();

            foreach (var sheep in sheepList)
            {
                SheepBehaviour.SheepState sheepState = sheep.GetComponent<SheepBehaviour>().GetState();
                if(sheepState == SheepBehaviour.SheepState.Dance) 
                {
                    dancingSheep.Add(sheep.transform);
                }
            }

            if (dancingSheep.Count > 0)
            {
                // Pick a random sheep from the ones in the "Dance" state
                Transform randomSheep = dancingSheep[Random.Range(0, dancingSheep.Count)];

                // Spawn the prefab at the sheep's location
                GameObject spawnedPrefab = Instantiate(prefab, randomSheep.position, Quaternion.identity);

                // Get the TextBubbleMover script from the prefab
                TextBubbleMover textBubbleMover = spawnedPrefab.GetComponent<TextBubbleMover>();

                if (textBubbleMover != null)
                {
                    // Set the target transform of the TextBubbleMover to the sheep's transform
                    textBubbleMover.target = randomSheep;
                }
            }
        }
    }
}
