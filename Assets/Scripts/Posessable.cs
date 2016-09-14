using UnityEngine;
using System.Collections;

public class Posessable : MonoBehaviour {
	public bool posessed = false;
	public bool shouldScare = false;
    public bool lit = false;
    bool stayIn;
    Transform radiusTrans;

    GameObject player;
	// Use this for initialization
	void Start () {
        player = GameObject.Find("Player");
        stayIn = true;

    }
	
	// Update is called once per frame
	void Update () {



        //exit posession
		if ((Input.GetButtonDown("B")) && posessed && (!stayIn || !this.gameObject.GetComponent<Scare>().scaring)){

            deposess();
		
		}

	}

    public void deposess()
    {
        //re-enable player
        SkinnedMeshRenderer[] skins = player.GetComponentsInChildren<SkinnedMeshRenderer>();//turn on mesh renderer
        foreach (SkinnedMeshRenderer s in skins)
        {
            s.enabled = true;
        }
        player.GetComponent<UnityStandardAssets.Characters.ThirdPerson.ThirdPersonUserControl>().enabled = true;//turn on control
        player.GetComponent<Rigidbody>().isKinematic = false;//unfix
        player.GetComponent<player>().control = true;
        player.GetComponent<CapsuleCollider>().enabled = true;//turn on collider
        player.GetComponent<posess>().one = false;//enable posession of another object
                                                  //player.GetComponentInChildren<ParticleSystem>().Play();//turn flame head back on

        //disable object
        this.GetComponent<Collider>().isTrigger = true;//turn trigger back on 
        this.lit = false;//mark unlit
        posessed = false;

        //turn off and scale circle
        radiusTrans = this.gameObject.transform.parent.transform.parent.FindChild("Circle");



        radiusTrans.gameObject.GetComponent<MeshRenderer>().enabled = false;


    }


}
