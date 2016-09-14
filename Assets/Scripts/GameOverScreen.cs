using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour {

    public Canvas GameOverCanvas;
    public Button exitText;
    public Button restartText;

    // Use this for initialization
    void Start()
    {

        GameOverCanvas = GameOverCanvas.GetComponent<Canvas>();

        exitText = exitText.GetComponent<Button>();
        restartText = restartText.GetComponent<Button>();

        GameOverCanvas.GetComponent<Image>().enabled = false;
        exitText.gameObject.SetActive(false);
        restartText.gameObject.SetActive(false);


    }
    public void Died()

    {
        //GameObject.Find("pause").gameObject.SetActive(false);
        GameOverCanvas.GetComponent<Image>().enabled = true;
        exitText.gameObject.SetActive(true);
        restartText.gameObject.SetActive(true);
        //GameObject.Find("Player").GetComponent<player>().canFly = false;
        Time.timeScale = 0.0f;
        restartText.Select();
    }

    public void ReStartLevel()
	{
        if(GameModeControl.mode == 2)
		    Time.timeScale = 2.0f;
        else
            Time.timeScale = 1.0f;
        SceneManager.LoadScene(1);
		//GameOverCanvas.enabled = false;
	}

	public void ExitLevel()
	{
        SceneManager.LoadScene(0);

    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
