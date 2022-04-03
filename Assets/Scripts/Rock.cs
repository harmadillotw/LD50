using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    public bool visible = false;

   

    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponentInChildren<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if ((visible) && (rb.transform.position.y < -16f))
        {
            resetRock();
        }
    }

    public void resetRock()
    {
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.gravityScale = 0f;
        rb.transform.position = new Vector3(0, -50, 0);

        visible = false;
        StateSettings.currentRocks--;
    }
    

}
