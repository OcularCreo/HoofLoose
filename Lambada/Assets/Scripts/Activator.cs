using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activator : MonoBehaviour
{

    [SerializeField] private KeyCode Keycode;   //key code associated with the activator
    public bool keyPressed;                     //keeps track of if key has been pressed

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //check if the accociated keycode has been pressed
        if(keyPressed && collision.gameObject.tag == "note")
        {
            //add one to combo
        } 
        else
        {
            //remove combo
        }
    }
}
