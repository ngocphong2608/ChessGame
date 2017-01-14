using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGameManager : MonoBehaviour {
    VisualizeMatch visualize;
    float delay = 2;
    float oldTime = 0;
    public bool isVisualize = false;
    public GameObject playBtn;
   
	// Use this for initialization
	void Start () {
        MoveCameraAroundOnStart();
    }

    private void MoveCameraAroundOnStart()
    {

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
        playBtn.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
        if (isVisualize)
        {
            if (Time.time - oldTime > delay)
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
        BoardManager.Instance.SelectChessman(src.x, src.y);
        BoardManager.Instance.MoveChessman(dst.x, dst.y);
        Debug.Log("Move from " + src + " to " + dst);
    }
}
