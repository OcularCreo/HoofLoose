using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleActivator : MonoBehaviour
{

    [SerializeField] private Transform circleIndicator;

    [SerializeField] float growthSpeed;

    
    // Start is called before the first frame update
    void Start()
    {
        circleIndicator.GetComponent<Transform>().localScale = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
