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

    public int maxCombo;
    private int maxLives;

    [SerializeField] private GameObject[] keyActivators;
    public KeyCode[] existingKeys;

    [SerializeField] private TextMeshProUGUI comboTxt;
    [SerializeField] private TextMeshProUGUI sheepTxt;
    [SerializeField] private GameObject countDownTxt;
    [SerializeField] private float countDownTimer;
    
    private bool runGame;
    private bool showScore;

    private List<GameObject> keyActivatorsList;
    private bool shiftDown;

    private float time = 0;

    [SerializeField] private SheepManager sheepManager;

    [SerializeField] private ParticleSystem poseParticle;
    private GameObject player;

    [SerializeField] AudioManager audioManager;

    // bubble butt stuff
    private int twerkCount;
    [SerializeField] private float amountToTwerk;
    private bool spaceKeyJustPressed;
    public bool win;

    [SerializeField] private GameObject scoreCanvas;

    [SerializeField] private TextMeshProUGUI maxComboTxt;
    [SerializeField] private TextMeshProUGUI maxSheepTxt;
    [SerializeField] private TextMeshProUGUI currentSheepTxt;
    [SerializeField] private TextMeshProUGUI buttTxt;

    // Start is called before the first frame update
    void Start()
    {
        setupGame();
    }

    public void setupGame()
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

        //create an index for each key activator
        existingKeys = new KeyCode[keyActivators.Length];

        //get the key code of each activator
        for (int i = 0; i < keyActivators.Length; i++)
        {
            existingKeys[i] = keyActivators[i].GetComponent<CircleActivator>().getKeyCode();
        }

        shiftDown = false;
        runGame = false;

        comboTxt.text = "";
        sheepTxt.text = "";

        // bubble butt stuff
        twerkCount = 0;
        spaceKeyJustPressed = false;
        win = false;
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

        //if the count down timer is above 0
        if(countDownTimer >= 0)
        {
            countDownTxt.GetComponent<TextMeshProUGUI>().text = ((int)countDownTimer).ToString();   //display the time in whole numbers
            countDownTimer -= Time.deltaTime;                                                       //track the change in time

        } 
        //star the game if the timer is below or equal to zero
        else
        {
            
            //if timer text is active at this point, make it inactive
            if(runGame == false && showScore == false)
            {
                countDownTxt.SetActive(false);
                resetKeyList();
                StartCoroutine(activate(0));
                runGame = true;
            } 
            //if the game hasn't ended
            else if(!showScore)
            {
                if (failCounter > 2)
                {
                    lives--;
                    sheepManager.KillSheep(1);
                    failCounter = 0;
                }

                if (lives < 0)
                {
                    lives = 0;

                    runGame = false;    //stop running the game
                    showScore = true;   //set show score to true to be able to tell that the score is showing
                    win = false;        //set win to false

                    endGame();
                }

                handleShiftInput();
                checkWrongKeyPress();
                BubbleButt();
                checkComboSubmit();
                
            }
            
        }

        //make sure that the combo text and sheep text exist
        if (comboTxt && sheepTxt && runGame)
        {
            comboTxt.text = combo.ToString();               //update the combo text string
            sheepTxt.text = "Sheep: " + lives.ToString();   //update the sheep text string
        }

    }

    private void endGame()
    {
        //make all activators inactive
        for (int i = 0; i < keyActivators.Length; i++)
        {
            keyActivators[i].SetActive(false);
        }

        //stop all other actions in the game
        StopAllCoroutines();

        //show the score canvas
        if (scoreCanvas)
        {
            scoreCanvas.SetActive(true);

            maxComboTxt.text = maxCombo.ToString();
            maxSheepTxt.text = maxLives.ToString();
            currentSheepTxt.text = lives.ToString();
            buttTxt.text = (twerkCount / amountToTwerk).ToString();
        }

    }

    private void handleShiftInput()
    {
        //staring stare controles
        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
        {
            audioManager.PlaySong(audioManager.wtf);

            shiftDown = true;       //set shift down to true
            StopAllCoroutines();    //stop all coroutines
            time = Time.time;       //log the first time

        }
        else if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.RightShift))
        {
            audioManager.PlaySong(audioManager.backgroundMusic);

            shiftDown = false;
            StartCoroutine(activate(0));
            time = 0;
        }

        //while shift is being held down
        if (shiftDown)
        {
            float elapsedTime = Time.time - time;

            if (elapsedTime >= 3)
            {
                lives -= 2;
                sheepManager.KillSheep(2);
                time = Time.time;
            }

        }
    }

    private void checkWrongKeyPress()
    {
        //bool helps keep track of if the player has hit the wrong key
        bool wrongKey = false;

        //check if all they pressed a keycode corisponding to an activator that isn't active
        for (int i = 0; i < existingKeys.Length; i++)
        {
            //first time they have hit a key for an inactive activator, break the loop and set wrong key to false
            if (Input.GetKeyDown(existingKeys[i]) && !keyActivators[i].activeSelf)
            {
                wrongKey = true;
                break;
            }
        }

        //if they hit the wrong key find all active activators and call their missed function and set them to inactive
        if (wrongKey)
        {
            for (int i = 0; i < existingKeys.Length; i++)
            {
                if (keyActivators[i].activeSelf)
                {
                    keyActivators[i].GetComponent<CircleActivator>().missed();
                    keyActivators[i].GetComponent<CircleActivator>().resetActivator();
                }
            }
        }
    }

    private void checkComboSubmit()
    {
        // if any key is pressed 
        if (Input.anyKeyDown)
        {
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

                    if (gainedLives >= 55)
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
                    if (lives < 55)
                    {
                        sheepManager.SubmitCombo(gainedLives); //add sheep to represent lives
                    }

                    PoseParticle();                            //instantiate the pose particles
                }

                // bubble butt
                spaceKeyJustPressed = true;

                //check if max sheep has been beaten 
                if(maxLives < lives)
                {
                    maxLives = lives;
                }

                combo = 0; //reset their combo to zero
                StopAllCoroutines();
                StartCoroutine(activate(1f));
            }
            else
            {
                // if a key is pressed that isn't the space bar
                spaceKeyJustPressed = false;
            }
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

    //get function for the activators
    public GameObject[] getActivators()
    {
        return keyActivators;
    }

    // bubble butt function
    public void BubbleButt()
    {
        // if player hits space button more than once in a row, add to twerk count
        if (spaceKeyJustPressed)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                twerkCount += 1;
                Debug.Log("twerked: " + twerkCount);
            }
        }

        // if player reaches the amount of twerks to win
        if(twerkCount >= 10) // i made it 10 for now for testing
        {
            win = true;
        }
    }
}
