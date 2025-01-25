using UnityEngine;
using System.Collections;
using static UnityEngine.UI.Image;

public class BouncyScript : MonoBehaviour
{
    [SerializeField] private float targetGrowthPercent = 1.40f;
    private Vector2 targetScale; // Target scale for the object
    public float lerpDurationX = 0.05f; // Duration for the X axis lerp transition
    public float lerpDurationY = 0.05f; // Duration for the Y axis lerp transition
    public float delayBeforeX = 0.03f; // Delay before X starts scaling (in seconds)

    private Vector2 originalScale;
    //private bool isScaling = false;
    private Coroutine scaleCoroutine;

    [SerializeField] bool onAnyKey = false;
    [SerializeField] bool onAlive = false;

    [SerializeField] KeyCode[] keycode;

    void Start()
    {
        originalScale = transform.localScale; // Save the original scale of the object

        targetScale = new Vector2(originalScale.x * targetGrowthPercent, originalScale.y * targetGrowthPercent);

        if (onAlive) 
        {
            scaleCoroutine = StartCoroutine(ScaleSequence());
        }
    }

    void Update()
    {
        if (onAnyKey && Input.anyKeyDown)
        {
            Bounce();
        }
        else 
        {
            for (int i = 0; i < keycode.Length; i++) 
            {
                if (Input.GetKeyDown(keycode[i])) 
                {
                    Bounce();
                }
            }
        }
    }

    public void Bounce() 
    {
        if (scaleCoroutine != null)
        {
            StopCoroutine(scaleCoroutine);
        }
        scaleCoroutine = StartCoroutine(ScaleSequence());
    }

    private IEnumerator ScaleSequence()
    {
        //isScaling = true;

        // Scale up to the target size
        yield return LerpScale(originalScale, targetScale);

        // Scale down to a slightly smaller size (using 0.9f multiplier for both x and y)
        Vector2 smallerScale = new Vector2(originalScale.x * 0.9f, originalScale.y * 0.9f);
        yield return LerpScale(targetScale, smallerScale);

        // Scale back to the original size
        yield return LerpScale(smallerScale, originalScale);

        //isScaling = false;
    }

    private IEnumerator LerpScale(Vector2 startScale, Vector2 endScale)
    {
        float elapsedTimeX = 0f;
        float elapsedTimeY = 0f;
        Vector2 initialScale = startScale;

        // Start the Y-axis scaling immediately, but delay the X-axis scaling by delayBeforeX
        while (elapsedTimeY < lerpDurationY || elapsedTimeX < lerpDurationX)
        {
            if (elapsedTimeY < lerpDurationY)
            {
                // Update the Y axis scaling
                float lerpFactorY = elapsedTimeY / lerpDurationY;
                transform.localScale = new Vector3(
                    transform.localScale.x,  // Keep the current X value for now
                    Mathf.Lerp(initialScale.y, endScale.y, lerpFactorY),
                    transform.localScale.z  // Keep the original z scale
                );
                elapsedTimeY += Time.deltaTime;
            }

            if (elapsedTimeX < lerpDurationX && elapsedTimeY > delayBeforeX)
            {
                // Start updating the X axis after the specified delay
                float lerpFactorX = elapsedTimeX / lerpDurationX;
                transform.localScale = new Vector3(
                    Mathf.Lerp(initialScale.x, endScale.x, lerpFactorX),
                    transform.localScale.y, // Keep the current Y value for now
                    transform.localScale.z  // Keep the original z scale
                );
                elapsedTimeX += Time.deltaTime;
            }

            yield return null;
        }

        // Ensure the final scale is exactly the target value
        transform.localScale = new Vector3(endScale.x, endScale.y, transform.localScale.z);
    }
}
