
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using Steamworks;


public class MenuScript : MonoBehaviour {
	public Button startText;
	//public Button exitText;
	public Button exitMenuText;
    public Button exitCredits;
	public Button ControlsText;
    public Button floodButton;
    public Button doubleButton;
	public Canvas ControlMenu;
	public Canvas MainCanvas;
    public Image loading;
    public Canvas credits;
	// Use this for initialization
	void Start () {
		ControlMenu.enabled = false;
		startText = startText.GetComponent<Button> ();
		startText.image.enabled = false;
		ControlsText.image.enabled = false;
		exitMenuText = exitMenuText.GetComponent<Button> ();
        exitCredits = exitCredits.GetComponent<Button>();
		startText.image.enabled = true;
		ControlsText.image.enabled = true;
        loading.enabled = false;
        credits.enabled = false;
		startText.Select ();
        //detect keyboard
        bool usingController = false;
        foreach (string word in Input.GetJoystickNames())
        {
            if (word != "")
            {
                usingController = true;
            }
        }

        if (!usingController)
        {
            ControlMenu.gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("controlkey");
        }

    }
	public void ExitPress()
	{
		startText.Select (); // exits controls screen and reselects startText
		ControlMenu.enabled = false;

    }

	public void StartLevel()
	{
        SceneManager.LoadScene(2);
        GameModeControl.mode = 0;
        loading.enabled = true;

    }

    public void StartTutorial()
    {
        SceneManager.LoadScene(2);
        GameModeControl.mode = 3;
    }

    public void StartFlood()
    {
        SceneManager.LoadScene(1);
        GameModeControl.mode = 1;
        loading.enabled = true;
    }
    public void StartDouble()
    {
        SceneManager.LoadScene(1);
        GameModeControl.mode = 2;
        loading.enabled = true;
    }
    public void ShowControl()
	{
		ControlMenu.enabled = true;
        MainCanvas.enabled = false;
		exitMenuText.Select ();

	}
	public void HideControl()
	{
		ControlMenu.enabled = false;
        MainCanvas.enabled = true;
		startText.Select ();

    }

    public void ShowCredits()
    {
        credits.enabled = true;
        MainCanvas.enabled = false;
        exitCredits.Select();
        try
        {
            SteamUserStats.SetAchievement("credits");
            SteamUserStats.StoreStats();
        }
        catch
        {

        }

    }
    public void HideCredits()
    {
        credits.enabled = false;
        MainCanvas.enabled = true;
        startText.Select();

    }

    public void ExitGame()
	{
		Application.Quit();
	}
	
	// Update is called once per frame
	void Update () {

	}
}
