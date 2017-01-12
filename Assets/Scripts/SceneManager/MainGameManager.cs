using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGameManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    [ContextMenu("VisualizeAMatch")]
    void VisualizeAMatch()
    {
        VisualizeMatch visualize = new VisualizeMatch(this);
        visualize.LoadMatchData();
        visualize.Visualize();
    }

    public void KingCastling(int turn)
    {
        BoardManager.Instance.KingCastling(turn);
    }

    public void QueenCastling(int turn)
    {
        BoardManager.Instance.QueenCastling(turn);
    }

    public Location Find(int turn, char c, Location dst, string disam)
    {
        return BoardManager.Instance.Find(turn, c, dst, disam);
    }

    public void Move(Location src, Location dst)
    {
        BoardManager.Instance.SelectChessman(src.x, src.y);
        BoardManager.Instance.MoveChessman(dst.x, dst.y);
    }
}
