using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Steamworks;

public class spawnGlobal : MonoBehaviour {

    // Use this for initialization
    int totalPatrons;
    int killedPatrons;
    int waveCount;
    public int totalWaves;
    int enemySpawnCount;
    int score;
    int momCount = 0;
    int dadCount = 0;
    int kidCount = 0;
    public float failTime;
    public float wavePrepTime;
    float waveTimer;
    float timer;
    float deathTimer;
    bool dead = false;
    bool timing;

    //spawn timers for the different spawn points
    float kitchenTimer;
    float mainTimer;
    float windowTimer;
    float bathroomTimer;

    //enemy at each point will spawn every set number of seconds
    public float kitchenRate;
    public float mainRate;
    public float windowRate;
    public float bathroomRate;
    //wave for each spawn to start making enemies
    public int kitchenStart;
    public int mainStart;
    public int windowStart;
    public int bathroomStart;

    //UI elements
    Text timerText;
    Text waveText;
    Text scoreText;
    Text kidText;
    Text momText;
    Text dadText;
    GameOverScreen GO;
    WinScreen winScreen;
    Image flameHealth;

    Image Ready;
    Image Wave;
    Image waveNumber;
    Image readyNumber;

    Sprite[] num;
    Sprite[] numDesat;

    //number of each enemy type to spawn on each wave
    int[] waveEnemyCountKid;
    int[] waveEnemyCountMom;
    int[] waveEnemyCountDad;

	public AudioClip collectfearsound;
	private AudioSource source; 

    spawnAI ms;
    spawnAI ks;
    spawnAI ws;
    spawnAI bs;

    //time in the future to start up the timer again after popwer up is used
    float stopTimeWindow;
    powerUp powers;



    //degree of speed up for fast mode
    public float speedMod;



    bool won;
    //used for the do nothing achievement
    public bool nothing;

    void Start () {
        timerText = GameObject.Find("WaveTimeUI").GetComponent<Text>();
        kidText = GameObject.Find("GirlCount").GetComponent<Text>();
        momText = GameObject.Find("MomCount").GetComponent<Text>();
        dadText = GameObject.Find("DadCount").GetComponent<Text>();
        GO = GameObject.Find("gameover").GetComponent<GameOverScreen>();
        winScreen = GameObject.Find("WinScreen").GetComponent<WinScreen>();
        flameHealth = GameObject.Find("SkullFlame").GetComponent<Image>();
        Ready = GameObject.Find("Ready").GetComponent<Image>();
        Wave = GameObject.Find("Wave").GetComponent<Image>();
        waveNumber = GameObject.Find("waveNumber").GetComponent<Image>();
        readyNumber = GameObject.Find("readyNumber").GetComponent<Image>();
        source = GetComponent<AudioSource>();
        ms = GameObject.Find("MainSpawn").GetComponentInChildren<spawnAI>();
        ks = GameObject.Find("KitchenSpawn").GetComponentInChildren<spawnAI>();
        ws = GameObject.Find("WindowSpawn").GetComponentInChildren<spawnAI>();
        bs = GameObject.Find("BathroomSpawn").GetComponentInChildren<spawnAI>();

        powers = GameObject.Find("powerObject").GetComponent<powerUp>();


        score = 0;
        waveCount = -1;
        totalWaves = 10;
        timing = true;
        won = false;

        timer = -wavePrepTime;
        waveTimer = timer;
        kitchenTimer = timer;
        mainTimer = timer;
        windowTimer = timer;
        bathroomTimer = timer;




        if (GameModeControl.mode == 2)
        {
            Time.timeScale = speedMod;
            
        }
        else
            Time.timeScale = 1;

        totalWaves = 10;

        populateEnemy(GameModeControl.mode);

        num = new Sprite[10];
        numDesat = new Sprite[10];

        //load in art assets for wave counter
        for(int i = 0; i < 10; i++)
        {
            num[i] = Resources.Load<Sprite>("WaveUI/WaveUI/" + (i+1));
            numDesat[i] = Resources.Load<Sprite>("WaveUI/WaveUI/" + (i+1) + "desat");

            
        }

        //changed spawning for trick or treat mode
       if(GameModeControl.mode == 1)
       {
           mainStart = 0;
           kitchenStart = 0;
           windowStart = 0;
           bathroomStart = 0;

           mainRate = 1.1f;
           kitchenRate = 1.2f;
           windowRate = 1.3f;
           bathroomRate = 1.4f;
       }

        nothing = true;
        spawnNextWave();

    }

