using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public int combo;
    public int failCounter;
    public int lives;

    [SerializeField] private GameObject[] keyActivators;

    [SerializeField] private TextMeshProUGUI comboTxt;
    [SerializeField] private TextMeshProUGUI sheepTxt;
   
    private List<GameObject> keyActivatorsList;
    private bool shiftDown;

    private float time = 0;

    [SerializeField] private SheepManager sheepManager;

    [SerializeField] private ParticleSystem poseParticle;
    private GameObject player;

    [SerializeField] AudioManager audioManager;

    // Start is called before the first frame update
    void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();

        if (sheepManager == null)
        {
            sheepManager = GameObject.FindGameObjectWithTag("SheepManager").GetComponent<SheepManager>();
        }

        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        shiftDown = false;

        resetKeyList();

        StartCoroutine(activate(0));

    }

    private void resetKeyList()
    {
        if (keyActivatorsList != null)
        {
            keyActivatorsList.Clear();                  //clear the list
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
        if (failCounter > 2)
        {
            lives--;
            if (lives < 0) 
            {
                lives = 0;
            }

            sheepManager.KillSheep(1);
            failCounter = 0;
        }

        //staring mechanic controls
        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
        {
            audioManager.PlaySong(audioManager.wtf);

            shiftDown = true;       //set shift down to true
            StopAllCoroutines();    //stop all coroutines
            time = Time.time;       //log the first time

        } else if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.RightShift))
        {
            audioManager.PlaySong(audioManager.backgroundMusic);

            shiftDown = false;
            StartCoroutine(activate(0));
            time = 0;
        }

        //while shift is being held down
        if(shiftDown)
        {
            float elapsedTime = Time.time - time;
            
            if(elapsedTime >= 3)
            {
                lives-=2;
                sheepManager.KillSheep(2);
                time = Time.time;
            }

        }

        //when player hits the pose button
        if (Input.GetKeyDown(KeyCode.Space))
        {
            audioManager.PlaySFX(audioManager.twerk);

            int iterations = combo / 8; //check if their combo is high enough to gain sheep
            int gainedLives = 0;        //variable to count how many lives they gained

            //use fibinache sequence to calculate how many sheep/lives to add
            int prev = 0;
            int next = 1;

            for (int i = 0; i < iterations; i++)
            {
                gainedLives = prev + next;

                if(gainedLives >= 55)
                {
                    break;
                }

                prev = next;
                next = gainedLives;
            }

            //add the amount of lives they gained
            lives += gainedLives;

            //ensure the sheep manager and player exist
            if (sheepManager && player)
            {
                //cap spawning 55 sheep
                if(lives < 55)
                {
                    sheepManager.SubmitCombo(gainedLives); //add sheep to represent lives
                } 

                PoseParticle();                            //instantiate the pose particles
            }

            combo = 0; //reset their combo to zero
            StopAllCoroutines();
            StartCoroutine(activate(1f));
        }

        //make sure that the combo text and sheep text exist
        if (comboTxt && sheepTxt)
        {
            comboTxt.text = combo.ToString();               //update the combo text string
            sheepTxt.text = "Sheep: " + lives.ToString();   //update the sheep text string
        }
    }

    IEnumerator activate(float waitTime)
    {

        yield return new WaitForSecondsRealtime(waitTime);

        if (!shiftDown)
        {
            resetKeyList();
            activateKey();

            // 40% chance of double key press
            if (Random.value <= 0.3)
            {
                activateKey();
            }

            float activateTime = 1.1f - ((combo / 8) * 0.15f);
            activateTime = Mathf.Clamp(activateTime, 0.7f, 1.1f);

            StartCoroutine(activate(activateTime));
        }

    }

    private void activateKey()
    {
        int selectedIdx = Random.Range(0, keyActivatorsList.Count);
        keyActivatorsList[selectedIdx].SetActive(true);
        keyActivatorsList.RemoveAt(selectedIdx);
    }

    private void PoseParticle()
    {
        if (poseParticle) 
        {
            Instantiate(poseParticle, player.transform.position, Quaternion.identity);
        }
    }
}
