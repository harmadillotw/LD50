using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashConroller : MonoBehaviour
{
    private float splashTimer;
    // Start is called before the first frame update
    void Start()
    {
        splashTimer = 0;
        if (!PlayerPrefs.HasKey("Difficulty"))
        {
            PlayerPrefs.SetInt("Difficulty", 1);
        }
    }

    // Update is called once per frame
    void Update()
    {
        splashTimer += Time.deltaTime;
        if ((Input.anyKey) || (splashTimer > 3f))
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}
