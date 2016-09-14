using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Cam : MonoBehaviour {


	Vector3 Zoom1;
	Vector3 Zoom2;
	Vector3 Left;
	Vector3 Center;
	Vector3 Right;
    Vector3 UpperLeft;
    Vector3 UpperCenter;
    Vector3 UpperRight;

    GameObject player;
    bool canZoom;
    public bool limitCamera;
	Vector3[] zoomStates;
    Vector3[] rooms;
    public int zoomState;


    public float scrollSpeed;
    float zoomCool;
    float timer;
    float zoomWindow;
    int camLocation; //corresponding to the locationKey in the player script
    int playerLoc;
    bool movingCamera;
    float movePercentage;

	player ps;
    Image camx;

	// Use this for initialization
	void Start () {
		player = GameObject.Find ("Player");
        camx = GameObject.Find("CamEx").GetComponent<Image>();
        camx.enabled = false;
		zoomState = 0;
		ps = player.GetComponent<player>();

        scrollSpeed = 17;

        Left = new Vector3(3.78f, transform.position.y, 24.9f);
        Center = new Vector3(3.78f, transform.position.y, 17.71f);
        Right = new Vector3(3.78f, transform.position.y, 11.3f);
        UpperLeft = new Vector3(Left.x, 16.355f, Left.z);
        UpperCenter = new Vector3(Center.x, 16.355f, Center.z);
        UpperRight = new Vector3(Right.x, 16.355f, Right.z);
        Zoom1 = new Vector3 (3.78f, 12.82f, 24.9f);
		Zoom2 = new Vector3 (-5.28f, 16.13f, 18.14f);
		zoomStates = new Vector3[2];
		zoomStates [0] = Zoom1;
		zoomStates [1] = Zoom2;
        rooms = new Vector3[6];
        rooms[0] = Left;
        rooms[1] = Center;
        rooms[2] = Right;
        rooms[3] = UpperLeft;
        rooms[4] = UpperCenter;
        rooms[5] = UpperRight;
        canZoom = true; //bool for if player can zoom out the camera
        timer = 0;
        camLocation = 0;


	}

    //needed to separate from fixed update because changing states at low framerates
    //caused the camera to not actually zoom out
    void Update()
    {
        if ((Input.GetButtonDown("RightStick") || Input.GetMouseButtonDown(2)))
        {

            //player zooms out the camera
            if (canZoom)
            {
                zoomState = 1;

                zoomCool = timer + 20;
                zoomWindow = timer + 4;
                canZoom = false;
                camx.enabled = true;


            }
            else if (zoomState == 1)
                zoomState = 0;

            if (zoomState == 1)
                this.transform.position = Zoom2;
            if (zoomState == 0)
            {
                zoomIn();
            }
        }
    }


	// Update is called once per frame
	void FixedUpdate () {

        //enforce a time limit and cooldown on the zoomed out menu
        if(!canZoom)
        {
            timer += Time.deltaTime;

            if(timer > zoomWindow)
            {
                //if (zoomState == 1)
                zoomState = 0;
                zoomIn();
                zoomWindow = timer + 100;
            }

            if (timer > zoomCool)
            {
                //allow zoom again and update the HUD
                canZoom = true;
                camx.enabled = false;
                timer = 0;
            }
        }

        //new code to move camera consistently and smoothly
        //checks to see if player is in different room than camera
        playerLoc = player.GetComponent<player>().currentLocation();
        if (zoomState == 0 && camLocation != playerLoc)
        {
            
            camLocation = playerLoc;
            movingCamera = true;
            movePercentage = 0;
        }
        //moves camera to new room if required
        if(movingCamera && zoomState == 0)
        {
            movePercentage += 0.0035f;
            if (movePercentage > 1)
            {
                transform.position = Vector3.Lerp(transform.position, rooms[camLocation], 1);
                movingCamera = false;
                movePercentage = 0;
            }
            else
                transform.position = Vector3.Lerp(transform.position, rooms[camLocation], movePercentage);
        }

	}

    void zoomIn()
    {
        
        if (ps.bottomCenter)
        {
            this.transform.position = Center;
        }
        else if (ps.topCenter)
        {
            this.transform.position = UpperCenter;
        }
        else if (ps.topLeft)
        {
            this.transform.position = UpperLeft;
        }
        else if (ps.topRight)
        {
            this.transform.position = UpperRight;
        }
        else if (ps.bottomLeft)
        {
            this.transform.position = Left;
        }
        else if (ps.bottomRight)
        {
            this.transform.position = Right;
        }
    }

	






	
}
