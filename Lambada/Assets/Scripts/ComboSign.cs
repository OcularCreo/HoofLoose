using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComboSign : MonoBehaviour
{
    [SerializeField] Animator signAnimator;
    [SerializeField] GameManager gameManager;
    [SerializeField] Image signImage;

    //private bool slideOut;
    private bool comboCashed;
    private int prevCombo;
    private int currentCombo;

    // Start is called before the first frame update
    void Start()
    {
        //slideOut = false;
        comboCashed = false;
        prevCombo = 0;
        currentCombo = 0;
    }

    // Update is called once per frame
    void Update()
    {
        // if the combo is a multiple of 8, and is not 0, play the animation to make the sign slide in
        /*        if((gameManager.GetComponent<GameManager>().combo % 8 == 0) && (gameManager.GetComponent<GameManager>().combo != 0))
                {
                    signAnimator.SetBool("NewCombo", true);
                    slideOut = true;
                }
                else if ((gameManager.GetComponent<GameManager>().combo % 8 == 1) || (gameManager.GetComponent<GameManager>().combo == 0))
                {
                    if (slideOut)
                    {
                        signAnimator.SetBool("ComboDone", true);
                        slideOut = false;
                    }
                }
                // this was such a good idea but it doesn't work because you can bypass 8 in the game
         */


        //

        currentCombo = gameManager.GetComponent<GameManager>().combo;

        if ((currentCombo / 8) > (prevCombo / 8))
        {
            signAnimator.SetBool("ComboDone", false);
            signAnimator.SetBool("NewCombo", true);

            // change sign colour
            ChangeColor();
            Debug.Log("change colour pls");
        } 
        else if (currentCombo < prevCombo)
        {
            signAnimator.SetBool("NewCombo", false);
            signAnimator.SetBool("ComboDone", true);
        }

        prevCombo = currentCombo;

    }

    private void ChangeColor()
    {
        // Generate a random hue value (between 0 and 1)
        float hue = Random.Range(0f, 1f);

        // Convert hue to RGB and apply it to the sprite renderer
        Color randomColor = Color.HSVToRGB(hue, 0.6f, 1f);
        signImage.color = randomColor;
    }
}
