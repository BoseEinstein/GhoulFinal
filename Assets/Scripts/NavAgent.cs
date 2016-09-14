using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[System.Serializable]
public class AIPath
{
    public Vector3[] path;
    public int v;

    public AIPath()
    {
        path = new Vector3[10];
        v = 0;
    }

    public AIPath(int pathSize)
    {
        path = new Vector3[pathSize];
        v = pathSize;
    }

    public void setPoint(int i,Vector3 point)
    {
        path[i] = point;
    }

    public Vector3 getPoint(int i)
    {
        return path[i%v];
    }
}


public class NavAgent : MonoBehaviour {

    
    public float RotationSpeed;
    public float walkDelay;

	
	Vector3 target;
    Vector3 runTarget;
	int fearLevel;
    bool exiting = false;
	bool first = true;
    bool idle;
    int state;
    public AIPath pathPoints;
    public int pointCount;
    NavMeshAgent agent;
    Vector3 exit;
    Quaternion _lookRotation;
    Vector3 _direction;
    Vector3 viewTarget;
    float idleTimer;
    float walkTimer;
    float totalTimer;
    float scaredTimer;
    float deathTimer;
    int health;
    public int maxHealth;
    public Image healthBar;
	bool active; /////////////////NEW 
    bool scaredNow;
    bool isDead;
    bool dying;

	Animator anim;
    spawnGlobal sg;
    public int type;

    string currentScare;

    public float walkSpeed;
    public float runSpeed;
    bool running;


    // Use this for initialization
    void Start() {

 
        agent = GetComponent<NavMeshAgent>();
		anim = GetComponent<Animator> ();
        Image[] bars = GetComponentsInChildren<Image>();
        foreach (Image i in bars)
        {
            if (i.gameObject.name == "Health")
            {
                healthBar = i;
            }
        }
        sg = GameObject.Find("MetaSpawn").GetComponent<spawnGlobal>();
        idle = true;
        state = 0;
        active = true;
        health = maxHealth;
        isDead = false;
        scaredNow = false;
        dying = false;
		anim.SetBool ("Walk", true);
        currentScare = "none";
        deathTimer = 0;
        running = false;
        
        agent.avoidancePriority = Random.Range(1, 100);

        setSpawnTag();
        
        
    }

    // Update is called once per frame
    void Update() {

        if (idle )
        {			
            idleWalk();
        }

        
		if(active){///////////////NEW
              agent.SetDestination(target);
		}

        if(scaredNow)
        {
            changeView();
            scaredTimer += Time.deltaTime;
            AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
            //persist scare animation to completion or hard cap it at 4 seconds
            if (scaredTimer > stateInfo.length || scaredTimer > 4)
            {
                
                
				anim.SetBool ("Scared", false);
				anim.SetBool ("Walk", false);
                anim.SetBool("Idle", false);
                anim.SetBool("Run", true);
                running = true;
                //set nav agent speed
                this.gameObject.GetComponent<NavMeshAgent>().speed = runSpeed;
                scaredNow = false;
                scaredTimer = 0;
                agent.SetDestination(target);
                changeView();

            }
        }

        if (running && !scaredNow)
        {
			target = runTarget;
            agent.SetDestination(target);
            checkProximity();
            if (idle)
            {
                running = false;
                this.gameObject.GetComponent<NavMeshAgent>().speed = walkSpeed;
                anim.SetBool("Run", false);
                anim.SetBool("Walk", true);
                anim.SetBool("Idle", true);
                idle = true;
                active = true;
            }
        }


        if (health < 1)
        {
            //DO DEATH STUFF HERE

			active = false;///////////////NEW
            idle = false;
            scaredNow = false;
            setTarget(getCenter());
            agent.SetDestination(target);

            
            anim.SetBool("Death", true);
            isDead = true;
            
        }

        if(isDead)
        {
            if(!dying)
            {
                sg.patronWasKilled(type);
                dying = true;
            }
            deathTimer += Time.deltaTime;
            AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
            if (deathTimer > stateInfo.length)
            {
                
                Destroy(gameObject);
            }
        }
       


    }

    void idleWalk() {
		

        totalTimer += Time.deltaTime;
        active = true;

        checkProximity();

		if (scaredNow||idleTimer > 5 || first || walkTimer > 25) {
			
			first = false;
            scaredNow = false;
			float fstate = Random.Range (0, pointCount);

            state = (int)fstate;

            
            setTarget(pathPoints.getPoint(state));
            setView(pathPoints.getPoint(state));

            anim.SetBool("Walk", true);
            anim.SetBool("Scared", false);
            anim.SetBool("Idle", false);

            idleTimer = 0;
            walkTimer = 0;
        }
    }

    void checkProximity()
    {
        if (Mathf.Abs(transform.position.x - target.x) < .15f && Mathf.Abs(transform.position.z - target.z) < .15f)
        {
            anim.SetBool("Walk", false);
            anim.SetBool("Idle", true);
            anim.SetBool("Run", false);
            idle = true;
            idleTimer += Time.deltaTime;
        }
        else
        {
            anim.SetBool("Walk", true);
            anim.SetBool("Scared", false);
            anim.SetBool("Idle", false);
            walkTimer += Time.deltaTime;
        }
    }
    void changeView() {
		
        _direction = (viewTarget - transform.position).normalized;
        _lookRotation = Quaternion.LookRotation(_direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime * RotationSpeed);
    }

    public void setView(Vector3 newView) {
        viewTarget = newView;
    }


    public void setTarget(Vector3 newTarget) {
		
        target = newTarget;
    }

    public void scared(int scareVal, string scareObject,Vector3 newRun) {


        health -= scareVal;
        if (healthBar != null)
        {
            healthBar.fillAmount = (float)health / (float)maxHealth;
            scaredNow = true;
            setCurrentScare(scareObject);
            runTarget = newRun;
            anim.SetBool("Scared", true);
            anim.SetBool("Walk", false);
            anim.SetBool("Idle", false);

            idle = false;
            active = true;

            changeView();
        }

    }

    public bool isExiting() {
        return exiting;
    }

    public void setSpawnTag()
    {
        GameObject[] tempPoints;
        pointCount = 0;

        pathPoints = new AIPath();
        
        string tag;

 
        tag = "0AI_Path1";
        tempPoints = GameObject.FindGameObjectsWithTag(tag);
            

        pointCount = tempPoints.Length;
        pathPoints = new AIPath(pointCount);


        for (int i = 0; i < pointCount; i++)
        {
            pathPoints.setPoint(i, tempPoints[i].GetComponent<Transform>().position);

        }
   
    }

    public string getCurrentScare()
    {
        return currentScare;
    }

    public void setCurrentScare(string newScare)
    {
        currentScare = newScare;
    }

    public Vector3 getCenter()
    {
        return transform.position;
    }

     
}
