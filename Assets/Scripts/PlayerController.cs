using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb;
    public float vSpeed = 80f;
    public float hSpeed = 80f;
    public float initalFuel = 5f;
    public float fuelDrop = 1f;
    public float maxFuel = 10f;

    public GameObject deadLander;

    public GameObject burnerSprite;
    public GameObject RJet;
    public GameObject LJet;

    public float fuel;

    public AudioSource burnerAudioSource;
    public AudioSource jetsAudioSource;
    public AudioSource refuelAudioSource;
    public AudioSource groundAudioSource;
    public AudioSource monsterAudioSource;
    public AudioSource triggerAudioSource;

    public AudioClip burnerClip;
    public AudioClip jetsClip;
    public AudioClip refuelClip;
    public AudioClip groundClip;
    public AudioClip monsterClip;

    public Text MaxFuelText;

    public AudioClip rockExplosion;
    public AudioClip meteorExplosion;

    private GameObject shield;

    private float monsterFuel = 3f;
    private float damageFuelDrop = 2f;
    private float bubbleFuel = 2f;
    private float geyserFuel = 1.5f;

    private bool startExplode;

    private bool geyFill = false;

    private bool shieldUp = false;
    private float shieldCounter = 0f;

    private bool moveUpRequest = false;
    private bool moveDownRequest = false;
    private bool moveRightRequest = false;
    private bool moveLeftRequest = false;


    // Start is called before the first frame update
    void Start()
    {
        startExplode = false;
        MaxFuelText.enabled = false;
        if (PlayerPrefs.HasKey("Difficulty"))
        {
            int difficulty = PlayerPrefs.GetInt("Difficulty");
            if (difficulty == 0)
            {
                initalFuel = 40f;
                maxFuel = 50f;
                bubbleFuel = 10f;
                geyserFuel = 5f;
                monsterFuel = 0f;
            }
            if (difficulty == 1)
            {
                initalFuel = 20f;
                maxFuel = 30f;
                bubbleFuel = 5f;
                geyserFuel = 3f;
                monsterFuel = 3f;
            }
            if (difficulty == 2)
            {
                initalFuel = 5f;
                maxFuel = 10f;
                bubbleFuel = 1.5f;
                geyserFuel = 1.25f;
                monsterFuel = 4f;
            }
        }

        rb = GetComponent<Rigidbody2D>();
        fuel = initalFuel;
        burnerSprite.SetActive(false);
        geyFill = false;
        shield = this.transform.Find("landershield").gameObject;
        shield.SetActive(false);
        shieldUp = false;
        shieldCounter = 0f;
    }

    // Update is called once per frame
    void Update()
    {

        if ((!StateSettings.end) && (fuel > 0))
        {
            float inputX = Input.GetAxis("Horizontal");
            float inputY = Input.GetAxis("Vertical");

            if (inputX != 0)
            {
                playAudio(jetsClip, jetsAudioSource, true);
                //fuel -= fuelDrop * Time.deltaTime;
                LowerFuel(geyserFuel * Time.deltaTime);
                if (inputX > 0)
                {
                    moveRightRequest = true;
                    LJet.SetActive(true);
                    RJet.SetActive(false);
                }
                else
                {
                    moveLeftRequest = true;
                    RJet.SetActive(true);
                    LJet.SetActive(false);
                }
            }
            else
            {
                RJet.SetActive(false);
                LJet.SetActive(false);
            }
            if (inputY > 0)
            {
                //fuel -= fuelDrop * Time.deltaTime;
                moveUpRequest = true;
                playAudio(burnerClip, burnerAudioSource, true);
                LowerFuel(fuelDrop * Time.deltaTime);
                burnerSprite.SetActive(true);
            }
            else
            {
                moveDownRequest = true;
                burnerSprite.SetActive(false);
            }
            //rb.AddForce(new Vector2(inputX * hSpeed, 0));
            //rb.AddForce(new Vector2(0, inputY * vSpeed));

        }
        else
        {
            burnerSprite.SetActive(false);
            RJet.SetActive(false);
            LJet.SetActive(false);
            if (rb.velocity.magnitude == 0)
            {
                StateSettings.end = true;
            }
        }

        if (shieldUp)
        {
            shieldCounter += Time.deltaTime;
            if (shieldCounter > 2f)
            {
                shieldCounter = 0f;
                shieldUp = false;
                shield.SetActive(false);

            }
        }

    }

    void FixedUpdate()
    {

        if (moveUpRequest)
        {
            moveUpRequest = false;
            rb.AddForce(new Vector2(0,  vSpeed));
        }
        if (moveDownRequest)
        {
            moveDownRequest = false;
            rb.AddForce(new Vector2(0, -1 * vSpeed));
        }
        if (moveRightRequest)
        {
            moveRightRequest = false;
            rb.AddForce(new Vector2(1 * hSpeed, 0));
        }
        if (moveLeftRequest)
        {
            moveLeftRequest = false;
            rb.AddForce(new Vector2(-1 * hSpeed, 0));
        }
    }

        void activateShield()
    {
        shieldCounter = 0f;
        shieldUp = true;
        shield.SetActive(true);
    }

    
    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            playAudio(groundClip, groundAudioSource, true);
            activateShield();
            //fuel -= fuelDrop * Time.deltaTime * 10;
            LowerFuel(fuelDrop * Time.deltaTime * 10);

        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        activateShield();
        if (collision.gameObject.tag == "Rock")
        {
            playAudio(rockExplosion, triggerAudioSource, false);

            // show forcefield

            if (fuel <= 0)
            {
                StateSettings.end = true;
            }
            collision.gameObject.GetComponentInParent<Rock>().resetRock();
            LowerFuel(damageFuelDrop);


        }
        if (collision.gameObject.tag == "Meteor")
        {
            playAudio(meteorExplosion, triggerAudioSource, false);
            if (fuel <= 0)
            {
                StateSettings.end = true;
            }
            collision.gameObject.GetComponentInParent<Meteor>().resetMeteor();
            LowerFuel(damageFuelDrop);
        }

        if (collision.gameObject.tag == "FB")
        {

            collision.gameObject.GetComponentInParent<FuelBubble>().resetFuelBubble(true);
            RaiseFuel(bubbleFuel);
        }


    }

    public void OnTriggerLeave2D(Collider2D collision)
    {

    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        activateShield();
        if (collision.gameObject.tag == "Geyser")
        {
            playAudio(refuelClip, refuelAudioSource, true);
            //geyFill = collision.GetComponent<Collider2D>().enabled;
            RaiseFuel(geyserFuel * Time.deltaTime);
        }

        if (collision.gameObject.tag == "Tenticles")
        {
            LowerFuel(monsterFuel * Time.deltaTime);
            playAudio(monsterClip, monsterAudioSource, true);
        }
    }



        private void LowerFuel(float value)
    {
        fuel -= value;
        if (fuel < 0f)
        {
            fuel = 0f;
        }
        ToggleMaxFuel(false);
    }

    private void RaiseFuel(float value)
    {
        fuel += value;
        if (fuel > maxFuel)
        {
            ToggleMaxFuel(true);
            fuel = maxFuel;
        }
    }

    private void ToggleMaxFuel(bool turnOn)
    {
        if (turnOn)
        {
            MaxFuelText.enabled = true;
        }
        else
        {
            MaxFuelText.enabled = false;
        }
    }
    private void playAudio(AudioClip clip, AudioSource audioSource, bool contPlay)
    {
        if ((contPlay) && (audioSource.isPlaying))
        {
            return;
        }
        int volumeSet = PlayerPrefs.GetInt("FXvolumeSet");
        float vol = 1f;
        if (volumeSet > 0)
        {
            int volume = PlayerPrefs.GetInt("FXVolume");
            vol = 1f;
            vol = (float)volume / 100f;
        }

        audioSource.PlayOneShot(clip, vol);
    }

}
