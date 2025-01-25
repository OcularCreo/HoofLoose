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

    // Start is called before the first frame update
    void Start()
    {
        circleIndicator.GetComponent<Transform>().localScale = Vector3.zero;
        keyText.text = keyToPress.ToString();
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
            if(scalePercentage < 0.6)
            {
                missed();
            } else if(scalePercentage >= 0.6 && scalePercentage < 0.85)
            {
                Instantiate(goodParticle, particleSpawnTrans.position, Quaternion.identity);
                gameManager.combo += 1;
               
            } else if(scalePercentage >= 0.85 && scalePercentage <0.9)
            {
                Instantiate(greatParticle, particleSpawnTrans.position, Quaternion.identity);
                gameManager.combo += 1;

            } else if(scalePercentage >= 0.9 && scalePercentage <= 1.05)
            {
                gameManager.combo += 2;
                Instantiate(perfectParticle, particleSpawnTrans.position, Quaternion.identity);
            } 
            else if (scalePercentage > 1.05)
            {
                missed();
            }

            //if the player successful in any way and the fail counter is greater than 0
            if(success)
            {
                gameManager.failCounter = 0;    //reset the fail counter
            } 
            //if the player was not succesful
            else
            {
                gameManager.failCounter++;      
            }

            circleIndicator.localScale = Vector3.zero;
            gameObject.SetActive(false);
            
        }

        if(Input.GetKeyDown(KeyCode.LeftShift) ||  Input.GetKeyDown(KeyCode.RightShift))
        {
            circleIndicator.localScale = Vector3.zero;
            gameObject.SetActive(false);
        }
    }

    public void missed()
    {
        gameManager.failCounter++;
        Instantiate(missedParticle, particleSpawnTrans.position, Quaternion.identity);
    }

}
