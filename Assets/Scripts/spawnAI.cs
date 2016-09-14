using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class spawnAI : MonoBehaviour {
    public int identifier;
	GameObject kid;
    GameObject mom;
    GameObject dad;

    GameObject kid1;
    GameObject mom1;
    GameObject dad1;

    GameObject kid2;
    GameObject mom2;
    GameObject dad2;

    Vector3 spawn;

    
    Scare[] scareObjects;
    // Use this for initialization
    void Start () {
		
        mom = (GameObject)Resources.Load("Mom");
        dad = (GameObject)Resources.Load("Dad");

        
        mom1 = (GameObject)Resources.Load("Mom1");
        dad1 = (GameObject)Resources.Load("Dad1");

        
        mom2 = (GameObject)Resources.Load("Mom2");
        dad2 = (GameObject)Resources.Load("Dad2");

        if (GameModeControl.mode == 1)
        {
            kid = (GameObject)Resources.Load("Trick1");
            kid1 = (GameObject)Resources.Load("Trick2");
            kid2 = (GameObject)Resources.Load("Trick3");
        }
        else
        {
            kid = (GameObject)Resources.Load("kid");
            kid1 = (GameObject)Resources.Load("kid1");
            kid2 = (GameObject)Resources.Load("kid2");
        }




        spawn = GameObject.Find("AI_spawn_point"+identifier+"").GetComponent<Transform>().position;


    }
	
	// Update is called once per frame
	void Update () {


	}

    public void genAI()
    {
        int r = (int)Random.Range(0, 3);
        switch (r)
        {
            case 0:
                Instantiate(kid, spawn, Quaternion.identity);
                break;
            case 1:
                Instantiate(kid1, spawn, Quaternion.identity);
                break;
            case 2:
                Instantiate(kid2, spawn, Quaternion.identity);
                break;
            default:
                Instantiate(kid, spawn, Quaternion.identity);
                break;
        }
       

    }

    public void genMom()
    {
        int r = (int)Random.Range(0, 2);
        switch (r)
        {
            case 0:
                Instantiate(mom, spawn, Quaternion.identity);
                break;
            case 1:
                Instantiate(mom1, spawn, Quaternion.identity);
                break;
            case 2:
                Instantiate(mom2, spawn, Quaternion.identity);
                break;
            default:
                Instantiate(mom, spawn, Quaternion.identity);
                break;
        }

    }

    public void genDad()
    {
        int r = (int)Random.Range(0, 2);
        switch (r)
        {
            case 0:
                Instantiate(dad, spawn, Quaternion.identity);
                break;
            case 1:
                Instantiate(dad1, spawn, Quaternion.identity);
                break;
            case 2:
                Instantiate(dad2, spawn, Quaternion.identity);
                break;
            default:
                Instantiate(dad, spawn, Quaternion.identity);
                break;
        }

    }

}
