using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class tutorialScript : MonoBehaviour {
    public int tutCount;
    AudioSource music;
    Sprite[] tutImages;
    Sprite[] cutImages;
    Image tutCurr;
    Image currCut;
    Image nextCut;
    int totalImages;
    int cutCount;
    float cutTimer;
    bool playing;
    float cutLength;
    float window;
    int totalCutCount;
    Text nextText;
    Text backText;
    // Use this for initialization
    void Start () {
        tutCount = 0;
        music = gameObject.GetComponent<AudioSource>();
        tutCurr = GameObject.Find("TutorialImage").GetComponent<Image>();
        currCut = GameObject.Find("CurrCut").GetComponent<Image>();
        nextCut = GameObject.Find("NextCut").GetComponent<Image>();
        nextText = GameObject.Find("Next").GetComponent<Text>();
        backText = GameObject.Find("Back").GetComponent<Text>();
        totalImages = 4;
        tutCount = 0;
        totalCutCount = 9;
        tutImages = new Sprite[totalImages];
        cutImages = new Sprite[totalCutCount];
        cutTimer = 0;
        playing = true;
        cutLength = 2.0f;
        cutCount = 0;
        window = 1;

        //find out whether using a controller or keyboard
        bool usingController = false; 
        foreach (string word in Input.GetJoystickNames())
        {
            //is using controller
            if (word != "")
            {
                usingController = true;
            }
           
        }

        //is using controller
        if (usingController)
        {
            for (int i = 0; i < totalImages; i++)
            {
                tutImages[i] = Resources.Load<Sprite>("Tutorial/Tutorial/t" + (i + 1));
            }
        }
        //using keyboard
        else
        {
            for (int i = 0; i < totalImages; i++)
            {
                tutImages[i] = Resources.Load<Sprite>("Tutorial/Tutorial/tk" + (i + 1));
            }
        }

        for (int i = 0; i < totalCutCount; i++)
        {
            cutImages[i] = Resources.Load<Sprite>("Cutscenes/Cutscenes/" + (i + 1));
        }

        tutCurr.gameObject.SetActive(false);

        if(GameModeControl.mode == 0)
        {
            music.Play();
        }

    }

    // Update is called once per frame
    void Update()
    {
        //go forward
        if ((Input.GetButtonDown("A") || Input.GetMouseButtonDown(0)) && !playing)
        {
            tutCount++;
            backText.text = "Back";
            changeImage(tutCount);
            if (tutCount >= totalImages - 1)
            {
                //load scene
                
                GameObject[] go = GameObject.FindGameObjectsWithTag("HUD");
                foreach (GameObject g in go)
                    g.gameObject.SetActive(false);
                GameModeControl.mode = 0;
                SceneManager.LoadScene(1);
            }
            else
            {
                //load next image
                

                if (tutCount == totalImages - 2)
                    nextText.text = "StartGame";
            }
        }

        //go back
        if ((Input.GetButtonDown("B") || Input.GetMouseButtonDown(1)) && !playing)
        {
            if (tutCount > 0)
            {
                tutCount--;
                changeImage(tutCount);
                nextText.text = "Next";
                if (tutCount == 0)
                    backText.text = "Main Menu";
            }
            else
            {
                //load start screne
                SceneManager.LoadScene(0);
            }
        }
        //cutscene
        if (playing)
        {
            if (GameModeControl.mode == 3)
                endCut();
            else
            {
                backText.gameObject.SetActive(false);
                nextText.text = "Skip";
                cutTimer += Time.deltaTime;
                if (cutTimer > cutLength)
                {
                    currCut.CrossFadeAlpha(0.0f, 0.4f, true);
                    nextCut.CrossFadeAlpha(1.0f, 0.001f, true);
                    cutCount++;
                    cutTimer = 0;
                }

                if (cutCount < totalCutCount)
                {
                    if (cutTimer > window)
                    {
                        Sprite s = cutImages[cutCount];
                        currCut.sprite = s;
                        if (cutCount < totalCutCount - 1)
                            nextCut.sprite = cutImages[cutCount + 1];

                        currCut.CrossFadeAlpha(1.0f, 0.0000001f, true);
                        nextCut.CrossFadeAlpha(0.0f, 0.0000001f, true);
                    }

                }
                else
                {
                    endCut();
                }

                if (Input.GetButtonDown("A") || Input.GetMouseButtonDown(0) || Input.GetButtonDown("B") || Input.GetMouseButtonDown(1))
                {
                    endCut();
                    music.Stop();
                }
            }
        }
        

    }


    void endCut()
    {
        tutCurr.gameObject.SetActive(true);
        currCut.gameObject.SetActive(false);
        nextCut.gameObject.SetActive(false);
        playing = false;
        backText.gameObject.SetActive(true);
        nextText.text = "Next";
        GameObject.Find("ACut").SetActive(false);
        if (GameModeControl.mode == 0)
        {
            changeImage(totalImages-1);
            GameObject[] go = GameObject.FindGameObjectsWithTag("HUD");
            foreach (GameObject g in go)
                g.gameObject.SetActive(false);
            SceneManager.LoadScene(1);
        }
        else
            changeImage(tutCount);
    }
    void changeImage(int i)
    {
        tutCurr.sprite = tutImages[i];
    }
}
