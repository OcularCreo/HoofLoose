using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int combo;

    [SerializeField] private GameObject[] keyActivators;

    private int numActivators;
    
    // Start is called before the first frame update
    void Start()
    {
        numActivators = keyActivators.Length;
        StartCoroutine(activate(0)); 

    }

    // Update is called once per frame
    void Update()
    {
     
    }

    IEnumerator activate(float waitTime)
    {
        
        yield return new WaitForSecondsRealtime(waitTime);

        keyActivators[Random.Range(0, numActivators)].SetActive(true);

        StartCoroutine(activate(Random.Range(2, 4)));

    }
}
