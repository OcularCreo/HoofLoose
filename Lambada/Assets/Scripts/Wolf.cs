using System.Collections.Generic;
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
    private bool sheepRemoved = false;

    private float speed = 3f;

    private Vector3 escapePos = new Vector3(11f, 0f, 0f);


    private void Start()
    {
        if (!sheepManager) 
        {
            sheepManager = GameObject.FindGameObjectWithTag("SheepManager").GetComponent<SheepManager>();
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            isStareTimerRunning = true;
        }
		else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            isStareTimerRunning = false;
        }

        CheckStare();

        if (!isStareTimerRunning) 
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
                //Lose?
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
            hasSheep = true;
        }
    }

    private void AttemptEscape() 
    {
        if (!sheepRemoved) 
        {
            //remove sheep
            sheepManager.KillSheep(sheepToSteal);
            sheepRemoved = true;
        }

        //run off screen
        Vector2 direction = (escapePos - transform.position).normalized;

        transform.position = Vector2.MoveTowards(transform.position, escapePos, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, escapePos) < 0.5) 
        {
            Die();
        }
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
        if (hasSheep) 
        {
            sheepManager.SpawnSheep(sheepToSteal);
            List<GameObject> sheepRespawned = sheepManager.GetAllStateSheep(SheepBehaviour.SheepState.Graze);
            for (int i = 0; i < sheepToSteal; i++) 
            {
                sheepRespawned[i].GetComponent<SheepBehaviour>().TransitionToDanceState();
            }
        }

        Die();
    }

    private void Die() 
    {
        Destroy(gameObject);
    }
}
