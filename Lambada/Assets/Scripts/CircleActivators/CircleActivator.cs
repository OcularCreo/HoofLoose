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

            //check what percentage the size the indicator is relative to the static circle
            if(scalePercentage < 0.6)
            {
                Debug.Log("too early");
                gameManager.combo = 0;
            } else if(scalePercentage >= 0.6 && scalePercentage < 0.85)
            {
                Debug.Log("good!");
                gameManager.combo += 1;
               
            } else if(scalePercentage >= 0.85 && scalePercentage <0.9)
            {
                Debug.Log("Great!");
                gameManager.combo += 1;
            } else if(scalePercentage >= 0.9 && scalePercentage <= 1.05)
            {
                gameManager.combo += 2;
                Debug.Log("Perfect");
            } 
            else if (scalePercentage > 1.05)
            {
                Debug.Log("Missed!");
                gameManager.combo = 0;
            }

            circleIndicator.localScale = Vector3.zero;
            gameObject.SetActive(false);
            
        }
    }

}
