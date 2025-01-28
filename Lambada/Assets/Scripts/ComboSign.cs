using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComboSign : MonoBehaviour
{
    [SerializeField] Animator signAnimator;
    [SerializeField] GameManager gameManager;
    [SerializeField] Image signImage;

    private int prevCombo;
    private int currentCombo;

    // Start is called before the first frame update
    void Start()
    {
        prevCombo = 0;
        currentCombo = 0;
    }

    // Update is called once per frame
    void Update()
    {
        // get the current combo number
        currentCombo = gameManager.GetComponent<GameManager>().combo;

        // if the player reached a new multiple of 8
        if ((currentCombo / 8) > (prevCombo / 8))
        {
            if(currentCombo >= 16) // if this is not the first combo
            {
                // sign slides out then back in
                signAnimator.SetBool("NewCombo", false);
                signAnimator.SetBool("ComboDone", true);
                signAnimator.SetBool("New8", true);
            }
            else // if it's the first combo
            {
                // sign slides in 
                signAnimator.SetBool("ComboDone", false);
                signAnimator.SetBool("New8", false);
                signAnimator.SetBool("NewCombo", true);
            }

            // change sign colour
            ChangeColor();
        } 
        // else if player cashed in or lost their combo
        else if (currentCombo < prevCombo)
        {
            // sign slides out
            signAnimator.SetBool("NewCombo", false);
            signAnimator.SetBool("ComboDone", true);
            signAnimator.SetBool("New8", false);
        }
        else
        {
            signAnimator.SetBool("ComboDone", false); // sign stays onscreen
        }

        prevCombo = currentCombo;

    }

    private void ChangeColor()
    {
        // Generate a random hue value (between 0 and 1)
        float hue = Random.Range(0f, 1f);

        // Convert hue to RGB and apply it to the sprite renderer
        Color randomColor = Color.HSVToRGB(hue, 0.8f, 0.6f);
        signImage.color = randomColor;
    }
}
