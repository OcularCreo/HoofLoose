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
        /*
        if (Input.GetKeyDown("l")) 
        {
            SubmitCombo(20);
        }
        if (Input.GetKeyDown("k"))
        {
            KillSheep(15);
        }*/
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
        // Create a list of sheep that are in the "Dance" state
        var grazingSheep = GetAllStateSheep(SheepBehaviour.SheepState.Graze);

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
        // Create a list of sheep that are in the "Dance" state
        var dancingSheep = GetAllStateSheep(SheepBehaviour.SheepState.Dance);

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

    public GameObject GetRandomStateSheep(SheepBehaviour.SheepState state) 
    {
        GameObject theChosenOne;

        // Create a list of sheep that are in the "Dance" state
        var stateList = GetAllStateSheep(state);

        if (stateList.Count > 0) 
        {
            int randomSheep = Random.Range(0, stateList.Count);

            theChosenOne = stateList[randomSheep];

            return theChosenOne;
        }
        else 
        {
            return null;
        }
    }

    public int GetNumberStateSheep(SheepBehaviour.SheepState state)
    {
        int numSheep = GetAllStateSheep(state).Count;

        return numSheep;
    }

    public List<GameObject> GetAllStateSheep(SheepBehaviour.SheepState state)
    {
        // Find all sheep with the "Sheep" tag
        sheepList = GameObject.FindGameObjectsWithTag("Sheep");

        // Create a list of sheep that are in the "Dance" state
        var stateList = new System.Collections.Generic.List<GameObject>();

        foreach (var sheep in sheepList)
        {
            SheepBehaviour.SheepState sheepState = sheep.GetComponent<SheepBehaviour>().GetState();
            if (sheepState == state)
            {
                stateList.Add(sheep);
            }
        }

        return stateList;
    }
}
