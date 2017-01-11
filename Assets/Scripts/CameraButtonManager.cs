using UnityEngine;
using System.Collections;

public class CameraButtonManager : MonoBehaviour
{

    enum Position { White, Black, Left, Right }

    Animator animator;
    Position current = Position.White;


    // Use this for initialization
    void Start()
    {
        animator = GameObject.Find("Main Camera").GetComponent<Animator>();
        if (animator == null)
        {
            Debug.Log("Can't find camera animator.");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    // side: left, right, white, black
    public void MoveCamera(string side)
    {
        if (side == "left" && current != Position.Left)
        {
            animator.SetTrigger("LeftMotion");
            current = Position.Left;
        }
        else if (side == "right" && current != Position.Right)
        {
            animator.SetTrigger("RightMotion");
            current = Position.Right;
        }
        else if (side == "white" && current != Position.White)
        {
            animator.SetTrigger("WhiteTurn");
            current = Position.White;
        }
        else if (side == "black" && current != Position.Black)
        {
            animator.SetTrigger("BlackTurn");
            current = Position.Black;
        }
    }
}
