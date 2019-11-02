using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SystemManager : MonoBehaviour
{
    // The number of balls we want to have pooled
    int numberOfBallsToInstantiate = 10;

    // Two integers for a round timer
    int roundTime = 60;

    int originalRoundTime;

    // Two integers for the timer to start in tutorial 2
    int countDownTimedAmount = 3;

    int originalCountDownAmount;

    int score = 0;

    // The amount for the score
    Text scoreAmount;

    // The current timer value
    Text timer;

    // Missing shots amount Text
    Text Misses;
    // Amount of misses
    public int amountofMisses = 0;
    // Baskets Made amount Text
    Text BasketsMade;
    //Amount of Baskets made
    public int amountofBaskets = 0;

    // Timed Played amount Text
    Text TimePlayed;
    // Time Played Amount Float
    float Timeplay;

    //High Score Text
    Text HighScoreText;
    //High Score
    int HighScore = 0;

    [SerializeField]
    Transform ballsParent;

    // A reference to the type of camera we are using
    [SerializeField]
    GameObject CameraMode;

    // The two types of cameras we will be using
    //GameObject ARGOBJSParent;

    //GameObject normalCam;

    [SerializeField]
    GameObject interaction;

    // The timer for the countDown before starting
    Text countDownToStartText;

    // A list of the panels in the scene
    List<Transform> panels = new List<Transform>();

    // All of the buttons active in our scene
    List<List<Button>> allButtons = new List<List<Button>>();

    // A list of the randomized balls we will be using
    [SerializeField]
    List<GameObject> balls = new List<GameObject>();

    ARModifedTapToPlaceObject arPlacement;

    // A function for managing our canvas in the scene
    void CollectCanvasInfo()
    {
        //Find our panels based on the canvas children and that they have an image
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).GetComponent<Image>() != null)
                panels.Add(transform.GetChild(i));
            allButtons.Add(new List<Button>());
        }

        //Find Buttons for each panel and make sure it is a button
        for (int i = 0; i < panels.Count; i++)
        {
            for (int j = 0; j < panels[i].childCount; j++)
                if (panels[i].GetChild(j).GetComponent<Button>() != null)
                    allButtons[i].Add(panels[i].GetChild(j).GetComponent<Button>());
        }

        //Set the score amount textfield equal to a reference
        scoreAmount = panels[3].Find("Score").Find("ScoreAmount").GetComponent<Text>();
        timer = panels[3].Find("Timer").transform.GetChild(0).GetComponent<Text>();
        countDownToStartText = panels[3].Find("CountDown").GetChild(0).GetComponent<Text>();
        Misses = panels[4].Find("Misses").Find("miAmount").GetComponent<Text>();
        BasketsMade = panels[4].Find("Baskets Made").Find("baAmount").GetComponent<Text>();
        TimePlayed = panels[4].Find("TimePlayed").Find("tiAmount").GetComponent<Text>();
        HighScoreText = panels[4].Find("HighScore").Find("hiAmount").GetComponent<Text>();
        AddListenersToButtons();
    }

    // A function for organizing the canvas info
    void SetCurrentPanel(int panelToSetActive)
    {
        // Set all other panels to inactive minus the current transform we want
        for (int i = 0; i < panels.Count; i++)
            if (i != panelToSetActive)
                panels[i].gameObject.SetActive(false);
            else
                panels[i].gameObject.SetActive(true);

        // Choose what camera we want active in our game scene based on if we are in tutorials 1 or 2 or not in either
        if(panels[2].gameObject.activeInHierarchy || panels[3].gameObject.activeInHierarchy)
        {
            //SetCameraTypes(0);
            if(panels[2].gameObject.activeInHierarchy)
            {
                interaction.SetActive(true);
            }
            if (panels[3].gameObject.activeInHierarchy)
            {
                interaction.GetComponent<ARModifedTapToPlaceObject>().enabled = true;
                interaction.GetComponent<ARTapToPlaceObject>().enabled = false;
            }
        }
        else
        {
            ResetTutorial2();            
            //SetCameraTypes(1);
        }
    }

    //// A function for choosing what type of camera is being used in the active panel
    //void SetCameraTypes(int activeCam)
    //{
    //    // If active Cam is 0 than we want the normal camera. Otherwise we are using AR Camera
    //    if(activeCam == 0)
    //    {
    //        normalCam.SetActive(true);
    //        ARGOBJSParent.SetActive(false);
    //    }
    //    else
    //    {
    //        normalCam.SetActive(false);
    //        ARGOBJSParent.SetActive(true);
    //    }
    //}

    void ResetTutorial2()
    {
        for (int i = 0; i < ballsParent.transform.childCount; i++)
        {
            if (balls[i].activeInHierarchy)
            {
                balls[i].SetActive(true);
                break;
            }
        }
        interaction.SetActive(false);
        interaction.GetComponent<ARModifedTapToPlaceObject>().enabled = false;
        interaction.GetComponent<ARTapToPlaceObject>().enabled = true;
        arPlacement.IsPlaced = false;
        StopCoroutine(Tutorial2RoundTimer());
        StopCoroutine(Tutorial2StartCountDown());
        timer.text = originalRoundTime.ToString();
        countDownToStartText.text = originalCountDownAmount.ToString();
    }
    void AddListenersToButtons()
    {
        // Add listeners to all of the buttons

        //Main Menu Buttons
        allButtons[0][0].onClick.AddListener(InstructionsIsClicked);
        allButtons[0][1].onClick.AddListener(Tutorial1IsClicked);
        allButtons[0][2].onClick.AddListener(Tutorial2IsClicked);
        allButtons[0][3].onClick.AddListener(QuitIsClicked);

        //Instruction Buttons
        allButtons[1][0].onClick.AddListener(ReturnIsClicked);

        //Tutorial 1 Buttons
        allButtons[2][0].onClick.AddListener(ReturnIsClicked);


        //Tutorial 2 Buttons
        allButtons[3][0].onClick.AddListener(ReturnIsClicked);

        //Stat Buttons
        allButtons[4][0].onClick.AddListener(ReturnIsClicked);
    }

    void InstructionsIsClicked()
    {
        SetCurrentPanel(1);
    }

    void Tutorial1IsClicked()
    {
        SetCurrentPanel(2);
    }

    void Tutorial2IsClicked()
    {
        SetCurrentPanel(3);
    }

    void QuitIsClicked()
    {
        Application.Quit();
    }

    void ReturnIsClicked()
    {
        SetCurrentPanel(0);
    }

    // A function serving as the round Timer
    IEnumerator Tutorial2RoundTimer()
    {
        yield return new WaitForSeconds(1);
        timer.text = roundTime.ToString();

        if(roundTime >= 0)
        {
            roundTime -= 1;
            StartCoroutine(Tutorial2RoundTimer());
        }
        else
        {
            roundTime = originalRoundTime;
            StopCoroutine(Tutorial2RoundTimer());

            // Set the stats here
            SetStats();
            SetCurrentPanel(4);
        }
    }

    // A function serving as a countdown Timer
    IEnumerator Tutorial2StartCountDown()
    {
        yield return new WaitForSeconds(1);
        countDownToStartText.text = countDownTimedAmount.ToString();

        if(countDownTimedAmount >= 0)
        {
            countDownTimedAmount -= 1;
            StartCoroutine(Tutorial2StartCountDown());
        }
        else
        {
            countDownToStartText.transform.parent.gameObject.SetActive(false);
            countDownTimedAmount = originalCountDownAmount;
            StopCoroutine(Tutorial2StartCountDown());
            StartCoroutine(Tutorial2RoundTimer());
        }
    }

    void Awake()
    {
        for (int i = 0; i < numberOfBallsToInstantiate; i++)
        {
            balls[0].SetActive(false);
            Instantiate(balls[0], ballsParent);
        }

        // Find a reference to the armodifed tap to placce script
        arPlacement = interaction.GetComponent<ARModifedTapToPlaceObject>();

        // Set the current countdown value back to te original amount
        originalCountDownAmount = countDownTimedAmount;

        // Set the current roundtime value back to te original amount
        originalRoundTime = roundTime;

        // Assign the active camera
        //normalCam = CameraMode.transform.GetChild(0).gameObject;
        //ARGOBJSParent = CameraMode.transform.GetChild(1).gameObject;

        // Fill the base info for the game to work
        //SetCameraTypes(0);
        CollectCanvasInfo();
        SetCurrentPanel(0);

        //Set the text value references
        countDownToStartText.text = countDownTimedAmount.ToString();
        timer.text = roundTime.ToString();
    }

    void ObjectIsPlaced()
    {
        if(arPlacement.IsPlaced)
        {
            for (int i = 0; i < balls.Count; i++)
            {
                if(balls[i].activeInHierarchy)
                {
                    balls[i].gameObject.SetActive(true);
                    break;
                }
            }
            StartCoroutine(Tutorial2StartCountDown());
        }
    }

    // A function for setting the stats
    void SetStats()
    {
        Timeplay += (originalRoundTime - roundTime);
        
        TimePlayed.text = Timeplay.ToString();
        if (score >= HighScore)
        {
            HighScore = score;
        }
        HighScoreText.text = HighScore.ToString();
        Misses.text = amountofMisses.ToString();
        BasketsMade.text = amountofBaskets.ToString();

    }

    void Update()
    {
        //ObjectIsPlaced();
    }

    public int Score
    {
        get { return score; }

        set { score = value; }
    }
}