    // Update is called once per frame
    void Update()
    {
        if(!won)
        { 
            if (!dead)
            {

                timer += Time.deltaTime;
                if (timer > stopTimeWindow && !timing)
                    startTimer();
                if (timing)
                {
                    waveTimer += Time.deltaTime;
                    kitchenTimer += Time.deltaTime;
                    mainTimer += Time.deltaTime;
                    windowTimer += Time.deltaTime;
                    bathroomTimer += Time.deltaTime;
                }
            }

            //prep timer
            if (waveTimer < 0)
            {
                timerText.text = "00";
                Sprite us = num[(int)(-waveTimer)];
                readyNumber.sprite = us;

            }
            else//timer on the wave
            {
                if (Wave.IsActive())
                {
                    Wave.CrossFadeAlpha(0f, 0.2f, true);
                    Ready.CrossFadeAlpha(0f, 0.2f, true);
                    readyNumber.CrossFadeAlpha(0f, 0.2f, true);
                    waveNumber.CrossFadeAlpha(0f, 0.2f, true);
                }


                int curr = (int)(failTime - waveTimer);
               
                if (curr > 9)
                {
                    
                    timerText.text = "" + curr;
                }
                else
                {
                   
                    if (curr > 0)
                        timerText.text = "0" + curr;
                    else
                        timerText.text = "00";
                }
                flameHealth.fillAmount = 1 - (waveTimer / failTime);

            }

            if (waveCount > totalWaves && !dead)//set win condition wave
            {
                dead = true;
                winFunction();
            }
            if (killedPatrons >= enemySpawnCount && waveTimer > 0 && !dead)
            {
                source.PlayOneShot(collectfearsound, .5f);
                spawnNextWave();
            }

            if (waveTimer >= failTime)
            {
                dead = true;
                if (deathTimer == 0)
                {
                    GameObject player = GameObject.Find("Player");
                    player.GetComponent<player>().killPlayer();
                    //re-enable player
                    SkinnedMeshRenderer[] skins = player.GetComponentsInChildren<SkinnedMeshRenderer>();//turn on mesh renderer
                    foreach (SkinnedMeshRenderer s in skins)
                    {
                        s.enabled = true;
                    }
                    //player.GetComponentInChildren<MeshRenderer>().enabled = true;
                    player.GetComponent<UnityStandardAssets.Characters.ThirdPerson.ThirdPersonUserControl>().enabled = true;//turn on control
                    player.GetComponent<Rigidbody>().isKinematic = false;//unfix
                    player.GetComponent<player>().control = true;
                    player.GetComponent<CapsuleCollider>().enabled = true;//turn on collider
                    player.GetComponent<posess>().one = false;//enable posession of another object

                }
                deathTimer += Time.deltaTime;
              
                if (deathTimer > 4 && dead)
                {
                    failFunction();
                    waveTimer = 0;

                }
            }
            checkSpawns();
        }

    }

    void checkSpawns()
    {
        if (totalPatrons < enemySpawnCount)
        {
            if (kitchenTimer >= kitchenRate )
            {
                if (waveCount >= kitchenStart)
                {
                    if (kidCount < waveEnemyCountKid[waveCount])
                    {
                        ks.genAI();
                        kidCount++;
                        kidText.text = "" + kidCount;
                        totalPatrons++;
                    }
                    kitchenTimer = 0;

                    checkMom(ks);
                    checkDad(ks);
                    
                }

            }

        }
        if (totalPatrons < enemySpawnCount)
        {
            if (mainTimer >= mainRate )
            {
                if (waveCount >= mainStart)
                {
                    if (kidCount < waveEnemyCountKid[waveCount])
                    {
                        ms.genAI();
                        kidCount++;
                        kidText.text = "" + kidCount;
                        totalPatrons++;
                    }
                    mainTimer = 0;

                    checkMom(ms);
                    checkDad(ms);
                }


            }
        }
        if (totalPatrons < enemySpawnCount)
        {
            if (windowTimer >= windowRate )
            {
                if (waveCount >= windowStart)
                {
                    if (kidCount < waveEnemyCountKid[waveCount])
                    {
                        ws.genAI();
                        kidCount++;
                        kidText.text = "" + kidCount;
                        totalPatrons++;
                    }
                    windowTimer = 0;
                    checkMom(ws);
                    checkDad(ws);
                }

            }
        }

        if (totalPatrons < enemySpawnCount)
        {
            if (bathroomTimer >= bathroomRate )
            {
                if (waveCount >= bathroomStart)
                {
                    if (kidCount < waveEnemyCountKid[waveCount])
                    {
                        bs.genAI();
                        kidCount++;
                        kidText.text = "" + kidCount;
                        totalPatrons++;
                    }
                    bathroomTimer = 0;
  
                    checkMom(bs);
                    checkDad(bs);
                }

            }
        }
    }

