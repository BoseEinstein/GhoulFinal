using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class player : MonoBehaviour {

	public float speed = 1.5f;
	public bool control = true;
    public bool canFly = false;
    public float flySpeed = 0.5f;
	Animator anim;
	public int score = 0;
	Text score_text;
    float flyCon;
    float idleTimer;
    bool isIdle;
    bool dead = false;
    int headHash;
    Light deathLight;
    string roomLocation;
    //reference for an array of camera positions in the camera script rooms correspond to 0,1,2 along bottom and 3,4,5 on top floor inorder
    int locationKey; 
    powerUp powers;



	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
        powers = GameObject.Find("powerObject").GetComponent<powerUp>();
        headHash = Animator.StringToHash("tossHead");
        deathLight = gameObject.GetComponentInChildren<Light>();
        deathLight.enabled = false;
		
    }
	
	// Update is called once per frame
	void FixedUpdate () {
		float vert = Input.GetAxis("Vertical");
		float hori = Input.GetAxis("Horizontal");
        float Dx = Input.GetAxis("DPadX");
        float Dy = Input.GetAxis("DPadY");

        flyCon = Input.GetAxis("Fly");

        idleTimer += Time.deltaTime;

        if(idleTimer > 5)
        {
            anim.SetTrigger(headHash);

        }

		//in and out
		if (control) {
			if (vert > 0 && !dead) {
				transform.position = new Vector3 (transform.position.x + Time.deltaTime * speed,
			                                 transform.position.y, 
			                                 transform.position.z);

                idleTimer = 0;
			} else if (vert < 0 && !dead) {
				transform.position = new Vector3 (transform.position.x - Time.deltaTime * speed,
			                                 transform.position.y, 
			                                 transform.position.z);

                idleTimer = 0;
            }

			//left and right
			if (hori > 0 && transform.position.z > 7.3f && !dead) {
				transform.position = new Vector3 (transform.position.x,
			                                 transform.position.y, 
			                                 transform.position.z - Time.deltaTime * speed);

                idleTimer = 0;
            } else if (hori < 0 && transform.position.z < 27.9f && !dead) {
				transform.position = new Vector3 (transform.position.x,
			                                 transform.position.y, 
			                                 transform.position.z + Time.deltaTime * speed);

                idleTimer = 0;
            }

            if(canFly)
            {
                //fly up
                if ((flyCon < 0 || Input.GetMouseButton(0)) && transform.position.y < 17f && !dead)
                {
                    if (Input.GetMouseButton(0))
                        transform.position = new Vector3(transform.position.x,
                            transform.position.y + (flySpeed),
                                                 transform.position.z);
                    else
                        transform.position = new Vector3(transform.position.x,
                        transform.position.y - (flyCon * flySpeed),
                                             transform.position.z);
                    idleTimer = 0;
                }
                //fly down
                if ((flyCon > 0 || Input.GetMouseButton(1)) && transform.position.y > 12.32f && !dead)
                {
                    if(Input.GetMouseButton(1))
                    transform.position = new Vector3(transform.position.x,
						transform.position.y - (flySpeed),
                                             transform.position.z);
                    else
                        transform.position = new Vector3(transform.position.x,
                        transform.position.y - (flyCon * flySpeed),
                                             transform.position.z);
                    idleTimer = 0;
                }


            }

            if((Dx > 0 || Input.GetButtonDown("KeyPower3")) && !dead)
            {

                if (powers.canFullRoomScare())
                    powers.fullRoomScare(roomLocation);
            }
            if ((Dx < 0  || Input.GetButtonDown("KeyPower1")) && !dead)
            {

                if (powers.canStopTime())
                    powers.stopTime();
            }
            if ((Dy < 0 || Input.GetButtonDown("KeyPower2")) && !dead)
            {

                if (powers.canCooldownReset())
                    powers.cooldownReset(roomLocation);
            }
            if ((Dy > 0 || Input.GetButtonDown("KeyPower4")) && !dead)
            {
                //refill power ups
                //Debug.Log("Press D up: " + Dy);
                powers.refillPowers();
                //spawn.winFunction();
            }

           


        }
	}



	public bool topLeft = false;
	public bool topRight = false;
	public bool bottomLeft = false;
	public bool bottomRight = false;
	public bool bottomCenter = false;
	public bool topCenter = false;

	void OnTriggerStay(Collider c){
		if (c.name == "BottomLeft") {
			bottomLeft = true;
			topLeft = false;
			topRight = false;
			bottomRight = false;
			bottomCenter = false;
			topCenter = false;
            roomLocation = c.name;
            locationKey = 0;

		} else if (c.name == "BottomRight") {
			bottomLeft = false;
			topLeft = false;
			topRight = false;
			bottomRight = true;
			bottomCenter = false;
			topCenter = false;
            roomLocation = c.name;
            locationKey = 2;
        } else if (c.name == "TopRight") {
			bottomLeft = false;
			topLeft = false;
			topRight = true;
			bottomRight = false;
			bottomCenter = false;
			topCenter = false;
            roomLocation = c.name;
            locationKey = 5;
        } else if (c.name == "TopLeft") {
			bottomLeft = false;
			topLeft = true;
			topRight = false;
			bottomRight = false;
			bottomCenter = false;
			topCenter = false;
            roomLocation = c.name;
            locationKey = 3;
        }
        else if(c.name == "BottomCenter"){
			bottomLeft = false;
			topLeft = false;
			topRight = false;
			bottomRight = false;
			bottomCenter = true;
			topCenter = false;
            roomLocation = c.name;
            locationKey = 1;
        }
        else if(c.name == "TopCenter"){
			bottomLeft = false;
			topLeft = false;
			topRight = false;
			bottomRight = false;
			bottomCenter = false;
			topCenter = true;
            roomLocation = c.name;
            locationKey = 4;
        }
	}


    //function to find out what room the palyer is in
    public int currentLocation()
    {
        return locationKey;
    }

    public void killPlayer()
    {
        dead = true;
        anim.SetBool("die", true);
        GameObject.Find("Lights").gameObject.SetActive(false);
        deathLight.enabled = true;
    }

    public bool isDead()
    {
        return dead;
    }


}
