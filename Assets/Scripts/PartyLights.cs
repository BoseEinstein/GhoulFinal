using UnityEngine;
using System.Collections;

public class PartyLights : MonoBehaviour {

    // Use this for initialization
    Light[] lightsList;
    bool partyTime;
    int i; // index to access lights
    Light currLight;
    float timer;
    bool cycle;
	void Start () {
        lightsList = gameObject.GetComponentsInChildren<Light>();
        
        foreach (Light l in lightsList)
            l.enabled = false;
        currLight = lightsList[0];
        cycle = true;
        partyTime = false;
        i = 0;
    }
	
	// Update is called once per frame
	void Update () {
        //this.gameObject.transform.RotateAround(transform.position, Vector3.up,  Time.deltaTime*10);
        if(partyTime && cycle)
        {
            timer += Time.deltaTime;
            if (timer > 0.5)
            {
                i = (i + 1) % lightsList.Length;
                currLight.enabled = true;
                currLight = lightsList[i];
                currLight.enabled = false;
                timer = 0;
            }

        }
    }

    public void turnOnLights()
    {
        foreach (Light l in lightsList)
            l.enabled = true;
        partyTime = true;
    }
}
