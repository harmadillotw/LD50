using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    public float maxCameraSize = 30f;
    public float zoomStep = 0.1f;
    public GameObject[] levels;
    public float choke;

    private Camera mainCamera;
    private Vector2 screenBounds;
    private Rigidbody2D playerRb;
    private float minCameraSize;
    
    

    private Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position - player.transform.position;
        minCameraSize = Camera.main.orthographicSize;
        playerRb = player.GetComponentInChildren<Rigidbody2D>();

        mainCamera = gameObject.GetComponent<Camera>();
        screenBounds = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCamera.transform.position.z));
        foreach (GameObject obj in levels)
        {
            loadChildObjects(obj);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        transform.position = player.transform.position + offset;
        //Debug.Log("camera size = " + Camera.main.orthographicSize);
       //Debug.Log("player Vel  = " + playerRb.velocity.magnitude);

        //float newCameraSize = minCameraSize + playerRb.velocity.magnitude - 10;
        float targetCameraSize = minCameraSize + Mathf.Max(0f,(playerRb.velocity.magnitude- minCameraSize));

        if (targetCameraSize < minCameraSize)
        {
            targetCameraSize = minCameraSize;

        }
        if (targetCameraSize > maxCameraSize)
        {
            targetCameraSize = maxCameraSize;
        }

        if (Camera.main.orthographicSize > targetCameraSize)
        {
            Camera.main.orthographicSize -= zoomStep;
        }
        if (Camera.main.orthographicSize < targetCameraSize)
        {
            Camera.main.orthographicSize += zoomStep;
        }

    }


    private void LateUpdate()
    {
        foreach (GameObject obj in levels)
        {
            repositionChildObjects(obj);
        }
    }

    private void loadChildObjects(GameObject obj)
    {
        Debug.Log(obj.name);
        float objectWidth = obj.GetComponentInChildren<SpriteRenderer>().bounds.size.x - choke;
        int childsNeeded = (int)Mathf.Ceil(screenBounds.x * 3 / objectWidth) + 2;
        GameObject clone = Instantiate(obj) as GameObject;
        for (int i = 0; i <= childsNeeded; i++)
        {
            GameObject c = Instantiate(clone) as GameObject;
            c.transform.SetParent(obj.transform);
            c.transform.position = new Vector3(objectWidth * i, obj.transform.position.y, obj.transform.position.z);
            c.name = obj.name + i;
        }
        Destroy(clone);
        Destroy(obj.GetComponent<SpriteRenderer>());

    }

    private void repositionChildObjects(GameObject obj)
    {

        Transform[] children = obj.GetComponentsInChildren<Transform>();
        if (children.Length > 1)
        {

            GameObject firstChild = children[1].gameObject;
            GameObject lastChild = children[children.Length - 1].gameObject;
            float halfObjectWidth = lastChild.GetComponent<SpriteRenderer>().bounds.extents.x - choke;
            if (transform.position.x + (screenBounds.x *2) > lastChild.transform.position.x + halfObjectWidth)
            {

                firstChild.transform.SetAsLastSibling();
                firstChild.transform.position = new Vector3(lastChild.transform.position.x + halfObjectWidth * 2, lastChild.transform.position.y, lastChild.transform.position.z);

            }
            else if (transform.position.x - (screenBounds.x *2) < firstChild.transform.position.x - halfObjectWidth)
            {

                lastChild.transform.SetAsFirstSibling();
                lastChild.transform.position = new Vector3(firstChild.transform.position.x - halfObjectWidth * 2, firstChild.transform.position.y, firstChild.transform.position.z);
            }
        }
    }
}
