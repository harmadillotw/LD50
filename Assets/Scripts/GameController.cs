using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public GameObject player;
    public GameObject rockPrefab;
    public GameObject meteorPrefab;
    public GameObject fbPrefab;

    public Text Score;
    public Text Fuel;

    public int totalRocks = 5;
    public float maxRockInterval = 0.2f;
    public float minRockInterval = 4f;
    public float rockSpeed = 5f;

    public int totalMeteors = 5;
    public float maxMeteorsInterval = 0.2f;
    public float minMeteorsInterval = 4f;
    public float meteorsSpeed = 5f;

    public int totalFBs = 5;
    public float maxFBInterval = 0.2f;
    public float minFBInterval = 4f;
    public float fbSpeed = 3f;

    private float score;

    private float scoreMultiplier = 1f;

    private List<GameObject> rocks;

    private List<GameObject> meteors;

    private List<GameObject> fbs;

    private float RockCounter = 0f;
    private float MeteorCounter = 0f;
    private float FbCounter = 0f;

    private float endTimer = 3f;
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("Difficulty"))
        {
            int difficulty = PlayerPrefs.GetInt("Difficulty");
            if (difficulty == 0)
            {
                scoreMultiplier = 0.5f;
            }
            if (difficulty == 1)
            {
                scoreMultiplier = 1f;

            }
            if (difficulty == 2)
            {
                scoreMultiplier = 4f;
            }
        }
            //thrust = 100f;
        score = 0;
        StateSettings.end = false;
        StateSettings.currentRocks = 0;
        StateSettings.currentMeteors = 0;
        StateSettings.currentFuelBubbles = 0;

        // Create the rock gameobjects
        rocks = new List<GameObject>();

        for (int i=0; i< totalRocks; i++)
        {
            GameObject r =Instantiate(rockPrefab, new Vector3(0, -50, 0), Quaternion.identity);
            rocks.Add(r);
        }

        // Create the meteor gameobjects
        meteors = new List<GameObject>();

        for (int i = 0; i < totalMeteors; i++)
        {
            GameObject r = Instantiate(meteorPrefab, new Vector3(0,-50, 0), Quaternion.identity);
            meteors.Add(r);
        }

        //Create the fuel bubble gameobjects
        fbs = new List<GameObject>();

        for (int i = 0; i < totalFBs; i++)
        {
            GameObject r = Instantiate(fbPrefab, new Vector3(0, -50, 0), Quaternion.identity);
            fbs.Add(r);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Check if we need more rocks and spawn
        RockCounter += Time.deltaTime;
        if ((StateSettings.currentRocks < totalRocks) && (RockCounter > 2f))
        {
            spawnRock();
            RockCounter = 0f;
        }

        // Check if we need more meteors and spawn
        MeteorCounter += Time.deltaTime;
        if ((StateSettings.currentMeteors < totalMeteors) && (MeteorCounter > 2f))
        {
            spawnMeteor();
            MeteorCounter = 0f;
        }

        //Check if we need more fuel bubbles and spawn
        FbCounter += Time.deltaTime;
        if ((StateSettings.currentFuelBubbles < totalFBs) && (FbCounter > 2f))
        {
            spawnFuelBubble();
            FbCounter = 0f;
        }


        // Check if it is all over
        if (StateSettings.end)
        {
            endTimer -= Time.deltaTime;
        }
        if (endTimer < 0)
        {
            StateSettings.score = (int)score;
            SceneManager.LoadScene("EndScreen");
        }
        if ((!StateSettings.end) && (player.GetComponentInChildren<SpriteRenderer>().transform.position.y < 400f))
        {
            score += (Time.deltaTime * scoreMultiplier);
        }
    }

    private void LateUpdate()
    {
        Score.text = "Science:" + (int)score;
        Fuel.text = "Fuel:" + player.GetComponentInChildren<PlayerController>().fuel;
    }


        private void spawnMeteor()
    {
        foreach (GameObject meteor in meteors)
        {
            if (!meteor.GetComponent<Meteor>().visible)
            {
                //meteorPrefab.GetComponentInChildren<Meteor>().SpawnMeteor(player.GetComponentInChildren<SpriteRenderer>().transform.position.x);

                float vertRange = Random.Range(-2f, 2f);
                float horRange = Random.Range(-10f, 10f);
                Rigidbody2D meteorRb = meteor.GetComponentInChildren<Rigidbody2D>();
                //rock.transform.position = new Vector3(player.GetComponentInChildren<SpriteRenderer>().transform.position.x + horRange, -8f + vertRange, 0f);
                meteorRb.transform.position = new Vector3(player.GetComponentInChildren<SpriteRenderer>().transform.position.x + horRange, player.GetComponentInChildren<SpriteRenderer>().transform.position.y + 30f + vertRange, 0f);
                meteor.GetComponentInChildren<Rigidbody2D>().gravityScale = 1f;
                float angle = Random.Range(240f, 300f);
                float sRot = angle - 270;
                float thrust = 1000 + Random.Range(-250f, 250f);
                float xcomponent = Mathf.Cos(angle * Mathf.PI / 180) * thrust;
                float ycomponent = Mathf.Sin(angle * Mathf.PI / 180) * thrust;
                meteorRb.transform.rotation = Quaternion.Euler(transform.eulerAngles + new Vector3(0, 0, sRot));
                meteorRb.AddForce(new Vector2(xcomponent, ycomponent));
                meteor.GetComponent<Meteor>().visible = true;
                StateSettings.currentMeteors++;
                break;
            }
        }

    }
    private void spawnRock()
    {
        foreach( GameObject rock in rocks)
        {
            if (!rock.GetComponent<Rock>().visible)
            {
                //Debug.Log("Spawn Rock");
                float vertRange = Random.Range(-2f, 2f);
                float horRange = Random.Range(-10f, 10f);
                Rigidbody2D rockRb = rock.GetComponentInChildren<Rigidbody2D>();
                //rock.transform.position = new Vector3(player.GetComponentInChildren<SpriteRenderer>().transform.position.x + horRange, -8f + vertRange, 0f);
                rockRb.transform.position = new Vector3(player.GetComponentInChildren<SpriteRenderer>().transform.position.x + horRange, -8f + vertRange, 0f);
                rock.GetComponentInChildren<Rigidbody2D>().gravityScale = 1f;
                float angle = Random.Range(45f,135f);
                float thrust = 1000 + Random.Range(-250f,250f);
                float xcomponent = Mathf.Cos(angle * Mathf.PI / 180) * thrust;
                float ycomponent = Mathf.Sin(angle * Mathf.PI / 180) * thrust;
                rock.GetComponentInChildren<Rigidbody2D>().AddForce(new Vector2(xcomponent, ycomponent));              
                rock.GetComponent<Rock>().visible = true;
                StateSettings.currentRocks++;
                break;
            }
        }
            
    }

    private void spawnFuelBubble()
    {
        if ((player.GetComponentInChildren<SpriteRenderer>().transform.position.y > 50f) &&
            (player.GetComponentInChildren<SpriteRenderer>().transform.position.y < 250))
        {
            foreach (GameObject fb in fbs)
            {
                if (!fb.GetComponent<FuelBubble>().visible)
                {
                    //Debug.Log("Spawn Rock");
                    float vertRange = Random.Range(-20f, 20f);
                    float horRange = Random.Range(-20f, 20f);
                    Rigidbody2D fbRb = fb.GetComponentInChildren<Rigidbody2D>();
                    //rock.transform.position = new Vector3(player.GetComponentInChildren<SpriteRenderer>().transform.position.x + horRange, -8f + vertRange, 0f);
                    fbRb.transform.position = new Vector3(player.GetComponentInChildren<SpriteRenderer>().transform.position.x + horRange, 
                        player.GetComponentInChildren<SpriteRenderer>().transform.position.y + vertRange, 0f);
                    Color tc =fb.GetComponentInChildren<SpriteRenderer>().color;
                    tc.a = 0;
                    fb.GetComponentInChildren<SpriteRenderer>().color = tc;
                    fb.GetComponent<FuelBubble>().initFade();
                    
                    //fb.GetComponentInChildren<Rigidbody2D>().gravityScale = 1f;
                    float angle = Random.Range(0f, 360f);
                    float thrust = 100 + Random.Range(-250f, 250f);
                    float xcomponent = Mathf.Cos(angle * Mathf.PI / 180) * thrust;
                    float ycomponent = Mathf.Sin(angle * Mathf.PI / 180) * thrust;
                    fb.GetComponentInChildren<Rigidbody2D>().AddForce(new Vector2(xcomponent, ycomponent));
                    fb.GetComponent<FuelBubble>().visible = true;
                    StateSettings.currentRocks++;
                    break;
                }
            }
        }
    }

    
}
