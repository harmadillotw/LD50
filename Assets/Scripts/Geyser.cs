using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Geyser : MonoBehaviour
{
    public GameObject gy1;
    private SpriteRenderer sr;
    private Collider2D m_Collider;

    private float gTimer;
    private bool geyEnabled = true;
    // Start is called before the first frame update
    void Start()
    {
        sr = gy1.GetComponent<SpriteRenderer>();
        m_Collider = GetComponentInChildren<Collider2D>();
        gTimer = 0f;
        geyEnabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        gTimer += Time.deltaTime;

        //if (gTimer > 15f)
        //{
        //    gTimer = 0f;
        //}
        if (gTimer > 5f)
        {
            flipGeyser();
        }

    }
    private void flipGeyser()
    { 
        if (geyEnabled)
        {
            Color tmp = sr.color;
            tmp.a = 0;
            sr.color = tmp;
            m_Collider.transform.position = new Vector3(m_Collider.transform.position.x, m_Collider.transform.position.y - 50, 0);
            geyEnabled = false;
        }
        else
        {
            Color tmp = sr.color;
            tmp.a = 1;
            sr.color = tmp;
            m_Collider.transform.position = new Vector3(m_Collider.transform.position.x, m_Collider.transform.position.y + 50, 0);
            geyEnabled = true;
        }
        gTimer = 0f;
    }
}
