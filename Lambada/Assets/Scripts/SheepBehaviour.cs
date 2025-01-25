using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepBehaviour : MonoBehaviour
{
    //FLOCKING
    public float maxSpeed = 5f;
    public float maxForce = 1f;

    public float followWeight = 1f; // How strongly the sheep follow the player
    public float separationWeight = 1.5f; // How strongly the sheep avoid each other
    public float cohesionWeight = 1f; // How strongly the sheep stay together

    public float separationRadius = 2f; // Distance to stay away from other sheep
    public float cohesionRadius = 10f; // Distance to consider for flock cohesion
    public float followRadius = 20f; // Distance where sheep start following the player

    public Transform player; // Reference to the player's position
    public List<Transform> flockmates = new List<Transform>(); // List of other sheep

    private Vector3 velocity; // Current velocity of the sheep


    //STATES
    public enum SheepState
    {
        Graze,
        Dance
    }

    private SheepState currentState = SheepState.Graze;
    private GameObject[] grazePoints;
    private GameObject currentGrazePoint;

    public float moveSpeed = 3f;

    //DANCING
    private float rotationInterval = 1f; // Time interval (in seconds) between each 45-degree rotation
    private float timer;

    private void Start()
    {
        if (player == null)    
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    void Update()
    {
        if (player)
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

            // Press 'N' to switch to Dance state
            if (Input.GetKeyDown(KeyCode.N))
            {
                if (currentState == SheepState.Graze)
                {
                    TransitionToDanceState();
                }
                else
                {
                    //TransitionToGrazeState();
                }
            }
        }
    }

    private void Flock() 
    {
        // Update flockmates (other sheep in the flock)
        UpdateFlockmates();

        // Compute the forces applied to the sheep
        Vector3 followForce = FollowPlayer();
        Vector3 separationForce = Separation();
        Vector3 cohesionForce = Cohesion();

        // Combine all forces
        Vector3 steering = followForce * followWeight + separationForce * separationWeight + cohesionForce * cohesionWeight;

        // Apply the steering force to the velocity
        velocity += steering * Time.deltaTime;

        // Limit the velocity to the max speed
        velocity = Vector3.ClampMagnitude(velocity, maxSpeed);

        // Move the sheep
        transform.position += velocity * Time.deltaTime;
    }

    void UpdateFlockmates()
    {
        flockmates.Clear();
        foreach (var sheep in FindObjectsOfType<SheepBehaviour>())
        {
            if (sheep != this) // Avoid adding itself to the list
            {
                flockmates.Add(sheep.transform);
            }
        }
    }

    Vector3 FollowPlayer()
    {
        Vector3 desired = player.position - transform.position;
        float distance = desired.magnitude;

        // Only follow if within the followRadius
        if (distance < followRadius)
        {
            desired.Normalize();
            desired *= maxSpeed;
            Vector3 steer = desired - velocity;
            return Vector3.ClampMagnitude(steer, maxForce);
        }

        return Vector3.zero;
    }

    Vector3 Separation()
    {
        Vector3 force = Vector3.zero;
        int count = 0;

        foreach (var flockmate in flockmates)
        {
            float distance = Vector3.Distance(transform.position, flockmate.position);

            if (distance < separationRadius)
            {
                Vector3 diff = transform.position - flockmate.position;
                diff.Normalize();
                diff /= distance; // The closer the sheep, the stronger the repulsion
                force += diff;
                count++;
            }
        }

        if (count > 0)
        {
            force /= count;
        }

        if (force.magnitude > 0)
        {
            force.Normalize();
            force *= maxSpeed;
            force -= velocity;
            force = Vector3.ClampMagnitude(force, maxForce);
        }

        return force;
    }

    Vector3 Cohesion()
    {
        Vector3 force = Vector3.zero;
        int count = 0;

        foreach (var flockmate in flockmates)
        {
            float distance = Vector3.Distance(transform.position, flockmate.position);

            if (distance < cohesionRadius)
            {
                force += flockmate.position;
                count++;
            }
        }

        if (count > 0)
        {
            force /= count;
            force -= transform.position;
            force.Normalize();
            force *= maxSpeed;
            force -= velocity;
            force = Vector3.ClampMagnitude(force, maxForce);
        }

        return force;
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
        Flock();
        Dance();
    }

    private void Dance() 
    {
        // Increment the timer by the time passed since the last frame
        timer += Time.deltaTime;

        // Check if the specified interval has passed
        if (timer >= rotationInterval)
        {
            // Rotate the object by 45 degrees around the Z-axis (2D rotation)
            transform.Rotate(0, 0, 45);

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

    private void TransitionToGrazeState()
    {
        currentState = SheepState.Graze;
        ChooseNewGrazePoint();
    }

    private void TransitionToDanceState()
    {
        currentState = SheepState.Dance;
        // You can add a transition animation or behavior here if needed.
    }

    public SheepState GetState() 
    {
        return currentState;
    }
}
