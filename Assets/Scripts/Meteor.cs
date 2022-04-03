using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : MonoBehaviour
{
    public bool visible = false;
    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        visible = false;
        rb = GetComponentInChildren<Rigidbody2D>();
        //rb.gravityScale = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if ((visible) && (rb.transform.position.y < -16f))
        {
            resetMeteor();
        }
    }
    public void resetMeteor()
    {
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.gravityScale = 0f;
        rb.transform.position = new Vector3(0, -50, 0);

        visible = false;
        StateSettings.currentMeteors--;
    }
    public void SpawnMeteor(float playerPosX)
    {
        Debug.Log("Spawn meteor");
        float vertRange = Random.Range(-2f, 2f);
        float horRange = Random.Range(-10f, 10f);

        //rock.transform.position = new Vector3(player.GetComponentInChildren<SpriteRenderer>().transform.position.x + horRange, -8f + vertRange, 0f);
        rb.transform.position = new Vector3(playerPosX + horRange, 10f + vertRange, 0f);
        GetComponentInChildren<Rigidbody2D>().gravityScale = 1f;
        float angle = Random.Range(45f, 135f);
        float thrust = 1000 + Random.Range(-250f, 250f);
        float xcomponent = Mathf.Cos(angle * Mathf.PI / 180) * thrust;
        float ycomponent = Mathf.Sin(angle * Mathf.PI / 180) * thrust;
        rb.AddForce(new Vector2(xcomponent, ycomponent));
        visible = true;
    }
}
