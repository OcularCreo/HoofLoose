using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepFlockBehavior : MonoBehaviour
{
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

    void Update()
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
        foreach (var sheep in FindObjectsOfType<SheepFlockBehavior>())
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
}
