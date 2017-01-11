using UnityEngine;
using System.Collections;

public class CameraButtonManager : MonoBehaviour {

    Animator animator;

	// Use this for initialization
	void Start () {
        animator = GameObject.Find("Main Camera").GetComponent<Animator>();
        if (animator == null)
        {
            Debug.Log("Can't find camera animator.");
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    // side: left, right, white, black
    public void MoveCamera(string side)
    {
        if (side == "left")
        {
            animator.SetTrigger("LeftMotion");
        } else if (side == "right")
        {
            animator.SetTrigger("RightMotion");
        } else if (side == "white")
        {
            animator.SetTrigger("WhiteTurn");
        } else if (side == "black")
        {
            animator.SetTrigger("BlackTurn");
        }
    }
}
