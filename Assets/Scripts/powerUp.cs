using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class powerUp : MonoBehaviour {

    spawnGlobal globalTimer;

    Image cooldownResetUI;
    Image roomScareUI;
    Image stopTimeUI;

    //variables for powerup tracking
    int roomScareCurrent;
    public int roomScareMax;
    int stopTimeCurrent;
    public int stopTimeMax;
    int cooldownResetCurrent;
    public int cooldownResetMax;
    public float stopTimeLength;



    // Use this for initialization
    void Start () {
        globalTimer = GameObject.Find("MetaSpawn").GetComponent<spawnGlobal>();
        stopTimeUI = GameObject.Find("timePower").GetComponent<Image>();
        cooldownResetUI = GameObject.Find("speedPower").GetComponent<Image>();
        roomScareUI = GameObject.Find("scarePower").GetComponent<Image>();

        roomScareCurrent = 0;

        stopTimeCurrent = 0;

        cooldownResetCurrent = 0;

        cooldownResetUI.fillAmount = (float)cooldownResetCurrent / (float)cooldownResetMax;
        roomScareUI.fillAmount = (float)roomScareCurrent / (float)roomScareMax;
        stopTimeUI.fillAmount = (float)stopTimeCurrent / (float)stopTimeMax;
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public bool canFullRoomScare()
    {
        return roomScareCurrent >= roomScareMax;
    }
    public bool canStopTime()
    {
        return stopTimeCurrent >= stopTimeMax;
    }
    public bool canCooldownReset()
    {
        return cooldownResetCurrent >= cooldownResetMax;
    }

    //find all scare objects in the room ghoul kid is in and activate them
    public void fullRoomScare(string room)
    {
        GameObject[] roomScares = GameObject.FindGameObjectsWithTag(room);

        foreach (GameObject g in roomScares)
        {
            if (g.GetComponent<Scare>() != null)
            {
                //stupid fix for the lamp being different than all other objects
                if (g.transform.parent.name == "lamp")
                {
                    g.transform.parent.GetComponent<lamp>().stupidLamp();
                }
                Debug.Log(g.gameObject.transform.parent.gameObject.name);
                g.GetComponent<Scare>().startScare();
                g.GetComponent<Scare>().startAnimation();
            }

        }
        roomScareCurrent = 0;
        roomScareUI.fillAmount = (float)roomScareCurrent / (float)roomScareMax;
    }
    //increase the players speed.
    public void cooldownReset(string room)
    {
        GameObject[] roomScares = GameObject.FindGameObjectsWithTag(room);

        foreach (GameObject g in roomScares)
        {
            if (g.GetComponent<Scare>() != null)
                g.GetComponent<Scare>().resetCooldown();
        }
        cooldownResetCurrent = 0;
        cooldownResetUI.fillAmount = (float)cooldownResetCurrent / (float)cooldownResetMax;
    }

    public void stopTime()
    {
        globalTimer.stopTimer(stopTimeLength);
        stopTimeCurrent = 0;
        stopTimeUI.fillAmount = (float)stopTimeCurrent / (float)stopTimeMax;
    }


    public void refillPowers()
    {
        cooldownResetCurrent = cooldownResetMax;
        roomScareCurrent = roomScareMax;
        stopTimeCurrent = stopTimeMax;
        cooldownResetUI.fillAmount = (float)cooldownResetCurrent / (float)cooldownResetMax;
        roomScareUI.fillAmount = (float)roomScareCurrent / (float)roomScareMax;
        stopTimeUI.fillAmount = (float)stopTimeCurrent / (float)stopTimeMax;
    }

    public void incrementStopTime(int amount)
    {
        if (stopTimeCurrent < stopTimeMax)
        {
            stopTimeCurrent+=amount;
            stopTimeUI.fillAmount = (float)stopTimeCurrent / (float)stopTimeMax;
        }
    }

    public void incrementRoomScare(int amount)
    {
        if(roomScareCurrent < roomScareMax)
        {
            roomScareCurrent+=amount;
            roomScareUI.fillAmount = (float)roomScareCurrent / (float)roomScareMax;
        }
    }

    public void incrementCooldownReset(int amount)
    {
        if(cooldownResetCurrent < cooldownResetMax)
        {
            cooldownResetCurrent+=amount;
            cooldownResetUI.fillAmount = (float)cooldownResetCurrent / (float)cooldownResetMax;

        }
    }
}
