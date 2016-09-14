using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Steamworks;

public class WinScreen : MonoBehaviour {

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
    public void Won()

    {
        GameOverCanvas.GetComponent<Image>().enabled = true;
        exitText.gameObject.SetActive(true);
        restartText.gameObject.SetActive(true);
        Time.timeScale = 0.0f;
        restartText.Select();
    }

    public void StartDance()
	{

        Time.timeScale = 1.0f;
        GameObject[] g;//ghosts
        GameObject[] pl;//party lights
        GameObject[] l;//normal lights

        g = GameObject.FindGameObjectsWithTag("Ghosts");
        pl = GameObject.FindGameObjectsWithTag("PartyLights");
        l = GameObject.FindGameObjectsWithTag("Lights");

        GameObject.Find("PartyMusic").GetComponent<AudioSource>().Play();
        GameObject.Find("Background Music").GetComponent<AudioSource>().Stop();
        GameObject.Find("DanceWin").SetActive(false);
        GameObject.Find("ExitWin").SetActive(false);

        foreach (GameObject go in g)
        {
            if(go.GetComponent<GhostDance>() != null)
                go.GetComponent<GhostDance>().ghosrDanceProtocoll();
        }

        foreach (GameObject go in pl)
        {
            go.GetComponent<PartyLights>().turnOnLights();
        }

        foreach (GameObject go in l)
        {
            if(!go.name.Equals("TopLight"))
                go.GetComponent<Light>().enabled = false;
        }

        GameObject.Find("Player").GetComponent<Animator>().SetBool("party", true);

        GameOverCanvas.enabled = false;

        try
        {
            SteamUserStats.SetAchievement("party");
            SteamUserStats.StoreStats();
        }
        catch
        {

        }

    }

	public void ExitLevel()
	{
        SceneManager.LoadScene(0);
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
