using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SheepManager : MonoBehaviour
{
    [SerializeField] private SheepSpawner spawner;

    private GameObject[] sheep;
    private int numDancingSheep;
    private int numGrazingSheep;

    private GameObject[] sheepList;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("l")) 
        {
            SubmitCombo(20);
        }
        if (Input.GetKeyDown("k"))
        {
            KillSheep(15);
        }
    }

    public int CountDancingSheep() 
    {
        sheep = GameObject.FindGameObjectsWithTag("Sheep");

        numDancingSheep = 0;

        for (int i = 0; i < sheep.Length - 1; i++) 
        {
            if (sheep[i].GetComponent<SheepBehaviour>().GetState() == SheepBehaviour.SheepState.Dance) 
            {
                numDancingSheep++;
            }
        }

        return numDancingSheep;
    }

    public int CountGrazingSheep()
    {
        sheep = GameObject.FindGameObjectsWithTag("Sheep");

        numGrazingSheep = 0;

        for (int i = 0; i < sheep.Length - 1; i++)
        {
            if (sheep[i].GetComponent<SheepBehaviour>().GetState() == SheepBehaviour.SheepState.Graze)
            {
                numGrazingSheep++;
            }
        }

        return numGrazingSheep;
    }

    public void SpawnSheep(int sheepToSpawn) 
    {
        spawner.SpawnSheep(sheepToSpawn);
    }

    public void SubmitCombo(int comboVal) 
    {
        // Find all sheep with the "Sheep" tag
        sheepList = GameObject.FindGameObjectsWithTag("Sheep");

        // Create a list of sheep that are in the "Dance" state
        var grazingSheep = new System.Collections.Generic.List<Transform>();

        foreach (var sheep in sheepList)
        {
            SheepBehaviour.SheepState sheepState = sheep.GetComponent<SheepBehaviour>().GetState();
            if (sheepState == SheepBehaviour.SheepState.Graze)
            {
                grazingSheep.Add(sheep.transform);
            }
        }

        //Debug.Log(CountDancingSheep());
        //Debug.Log(grazingSheep.Count);

        for (int i = 0; i < comboVal; i++) 
        {
            if (i < grazingSheep.Count)
            {
                grazingSheep[i].GetComponent<SheepBehaviour>().TransitionToDanceState();
            }
            else
            {
                GameObject sheep = spawner.SpawnSheepReturnSheep();
                sheep.GetComponent<SheepBehaviour>().TransitionToDanceState();
            }
        }
    }

    public void KillSheep(int sheepToKill) 
    {
        // Find all sheep with the "Sheep" tag
        sheepList = GameObject.FindGameObjectsWithTag("Sheep");

        // Create a list of sheep that are in the "Dance" state
        var dancingSheep = new System.Collections.Generic.List<Transform>();

        foreach (var sheep in sheepList)
        {
            SheepBehaviour.SheepState sheepState = sheep.GetComponent<SheepBehaviour>().GetState();
            if (sheepState == SheepBehaviour.SheepState.Dance)
            {
                dancingSheep.Add(sheep.transform);
            }
        }

        for (int i = 0; i < sheepToKill; i++)
        {
            if (i < dancingSheep.Count)
            {
                Destroy(dancingSheep[i].gameObject);
            }
            else
            {
                // Lose Game?
                Debug.Log("You lose?");
            }

        }

        //Debug.Log("Attempted to kill " + sheepToKill + " sheep");
    }
}
