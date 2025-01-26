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

    private bool hasChangedDirection = false;

    private GameManager gameManager;


    private void Start()
    {
        gameObject.transform.rotation = Quaternion.Euler(transform.rotation.x, 180f, transform.rotation.z);
        if (!sheepManager) 
        {
            sheepManager = GameObject.FindGameObjectWithTag("SheepManager").GetComponent<SheepManager>();
        }

        gameManager = GameObject.FindObjectOfType<GameManager>();
    }
    void Update()
    {
        if (gameManager == null)
        {
            gameManager = GameObject.FindObjectOfType<GameManager>();
        }


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
            gameManager.lives = gameManager.lives - sheepToSteal;
            sheepRemoved = true;
        }

        if (!hasChangedDirection) 
        {
            hasChangedDirection = true;
            ChangeDirection();
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
                stareTimeRemaining = stareTimer; // Restart the timer
                isStareTimerRunning = false;
                DropSheep();

            }
        }
    }

    private void DropSheep()
    {
        //Spawn Sheep
        if (hasSheep) 
        {
            if (gameManager.lives + sheepToSteal > 50)
            {
                int sheepToSpawn = 50 - gameManager.lives;
                if (sheepToSpawn < 0) 
                {
                    sheepToSpawn = 0;
                }

                sheepManager.SubmitCombo(sheepToSpawn);
            }
            else 
            {
                sheepManager.SubmitCombo(sheepToSteal);
            }
        }

        Die();
    }

    private void Die() 
    {
        Destroy(gameObject);
    }

    private void ChangeDirection() 
    {
        gameObject.GetComponent<BouncyScript>().Bounce();
        gameObject.transform.rotation = Quaternion.Euler(transform.rotation.x, 0f, transform.rotation.z);
        //Vector3 newRot = gameObject.transform.rotation;
        //gameObject.transform.localScale = new Vector3(-1 * (newScale.x), newScale.y, newScale.z);
    }
}
