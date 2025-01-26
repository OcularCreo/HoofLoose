using UnityEngine;

public class RandomColorChanger : MonoBehaviour
{
    private SpriteRenderer spriteRenderer; // Reference to the SpriteRenderer component
    public float changeInterval = 1f; // Time interval between color changes in seconds

    private float timer = 0f; // Timer to track time elapsed

    private void Start()
    {
        // If spriteRenderer is not assigned in the inspector, try to get the component
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
    }

    private void Update()
    {
        // Increment the timer by the time that has passed since the last frame
        timer += Time.deltaTime;

        // If the timer exceeds the changeInterval, change the color
        if (timer >= changeInterval)
        {
            ChangeColor();
            timer = 0f; // Reset the timer
        }
    }

    private void ChangeColor()
    {
        // Generate a random hue value (between 0 and 1)
        float hue = Random.Range(0f, 1f);

        // Convert hue to RGB and apply it to the sprite renderer
        Color randomColor = Color.HSVToRGB(hue, 0.6f, 1f);
        spriteRenderer.color = randomColor;
    }
}
