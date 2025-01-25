using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowingCircle : MonoBehaviour
{
    public bool grow;
    
    [SerializeField] private float growSpeed;
    [SerializeField] private float maxScale;
    
    private Vector3 originalScale;
    
    // Start is called before the first frame update
    void Start()
    {
        grow = false;
        originalScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        if(grow)
        {
            if(transform.localScale.x < maxScale)
            {
                transform.localScale += Vector3.one * growSpeed * Time.deltaTime;
            }
        }
    }
}
