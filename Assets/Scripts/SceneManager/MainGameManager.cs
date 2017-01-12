using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGameManager : MonoBehaviour {
    VisualizeMatch visualize;
   
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    [ContextMenu("VisualizeAMatch")]
    public void VisualizeAMatch()
    {
        visualize = new VisualizeMatch(this);
        visualize.LoadMatchData();
        visualize.VisualizeNextStep();
    }

    public void StepByStep()
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
