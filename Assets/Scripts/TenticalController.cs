using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TenticalController : MonoBehaviour
{
    // Start is called before the first frame update
    public Animator Animator; // Assign in Inspector
    private const string ENABLE_ANIMATOR = "EnableAnimator";
    public void Start()
    {
        Invoke(ENABLE_ANIMATOR, (Random.value*2)); // you can multiply by however long your animation is, but if the user can immediately see some blobs, they may not be animated for up to 1 full animation cycle
    }

    private void EnableAnimator()
    {
        Animator.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
