using UnityEngine;

public class TextBubbleMover : MonoBehaviour
{
    public float moveSpeed = 3f;           // Speed at which the object moves towards the target
    public float destroyTime = 4f;        // Time before the object destroys itself
    public Vector2 positionOffset;         // Offset to add to the target's position

    public Transform target;             // The target to move toward

    void Start()
    {
        // Destroy this object after a set amount of time
        Destroy(gameObject, destroyTime);
    }

    void Update()
    {
        if (target != null)
        {
            // Continuously lerp towards the target position with offset
            transform.position = Vector2.Lerp(transform.position, target.position + (Vector3)positionOffset, moveSpeed * Time.deltaTime);
        }
    }
}
