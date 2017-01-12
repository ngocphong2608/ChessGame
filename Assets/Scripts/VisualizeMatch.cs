using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public class VisualizeMatch : MonoBehaviour {
    List<String> data;
    MainGameManager gameManager;

    public VisualizeMatch(MainGameManager gameManager)
    {
        this.gameManager = gameManager;
    }

    public void LoadMatchData()
    {
        data = new List<string>();
        string fileName = "Data/match1.txt";
        try
        {
            string line;
            StreamReader theReader = new StreamReader(fileName, Encoding.Default);
            using (theReader)
            {
                do
                {
                    line = theReader.ReadLine();

                    if (line != null)
                    {
                        //Debug.Log(line.Split('.')[1]);
                        data.Add(line.Split('.')[1]);
                    }
                } while (line != null); 

                theReader.Close();
            }
        }
        catch (Exception e)
        {
            Debug.Log("{0}\n" + e.Message);
        }
    }

    public void Visualize()
    {
        for (int i = 0; i < data.Count; i++)
        {
            String[] moves = data[i].Split(' ');
            Debug.Log("Turn " + (i+1) + ": " + moves[0] + " " + moves[1]);
            Move(0, moves[0]);
            Move(1, moves[1]);
        }
    }

    public String Normalize(String move)
    {
        return move.Replace("+", "").Replace("x", "");
    }

    public void Move(int turn, String move)
    {
        move = Normalize(move);
        char f = move[0];

        if (move == "O-O")
        {
            gameManager.KingSideCastling(turn);
        }
        else if (move == "O-O-O")
        {
            gameManager.QueenSideCastling(turn);
        }
        else
        {
            int n = move.Length;
            int rank = move[n - 1] - '1'; //hang
            int file = move[n - 2] - 'a'; //cot
            Location dest = new Location(rank, file);
            Location src;

            if ("KQBNR".Contains(f.ToString())) {
		        String disam = "";
		        if (n == 4)
			        disam += move[1];
		        src = gameManager.Find(turn, f, dest, disam);
	        } else { //Pawn
		        String disam = "";
		        if (n == 3)
			        disam += move[0];
		        src = gameManager.Find(turn, 'P', dest, disam);
	        }
            gameManager.Move(src, dest);
        }
    }
}
