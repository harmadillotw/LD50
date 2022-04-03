using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuelBubble : MonoBehaviour
{
    public bool visible = false;
    public float bubbleDuration = 5f;

    public AudioSource audioSource;
    public AudioClip popClip;

    private float bubbleTimer;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private bool finishedFadeIn = false;
    private bool startedFadeIn = false;
    private bool startedFadeOut = false;
    private bool finishedFadeOut = false;
    private float startTimer = 0f;
    // Start is called before the first frame update
    void Start()
    {
        bubbleTimer = 0;
        startTimer = 0;
        rb = GetComponentInChildren<Rigidbody2D>();
        sr = GetComponentInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        //if (!startedFadeIn)
        //{
        //    startTimer = bubbleTimer;
        //    startedFadeIn = true;
        //    StartCoroutine(FadeIn());
        //}
        //if ((finishedFadeIn) && (!startedFadeOut) && ((bubbleTimer - startTimer) > 4f))
        //{
        //    startedFadeOut = true;
        //    StartCoroutine(FadeOut());
        //}
        bubbleTimer += Time.deltaTime;

        if ((visible) && (finishedFadeIn) && (bubbleTimer > bubbleDuration))
        {
            resetFuelBubble(false);
        }
    }

    public void initFade()
    {
        bubbleTimer = 0f;
        finishedFadeIn = false;
        startedFadeOut = false;
        finishedFadeOut = false;
        startedFadeIn = true;
        StartCoroutine(FadeIn());
    }
    
    public void resetFuelBubble(bool playSound)
    {
        if (playSound)
        {
            playAudio(popClip, audioSource, false);
        }
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.gravityScale = 0f;
        rb.transform.position = new Vector3(0, -50, 0);

        visible = false;
        //initFade();
        Color TmpColour = sr.color;
        TmpColour.a = 1;
        sr.color = TmpColour;
        StateSettings.currentFuelBubbles--;
    }
    private IEnumerator FadeIn()
    {
        float alphaVal = sr.color.a;
        Color TmpColour = sr.color;

        while (sr.color.a < 1)
        {
            Debug.Log("D" + sr.color.a);
            alphaVal += 0.02f;
            TmpColour.a = alphaVal;
            sr.color = TmpColour;

            yield return new WaitForSeconds(0.05f);
        }
        finishedFadeIn = true;
    }

  

    private IEnumerator FadeOut()
    {
        float alphaVal = sr.color.a;
        Color TmpColour = sr.color;

        while (sr.color.a < 1)
        {
            Debug.Log("U" + sr.color.a);
            alphaVal += 0.02f;
            TmpColour.a = alphaVal;
            sr.color = TmpColour;

            yield return new WaitForSeconds(0.05f);
        }
        finishedFadeOut = true;
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