    void checkMom(spawnAI s)
    {
        
        if (waveEnemyCountMom[waveCount] > momCount)
        {
            s.genMom();
            momCount++;
            momText.text = "" + momCount;
            totalPatrons++;
        }
    }

    void checkDad(spawnAI s)
    {
        if ( waveEnemyCountDad[waveCount] > dadCount)
        {
            s.genDad();
            dadCount++;
            dadText.text = "" + dadCount;
            totalPatrons++;
        }
    }


    void setSteamAchievement(string achieveName)
    {
        try {
            SteamUserStats.SetAchievement(achieveName);
            SteamUserStats.StoreStats();
        }
        catch
        {

        }
    }

    //increments a given steam achievement counter
    void steamIncrement(string achieveName, string statName,int half,int max)
    {
        int steamVal;
        try
        {
            if (SteamUserStats.GetStat(statName, out steamVal))
            {
                steamVal++;
                SteamUserStats.SetStat(statName, steamVal);
                if (steamVal == 1 || steamVal == half)
                {

                    SteamUserStats.IndicateAchievementProgress(achieveName, (uint)steamVal, (uint)max);
                }
                if (steamVal == max)
                    setSteamAchievement(achieveName);
                
            }
        }
        catch
        {

        }
    }

    public void patronWasKilled(int type)
    {

        
        score++;
        killedPatrons++;


        steamIncrement("kills", "killCount", 606,1212);

        if (type == 0)
        {
            kidCount--;
            steamIncrement("babs", "babCount", 50,100);

                
            //flood mode power ups
            if(GameModeControl.mode == 1)
            {
                powers.incrementStopTime(1);

                powers.incrementRoomScare(1);

                powers.incrementCooldownReset(1);

            }
            //double speed power ups
            else if (GameModeControl.mode == 2)
            {
                powers.incrementCooldownReset(2);
            }
            //normal mode power up
            else
            {
                powers.incrementCooldownReset(1);
            }

            



        }
        else if (type == 1)
        {
            momCount--;
            steamIncrement("moms", "momCount", 50,100);

            //double speed mode
            if (GameModeControl.mode == 2)
            {
                powers.incrementRoomScare(2);

            }
            //normal mode
            else
            {
                powers.incrementRoomScare(1);
            }
            
        }
        else if (type == 2)
        {
            dadCount--;
            steamIncrement("dads", "dadCount", 50,100);

            //double speed mode
            if (GameModeControl.mode == 2)
            {
                powers.incrementStopTime(2);

            }
            //normal mode
            else
            {
                powers.incrementStopTime(1);
            }
            
        }
        kidText.text = "" + kidCount;
        momText.text = "" + momCount;
        dadText.text = "" + dadCount;
    }
    void populateEnemy(int mode)
    {
        waveEnemyCountKid = new int[totalWaves];
        waveEnemyCountMom = new int[totalWaves];
        waveEnemyCountDad = new int[totalWaves];
        //set spawn for little girl flood mode
        if (mode == 1)
        {
            waveEnemyCountKid[0] = 10;
            waveEnemyCountKid[1] = 30;
            waveEnemyCountKid[2] = 40;
            waveEnemyCountKid[3] = 45;
            waveEnemyCountKid[4] = 50;
            waveEnemyCountKid[5] = 55;
            waveEnemyCountKid[6] = 58;
            waveEnemyCountKid[7] = 60;
            waveEnemyCountKid[8] = 65;
            waveEnemyCountKid[9] = 70;


            kitchenStart = 0;
            windowStart = 0;
            bathroomStart = 0;
        }
        else if(mode==2)
        {
            //set number of kids on each wave
            waveEnemyCountKid[0] = 1;
            waveEnemyCountKid[1] = 3;
            waveEnemyCountKid[2] = 0;
            waveEnemyCountKid[3] = 4;
            waveEnemyCountKid[4] = 0;
            waveEnemyCountKid[5] = 6;
            waveEnemyCountKid[6] = 3;
            waveEnemyCountKid[7] = 4;
            waveEnemyCountKid[8] = 5;
            waveEnemyCountKid[9] = 4;

            //set number of moms on each wave
            waveEnemyCountMom[0] = 0;
            waveEnemyCountMom[1] = 0;
            waveEnemyCountMom[2] = 2;
            waveEnemyCountMom[3] = 1;
            waveEnemyCountMom[4] = 3;
            waveEnemyCountMom[5] = 1;
            waveEnemyCountMom[6] = 2;
            waveEnemyCountMom[7] = 2;
            waveEnemyCountMom[8] = 2;
            waveEnemyCountMom[9] = 3;

            //set dad count on each wave
            waveEnemyCountDad[0] = 0;
            waveEnemyCountDad[1] = 0;
            waveEnemyCountDad[2] = 1;
            waveEnemyCountDad[3] = 1;
            waveEnemyCountDad[4] = 2;
            waveEnemyCountDad[5] = 3;
            waveEnemyCountDad[6] = 2;
            waveEnemyCountDad[7] = 2;
            waveEnemyCountDad[8] = 2;
            waveEnemyCountDad[9] = 3;

        }
		else
        {
            //set number of kids on each wave
            waveEnemyCountKid[0] = 1;
            waveEnemyCountKid[1] = 3;
            waveEnemyCountKid[2] = 3;
            waveEnemyCountKid[3] = 1;
            waveEnemyCountKid[4] = 4;
            waveEnemyCountKid[5] = 5;
            waveEnemyCountKid[6] = 4;
            waveEnemyCountKid[7] = 6;
            waveEnemyCountKid[8] = 5;
            waveEnemyCountKid[9] = 5;

            //set number of moms on each wave
            waveEnemyCountMom[0] = 0;
            waveEnemyCountMom[1] = 0;
            waveEnemyCountMom[2] = 1;
            waveEnemyCountMom[3] = 2;
            waveEnemyCountMom[4] = 1;
            waveEnemyCountMom[5] = 2;
            waveEnemyCountMom[6] = 3;
            waveEnemyCountMom[7] = 2;
            waveEnemyCountMom[8] = 3;
            waveEnemyCountMom[9] = 4;

            //set dad count on each wave
            waveEnemyCountDad[0] = 0;
            waveEnemyCountDad[1] = 0;
            waveEnemyCountDad[2] = 1;
            waveEnemyCountDad[3] = 2;
            waveEnemyCountDad[4] = 2;
            waveEnemyCountDad[5] = 2;
            waveEnemyCountDad[6] = 2;
            waveEnemyCountDad[7] = 3;
            waveEnemyCountDad[8] = 3;
            waveEnemyCountDad[9] = 4;

        }

       
    }
    void failFunction()
    {
        //spawnNextWave();

        steamIncrement("gameOvers", "goCount", 6,13);
        if (nothing)
            setSteamAchievement("nothing");
        try
        {
            SteamUserStats.StoreStats();
        }
        catch
        {

        }
        GameObject.Find("UI").gameObject.SetActive(false);
        GO.Died();
    }

