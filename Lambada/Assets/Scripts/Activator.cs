using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activator : MonoBehaviour
{

    [SerializeField] private KeyCode keyToPress;   //key code associated with the activator
    private GameObject key;                        //GameObject to track what is in the activator

    // Start is called before the first frame update
    void Start()
    {
        key = null;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(keyToPress))
        {
            if(key != null)
            {
                Debug.Log("hit");
            } else
            {
                Debug.Log("Missed");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {

        //check if object in the collider is a note
        if(col.gameObject.CompareTag("Note"))
        {
            key = col.gameObject;
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.gameObject.CompareTag("Note"))
        {
            key = null;
        }
    }
}
