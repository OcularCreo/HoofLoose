using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int combo;
    public int failCounter;
    public int lives;

    [SerializeField] private GameObject[] keyActivators;

    private int numActivators;

    private List<GameObject> keyActivatorsList;

    [SerializeField] private SheepManager sheepManager;
    
    // Start is called before the first frame update
    void Start()
    {
        numActivators = keyActivators.Length;

        resetKeyList();

        StartCoroutine(activate(0)); 

    }

    private void resetKeyList ()
    {
        if (keyActivatorsList != null)
        {
            keyActivatorsList.Clear();                      //clear the list
        }
        
        keyActivatorsList = new List<GameObject>();     //allocate the list

        //add each item to the list
        foreach (GameObject item in keyActivators)
        {
            keyActivatorsList.Add(item);
        }

    }

    // Update is called once per frame
    void Update()
    {
        if(failCounter > 2)
        {
            lives--;
        }

        if(Input.GetKeyDown(KeyCode.E))
        {

            int iterations = combo / 8;
            int gainedLives = 0;

            int prev = 0;
            int next = 1;

            for(int i = 0; i < iterations; i++)
            {
                gainedLives = prev + next;
                prev = next; 
                next = gainedLives;
            }

            lives = gainedLives;
            sheepManager.GetComponent<SheepManager>().SubmitCombo(gainedLives);
            combo = 0; 
        }
    }

    IEnumerator activate(float waitTime)
    {
        
        yield return new WaitForSecondsRealtime(waitTime);

        resetKeyList();
        activateKey();

        // 40% chance of double key press
        if(Random.value <= 0.3)
        {
            StartCoroutine(activateSecondary(0.25f));
        }

        StartCoroutine(activate(1.25f));

    }

    IEnumerator activateSecondary (float waitTime)
    {
        yield return new WaitForSecondsRealtime(waitTime);
        activateKey();

    }

    private void activateKey ()
    {
        int selectedIdx = Random.Range(0, keyActivatorsList.Count);
        keyActivatorsList[selectedIdx].SetActive(true);
        keyActivatorsList.RemoveAt(selectedIdx);
    }
}
