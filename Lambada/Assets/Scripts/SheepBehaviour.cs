using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepBehaviour : MonoBehaviour
{
    private float moveSpeed = 1f; // Movement speed of the GameObject
    private Vector2 areaMin = new Vector2(-5f, -2.45f); // Bottom-left corner of the area
    private Vector2 areaMax = new Vector2(5f, -0.5f); // Top-right corner of the area
    private float changeDirectionInterval = 1f; // Time interval before changing direction

    private float timeSinceLastDirectionChange = 0f;
    private Vector2 currentDirection;

    private Color danceColour = Color.white;
    private Color grazeColour = new Color(183, 183, 183);


    //STATES
    public enum SheepState
    {
        Graze,
        Dance
    }

    private SheepState currentState = SheepState.Graze;
    private GameObject[] grazePoints;
    private GameObject currentGrazePoint;

    //public float moveSpeed = 3f;

    //DANCING
    private float rotationInterval = 1f; // Time interval (in seconds) between each 45-degree rotation
    private float timer;

    private void Start()
    {
        SetRandomDirection();

    }

    void Update()
    {
        switch (currentState)
        {
            case SheepState.Graze:
                GrazeUpdate();
                break;
            case SheepState.Dance:
                DanceUpdate();
                break;
        }
    }
    
    private void GrazeUpdate()
    {
        if (currentGrazePoint != null && Vector3.Distance(transform.position, currentGrazePoint.transform.position) > 0.5f)
        {
            MoveToGrazePoint();
        }
        else if (currentGrazePoint == null) 
        {
            ChooseNewGrazePoint();
            //Debug.Log("currentGrazePoint = null");
        }
    }

    private void DanceUpdate()
    {
        timeSinceLastDirectionChange += Time.deltaTime;

        // Change direction after the specified interval
        if (timeSinceLastDirectionChange >= changeDirectionInterval)
        {
            SetRandomDirection();
            timeSinceLastDirectionChange = 0f;
        }

        // Move the GameObject in the current direction
        transform.Translate(currentDirection * moveSpeed * Time.deltaTime);

        // Make sure the GameObject stays within the specified area
        Vector3 position = transform.position;
        position.x = Mathf.Clamp(position.x, areaMin.x, areaMax.x);
        position.y = Mathf.Clamp(position.y, areaMin.y, areaMax.y);
        transform.position = position;


        Dance();
    }

    void SetRandomDirection()
    {
        // Pick a random direction
        float randomAngle = Random.Range(0f, 360f);
        currentDirection = new Vector2(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle)).normalized;
    }

    private void Dance() 
    {
        // Increment the timer by the time passed since the last frame
        timer += Time.deltaTime;

        // Check if the specified interval has passed
        if (timer >= rotationInterval)
        {
            // Rotate the object by 45 degrees around the Z-axis (2D rotation)
            //transform.Rotate(0, 0, 45);

            // Reset the timer
            timer = 0f;

            if (GetComponent<BouncyScript>()) 
            {
                GetComponent<BouncyScript>().Bounce();
            }
        }
    }

    private void ChooseNewGrazePoint()
    {
        grazePoints = GameObject.FindGameObjectsWithTag("GrazePoint");

        if (grazePoints.Length > 0)
        {
            // Pick a random graze point
            currentGrazePoint = grazePoints[Random.Range(0, grazePoints.Length)];
        }
    }

    private void MoveToGrazePoint()
    {
        if (currentGrazePoint != null)
        {
            Vector3 targetPosition = currentGrazePoint.transform.position;

            // Move towards the target
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        }
    }

    public void TransitionToGrazeState()
    {
        currentState = SheepState.Graze;
        ChooseNewGrazePoint();
        gameObject.GetComponent<SpriteRenderer>().color = grazeColour;
    }

    public void TransitionToDanceState()
    {
        currentState = SheepState.Dance;
        gameObject.GetComponent<SpriteRenderer>().color = danceColour;
        // You can add a transition animation or behavior here if needed.
    }

    public SheepState GetState() 
    {
        return currentState;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue; // Set color for the boundaries

        // Draw a rectangle to show the spawn area
        Vector2 bottomLeft = areaMin;
        Vector2 topRight = areaMax;

        Gizmos.DrawLine(bottomLeft, new Vector2(areaMax.x, areaMin.y)); // Bottom line
        Gizmos.DrawLine(new Vector2(areaMax.x, areaMin.y), topRight); // Right line
        Gizmos.DrawLine(topRight, new Vector2(areaMin.x, areaMax.y)); // Top line
        Gizmos.DrawLine(new Vector2(areaMin.x, areaMax.y), bottomLeft); // Left line
    }
}
