using UnityEngine;
using System.Collections;

public class GhostDance : MonoBehaviour {

    // Use this for initialization
    SkinnedMeshRenderer sr;
    Animator anim;
    float timer;
    public float delay;
    bool party;
	void Start () {
        sr = this.GetComponentInChildren<SkinnedMeshRenderer>();
        sr.enabled = false;
        anim = this.GetComponent<Animator>();
        anim.enabled = false;
        party = false;
    }
	
	// Update is called once per frame
	void Update () {
        if (party)
        {
            timer += Time.deltaTime;
            if (timer > delay)
            {
                anim.enabled = true;
                sr.enabled = true;
            }
        }
	}

    public void ghosrDanceProtocoll()
    {
        
        party = true;
        
    }
}
