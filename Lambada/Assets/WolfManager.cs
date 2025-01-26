using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.UI;

public class WolfManager : MonoBehaviour
{
    // Timer duration
    private float totalTime = 25f; // Time before wolf comes
    private float timeRemaining = 25f;
    private bool isTimerRunning = true;

    private int sheepToSteal = 1;

    [SerializeField] private GameObject wolfPrefab;

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
        GameObject wolf = Instantiate(wolfPrefab, transform.position, Quaternion.identity);
        wolf.GetComponent<Wolf>().sheepToSteal = stealAmount;
    }
}
