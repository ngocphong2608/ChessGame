using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class VisualizeMatch : MonoBehaviour {
    List<String> data;

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
            Move(moves[0]);
        }
    }

    public void Move(String move)
    {

    }
}
