using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.UI;

public class Wolf : MonoBehaviour
{
    private SheepManager sheepManager;

    public int sheepToSteal = 1;

    private float stareTimer = 3f; // When this fills the wolf has been stopped
    private float stareTimeRemaining = 3f;
    private bool isStareTimerRunning = false;

    private bool targetAquired;
    private GameObject target;
    private float distFromTarget;
    private float distToGrab = 1f;

    private bool hasSheep;

    private float speed = 3f;


    private void Start()
    {
        if (!sheepManager) 
        {
            sheepManager = GameObject.FindGameObjectWithTag("SheepManager").GetComponent<SheepManager>();
        }
    }
    void Update()
    {
        CheckStare();

        if (isStareTimerRunning) 
        {
            //Do nothing
        }
        else
        {
            if (hasSheep)
            {
                AttemptEscape();
            }
            else
            {
                StalkSheep();
            }
        }
    }

    private void StalkSheep()
    {
        //pick sheep
        if (!targetAquired) 
        {
            target = sheepManager.GetRandomStateSheep(SheepBehaviour.SheepState.Dance);
            if (target != null)
            {
                targetAquired = true;
                distFromTarget = Vector2.Distance(transform.position, target.transform.position);
            }
            else 
            {
                Debug.Log("No Target");
            }
        }
        else if (distFromTarget > distToGrab)
        {
            //walk to sheep
            if (target != null)
            {
                Vector2 direction = (target.transform.position - transform.position).normalized;

                transform.position = Vector2.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);

                distFromTarget = Vector2.Distance(transform.position, target.transform.position);
            }
        }
        //if wolf reahes sheep
        else if (distFromTarget < distToGrab) 
        {
            AttemptEscape();
        }
    }

    private void AttemptEscape() 
    {
        //walk off screen
        //once off screen delete self
    }

    private void CheckStare() 
    {
        if (isStareTimerRunning)
        {
            // Decrease the timer by the time elapsed each frame
            stareTimeRemaining -= Time.deltaTime;

            //Debug.Log("Wolf Timer: " + stareTimeRemaining);

            // If the timer reaches 0, stop steal
            if (stareTimeRemaining <= 0f)
            {
                DropSheep();

                stareTimeRemaining = stareTimer; // Restart the timer
                isStareTimerRunning = false;
            }
        }
    }

    private void DropSheep()
    {
        //Spawn Sheep


        Die();
    }

    private void Die() 
    {
        Destroy(gameObject);
    }
}
