using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowingCircle : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] GameObject activator;
    
    public bool grow;
    
    [SerializeField] private float growSpeed;
    [SerializeField] private float maxScale;
    
    // Start is called before the first frame update
    void Start()
    {
        grow = true;
        transform.localScale = Vector3.zero;
    }

    /*
    private void OnEnable()
    {
        Debug.Log("On enabled called growing");
        grow = true;
    }
    */

    // Update is called once per frame
    void Update()
    {
        if(grow)
        {
            if(transform.localScale.x < maxScale)
            {
                transform.localScale += Vector3.one * growSpeed * Time.deltaTime;
            } else
            {
                transform.localScale = Vector3.zero;
                gameManager.combo = 0;
                activator.SetActive(false);
            }
        }
    }

}
