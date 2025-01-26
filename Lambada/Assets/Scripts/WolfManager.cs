using UnityEngine;
using UnityEngine.UI;

public class WolfManager : MonoBehaviour
{
    // Timer duration
    private float totalTime = 17f; // Time before wolf comes
    private float timeRemaining = 17f;
    private bool isTimerRunning = false;

    private int sheepToSteal = 1;

    [SerializeField] private GameObject wolfPrefab;

    private Vector3 spawnPos = new Vector3(11f, 0f, 0f);

    void Update()
    {
        if (isTimerRunning)
        {
            // Decrease the timer by the time elapsed each frame
            timeRemaining -= Time.deltaTime;

            //Debug.Log("Wolf Timer: " + timeRemaining);

            // If the timer reaches 0, restart it
            if (timeRemaining <= 0f)
            {
                SpawnWolf(sheepToSteal);

                timeRemaining = totalTime; // Restart the timer
                //Debug.Log("Timer restarted");

                sheepToSteal = sheepToSteal * 2;
                if (sheepToSteal > 500)
                {
                    sheepToSteal = 500;
                }
                //Debug.Log("Next Steal: " + sheepToSteal);
            }
        }
    }

    public void SpawnWolf(int stealAmount) 
    {
        GameObject wolf = Instantiate(wolfPrefab, spawnPos, Quaternion.identity);
        wolf.GetComponent<Wolf>().sheepToSteal = stealAmount;
    }

    public void StartTimer() 
    {
        if (!isTimerRunning) 
        {
            isTimerRunning = true;
            //Debug.Log("Wolf timer started");
        }
    }
}
