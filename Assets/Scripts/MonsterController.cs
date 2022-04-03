using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour
{

    public GameObject player;
    public AudioSource monsterAudioSource;
    public AudioClip monsterClip;

    private bool canSpawn = false;
    private float whenSpawn = 100f;
    private bool hasSpawned = false;
    private float speed = 5.0f;
    private float spawnTimer;

    private float soundTimer;
    // Start is called before the first frame update
    void Start()
    {
        spawnTimer = 0;
        soundTimer = 0;
        if (PlayerPrefs.HasKey("Difficulty"))
        {
            int difficulty = PlayerPrefs.GetInt("Difficulty");
            if (difficulty == 0)
            {
                canSpawn = false;
                hasSpawned = false;
                speed = 2.0f;
            }
            if (difficulty == 1)
            {
                canSpawn = true;
                whenSpawn = 100f;
                hasSpawned = false;
                speed = 5.0f;
            }
            if (difficulty == 2)
            {
                canSpawn = true;
                whenSpawn = 75f;
                hasSpawned = false;
                speed = 10.0f;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (hasSpawned)
        {
            float step = speed * Time.deltaTime;

            // move sprite towards the target location
            transform.position = Vector2.MoveTowards(transform.position, player.GetComponentInChildren<SpriteRenderer>().transform.position, step);
        }
        else
        {
            spawnTimer += Time.deltaTime;
            if (spawnTimer > whenSpawn)
            {
                SpawnMonster();
            }
        }
        //soundTimer += Time.deltaTime;
        //if (soundTimer > 3f)
        //{
        //    soundTimer = 0;
        //    //playAudio(monsterClip, monsterAudioSource, false);
        //}
    }

    private void SpawnMonster()
    {
        if (player.GetComponentInChildren<SpriteRenderer>().transform.position.y < 400f)
        {
            transform.position = new Vector3(player.GetComponentInChildren<SpriteRenderer>().transform.position.x + 20f, player.GetComponentInChildren<SpriteRenderer>().transform.position.y + 20f, 0f);
            hasSpawned = true;
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
