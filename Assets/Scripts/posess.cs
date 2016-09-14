using UnityEngine;
using System.Collections;

public class posess : MonoBehaviour {

	//used to make sure only one object glows and that is the possesable object
	public bool one = false;
    player p;
	bool paused = false;

	// Use this for initialization
	void Start () {
        p = GameObject.Find("Player").GetComponent<player>();

	}

	// Update is called once per frame
	void Update () {
		if ((Input.GetKeyDown (KeyCode.Escape)|| Input.GetButtonDown("Start")) && !paused && Time.timeScale != 0) {
			GameObject.Find ("pause").GetComponent<PauseScreen> ().PausePress ();
			paused = true;
		} else if((Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("Start")) && paused) {
			GameObject.Find ("pause").GetComponent<PauseScreen> ().ResumePress ();
			paused = false;
		}
	}



	IEnumerator OnTriggerStay(Collider c){

		//Possesable interactions on trigger
		if (c.GetComponent<Posessable>() != null) { //make sure item has posseable component


			if (one == false || c.GetComponent<Posessable>().lit == true) { //make sure only possesing one object that is glowing

				
				one = true; 
				c.GetComponent<Posessable>().lit = true;//mark object as lit
				c.GetComponentInParent<shaderGlow>().lightOn();
				

				if ((Input.GetButtonDown("A")) && c.GetComponent<Posessable>() != null && !p.isDead()) { //detect posses button (Q)


					//start posession change to player
					SkinnedMeshRenderer[] skins = this.gameObject.GetComponentsInChildren<SkinnedMeshRenderer>();//turn off mesh renderer
					foreach(SkinnedMeshRenderer s in skins){
						s.enabled = false;
					}

					//bad variable use, could be cleaner
					Scare sc = c.GetComponent<Scare>();
					int sr = sc.scareRadius;

					Transform rt = c.gameObject.transform.parent.transform.parent.FindChild("Circle");

                    //controls size of scare circle
					switch (sr)
					{
					case 1:
						rt.localScale = new Vector3(32.90985f, 66.6f, 32.90985f);
						break;
					case 2:
						rt.localScale = new Vector3(74.35389f, 66.6f, 74.65389f);
						break;
					case 3:
						rt.localScale = new Vector3(103.4047f, 66.6f, 103.4047f);
						break;
					}

					rt.gameObject.GetComponent<MeshRenderer>().enabled = true;


					this.gameObject.GetComponent<UnityStandardAssets.Characters.ThirdPerson.ThirdPersonUserControl>().enabled = false;//turn off player control
					this.gameObject.GetComponent<Rigidbody>().isKinematic = true;//fix player position
					this.gameObject.GetComponent<player>().control = false;
					this.gameObject.GetComponent<CapsuleCollider>().enabled = false;//turn off players collider

					yield return new WaitForSeconds(.2f);
					c.GetComponent<Posessable>().posessed = true;
				}
			}
		}
	}


	//called when exiting a trigger
	void OnTriggerExit(Collider c) {

		//for exiting posessible triggers
		if (c.GetComponent<Posessable>() != null ) {//if object is posessable and has particle system
			one = false;//allow possesion of another object
			c.GetComponent<Posessable>().lit = false; //mark this object as unlit
			shaderGlow sg = c.GetComponentInParent<shaderGlow>();
			if (sg != null)
				sg.lightOff();
		}
	}
}