    public void winFunction()
    {
        won = true;
        if (GameModeControl.mode == 0)
            setSteamAchievement("win");
        if (GameModeControl.mode == 1)
            setSteamAchievement("trickwin");
        if (GameModeControl.mode == 2)
            setSteamAchievement("doublewin");
        try
        {
            SteamUserStats.StoreStats();
        }
        catch
        {

        }

        GameObject ui = GameObject.Find("UI");
        if(ui != null)
            ui.gameObject.SetActive(false);
        dead = false;
        winScreen.Won();

    }

    public void spawnNextWave()
    {
        
        waveCount++;
        if (waveCount > totalWaves)
            return;
        enemySpawnCount = waveEnemyCountKid[waveCount];
        enemySpawnCount += waveEnemyCountMom[waveCount];
        enemySpawnCount += waveEnemyCountDad[waveCount];
        killedPatrons = 0;
        totalPatrons = 0;
        momCount = 0;
        dadCount = 0;
        kidCount = 0;
        timer = -wavePrepTime;
        waveTimer = -wavePrepTime;
        kitchenTimer = timer;
        mainTimer = timer;
        windowTimer = timer;
        bathroomTimer = timer;
        flameHealth.fillAmount = 1;
        timing = true;
        Wave.CrossFadeAlpha(1f, 0.2f, true);
        Ready.CrossFadeAlpha(1f, 0.2f, true);
        readyNumber.CrossFadeAlpha(1f, 0.2f, true);
        waveNumber.CrossFadeAlpha(1f, 0.2f, true);

        waveNumber.sprite = numDesat[waveCount];
        readyNumber.sprite = num[5];
        try
        {
            SteamUserStats.StoreStats();
        }
        catch
        {

        }
        
    }

    public int getWaveCount()
    {
        return waveCount;
    }

    public int getTotalWaves()
    {
        return totalWaves;
    }

    public void stopTimer(float stopTimeLength)
    {
        timing = false;
        stopTimeWindow = timer + stopTimeLength;
        
    }
    public void startTimer()
    {
        timing = true;
    }


}
