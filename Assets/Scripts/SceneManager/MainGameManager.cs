using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGameManager : MonoBehaviour {
    VisualizeMatch visualize;

    float delay = 3f;
    float oldTime = 0;
    public bool isVisualize = false;
    public GameObject playBtn;
    public Animator animator;
    bool isFirst = true;
    float commandDelay = 1.0f;
    float commandDelayOld = 0f;

    public Animator checkedAnimator;

    // Use this for initialization
    void Start () {
    }

    public void PlayGame()
    {
        if (GameManager.Instance.GameMode == GameManager.MODE.VISUALIZE)
        {
            Debug.Log("Visulize Mode");
            VisualizeAMatch();
        }
        else if (GameManager.Instance.GameMode == GameManager.MODE.PLAYER_VS_PLAYER)
        {
            Debug.Log("Player Vs PLayer Mode");
        }
        //playBtn.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
        if (isFirst) //Xong animation thi thuc hien PlayGame() 
        {
            if (animator.IsInTransition(0))
            {
                PlayGame();
                isFirst = false;
            }
        }
        

        if (isVisualize)
        {
            if (commands.Count > 0)
            {
                if (Time.time - commandDelayOld > commandDelay)
                {
                    processCommand(commands.Dequeue());
                    commandDelayOld = Time.time;
                }
            } else if (Time.time - oldTime > delay)
            {
                oldTime = Time.time;
                StepNext();
            }
        }
	}

    [ContextMenu("VisualizeAMatch")]
    public void VisualizeAMatch()
    {
        visualize = new VisualizeMatch(this);
        visualize.LoadMatchData();
        isVisualize = true;
    }

    public void StopVisualize()
    {
        isVisualize = false;
    }

    public void StepNext()
    {
        visualize.VisualizeNextStep();
    }

    public void KingSideCastling(int turn)
    {
        BoardManager.Instance.KingSideCastling(turn);
    }

    public void QueenSideCastling(int turn)
    {
        BoardManager.Instance.QueenSideCastling(turn);
    }

    public Location Find(int turn, char c, Location dst, string disam)
    {
        Debug.Log("Find: " + c + " dst: " + dst + " dis: " + disam);
        return BoardManager.Instance.Find(turn, c, dst, disam);
    }

    public void Move(Location src, Location dst)
    {
        //BoardManager.Instance.SelectChessman(src.x, src.y);
        //BoardManager.Instance.MoveChessman(dst.x, dst.y);
        commands.Enqueue(new Command(0, src.x, src.y));
        commands.Enqueue(new Command(1, dst.x, dst.y));
        Debug.Log("Move from " + src + " to " + dst);
    }


    Queue<Command> commands = new Queue<Command>();
    struct Command
    {
        public int type;
        public int x, y;
        public Command(int type, int x, int y)
        {
            this.type = type;
            this.x = x;
            this.y = y;
        }
    }

    void processCommand(Command cmd)
    {
        if (cmd.type == 0)
        {
            BoardManager.Instance.SelectChessman(cmd.x, cmd.y);

        }
        else if (cmd.type == 1)
        {
            BoardManager.Instance.MoveChessman(cmd.x, cmd.y);
        }
    }

    public void OnChecked()
    {
        checkedAnimator.SetTrigger("PlayChecked");
    }
}
