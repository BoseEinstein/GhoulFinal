using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class PauseScreen : MonoBehaviour {

	public Canvas PauseMenu;
	public Image ControlsImage;
	public Button resumeText;
	public Button restartText;
	public Button exitText;
	public Button controlsText;
	public Button exitControlsText;
	public Canvas gameplayUI;
    public Button MainButton;

	bool usingController = false;
	player p;


	// Use this for initialization
	void Start () {


		p = GameObject.Find ("Player").GetComponent<player> ();
		PauseMenu = PauseMenu.GetComponent<Canvas> ();
		resumeText = resumeText.GetComponent<Button> ();
		exitText = exitText.GetComponent<Button> ();
		restartText = exitText.GetComponent<Button> ();
		controlsText= controlsText.GetComponent<Button> ();
		exitControlsText = exitControlsText.GetComponent<Button> ();
		PauseMenu.enabled = false;
		ControlsImage.enabled = false;
		exitControlsText.gameObject.SetActive (false);
		//exitControlsText.enabled = false;

        //detect keyboard
		foreach (string word in Input.GetJoystickNames()) {
			if (word != "") {
				usingController = true;
			} 
		}

        if(!usingController)
        {
            ControlsImage.sprite = Resources.Load<Sprite>("controlkey");
        }

	}

	public void PausePress()

	{
		showButtons ();
		PauseMenu.enabled = true;
		resumeText.enabled = true;
		controlsText.enabled = true;
		exitText.enabled = true;
		restartText.enabled = true;
		gameplayUI.enabled = false;
		Time.timeScale = 0.0f;
		if (usingController) {
			resumeText.Select ();
		}
        resumeText.Select();

        p.canFly = false;

	}

	public void ResumePress()

	{
		PauseMenu.enabled = false;
		hideButtons ();
		p.canFly = true;
        if (GameModeControl.mode == 2)
            Time.timeScale = 2.0f;
        else
            Time.timeScale = 1.0f;
		gameplayUI.enabled = true;
	}

	public void ReStartLevel()
	{
		Time.timeScale = 1.0f;
        SceneManager.LoadScene(1);
	}
	public void ExitLevel()
	{
		Application.Quit();

	}

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

	void hideButtons(){
		resumeText.gameObject.SetActive (false);
		exitText.gameObject.SetActive (false);
		restartText.gameObject.SetActive (false);
		controlsText.gameObject.SetActive (false);
        PauseMenu.GetComponent<Image>().enabled = false;
	}

	void showButtons(){
		resumeText.gameObject.SetActive (true);
		exitText.gameObject.SetActive (true);
		restartText.gameObject.SetActive (true);
		controlsText.gameObject.SetActive (true);
        PauseMenu.GetComponent<Image>().enabled = true;
    }

	public void ShowControl()
	{

		exitControlsText.gameObject.SetActive (true);
		ControlsImage.enabled = true;
		//exitControlsText.enabled = true;
		if (usingController) {
			exitControlsText.Select ();
		}
		hideButtons ();
	}
	public void HideControl()
	{
		//PauseMenu.enabled = false;
		exitControlsText.gameObject.SetActive (false);
		ControlsImage.enabled = false;
		//Time.timeScale = 1.0f;
		showButtons();
        controlsText.Select();
	}
	// Update is called once per frame



}
