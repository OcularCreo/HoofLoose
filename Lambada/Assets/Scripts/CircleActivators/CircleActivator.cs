using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Search;
using UnityEngine;

public class CircleActivator : MonoBehaviour
{
    [SerializeField] GameManager gameManager;

    [SerializeField] private Transform circleIndicator;
    [SerializeField] private float growthSpeed;
    [SerializeField] private KeyCode keyToPress;

    //particles
    [SerializeField] GameObject perfectParticle;
    [SerializeField] GameObject greatParticle;
    [SerializeField] GameObject goodParticle;
    [SerializeField] GameObject missedParticle;
    [SerializeField] Transform particleSpawnTrans;

    [SerializeField] private TextMeshProUGUI keyText;
    [SerializeField] private Sprite[] keyIndicatorSprites;
    [SerializeField] private SpriteRenderer keyIndicator;

	AudioManager audioManager;

	// Start is called before the first frame update
	void Start()
    {
        circleIndicator.GetComponent<Transform>().localScale = Vector3.zero;
		audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();

        matchSprite();
	}

    private void matchSprite()
    {
        //if there are sprite indicators and a keyIndicator sprite renderer
        if(keyIndicatorSprites.Length > 3 && keyIndicator) {

            //match set the set the sprite renderer to the matching indicator sprite
            if (keyToPress == KeyCode.LeftArrow)
            {
                keyIndicator.sprite = keyIndicatorSprites[0];
                keyText.enabled = false;
            }
            else if (keyToPress == KeyCode.RightArrow)
            {
                keyIndicator.sprite = keyIndicatorSprites[1];
                keyText.enabled = false;
            }
            else if (keyToPress == KeyCode.UpArrow)
            {
                keyIndicator.sprite = keyIndicatorSprites[2];
                keyText.enabled = false;
            }
            else if (keyToPress == KeyCode.DownArrow)
            {
                keyIndicator.sprite = keyIndicatorSprites[3];
                keyText.enabled = false;
            }
            //if the key to press for the object is not an arrow key then just edit the text gui
            else
            {
                keyText.text = keyToPress.ToString();
                keyIndicator.enabled = false;
            }

        } 
        //if there is no renderer or all the sprites aren't there then just write th eky to press as a string
        else
        {

            Debug.Log("Not enough or no renderer");
            keyText.text = keyToPress.ToString();
            keyIndicator.enabled = false;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        //check if the key to press has been pressed and the circleIndicator exists
        if(Input.GetKeyDown(keyToPress) && circleIndicator != null)
        {
            //calulate what percentage of the indicator's scale is of the static circle
            float scalePercentage = circleIndicator.lossyScale.x / transform.localScale.x;

            bool success = false;

            //check what percentage the size the indicator is relative to the static circle
            if(scalePercentage < 0.7)
            {
                missed();
				audioManager.PlaySFX(audioManager.miss);
			} else if(scalePercentage >= 0.7 && scalePercentage < 0.85)
            {
                Instantiate(goodParticle, particleSpawnTrans.position, Quaternion.identity);
                gameManager.combo += 1;
                success = true;
				audioManager.PlaySFX(audioManager.good);

			} else if(scalePercentage >= 0.85 && scalePercentage <0.9)
            {
                Instantiate(greatParticle, particleSpawnTrans.position, Quaternion.identity);
                gameManager.combo += 1;
                success = true;
				audioManager.PlaySFX(audioManager.great);

			} else if(scalePercentage >= 0.9 && scalePercentage <= 1.1)
            {
                gameManager.combo += 2;
                Instantiate(perfectParticle, particleSpawnTrans.position, Quaternion.identity);
                success = true;
                audioManager.PlaySFX(audioManager.perfect);
            } 
            else if (scalePercentage > 1.1)
            {
                missed();
				audioManager.PlaySFX(audioManager.miss);
			}

            //if the player successful in any way and the fail counter is greater than 0
            if(success)
            {
                gameManager.failCounter = 0;    //reset the fail counter

                //check if the max combo was reached
                if(gameManager.maxCombo < gameManager.combo)
                {
                    gameManager.maxCombo = gameManager.combo;
                }
            } 
            //if the player was not succesful
            else
            {
                StopAllCoroutines();
            }

            resetActivator();
            
        }

        if(Input.GetKeyDown(KeyCode.LeftShift) ||  Input.GetKeyDown(KeyCode.RightShift))
        {
            resetActivator();
        }
    }

    public void missed()
    {
        gameManager.failCounter++;
        gameManager.combo = 0;
        Instantiate(missedParticle, particleSpawnTrans.position, Quaternion.identity);
    }

    //returns the keycode of the activator
    public KeyCode getKeyCode()
    {
        return keyToPress;
    }

    public void resetActivator ()
    {
        gameObject.SetActive(false);
        circleIndicator.localScale = Vector3.zero;
    }

}
