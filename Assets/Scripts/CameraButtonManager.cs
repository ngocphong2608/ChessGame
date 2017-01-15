using UnityEngine;
using System.Collections;

public class CameraButtonManager : MonoBehaviour
{

    Animator animator;

    int index = 0;
    private string[] cameraTrigger = { "WhiteTurn", "RightMotion", "BlackTurn", "LeftMotion"};

    // Use this for initialization
    void Start()
    {
        animator = GameObject.Find("Main Camera").GetComponent<Animator>();
        if (animator == null)
        {
            Debug.Log("Can't find camera animator.");
        }
    }

    // side: left, right, white, black
    public void MoveCamera(string side)
    {
        if (side == "left")
        {
            index--;
            if (index < 0)
                index = 3;
            
        } else
        {
            index++;
            if (index > 3)
                index = 0;
        }
        animator.SetTrigger(cameraTrigger[index]);
    }
}
