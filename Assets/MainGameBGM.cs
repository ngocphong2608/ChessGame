using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGameBGM : MonoBehaviour {
    public Animator animator;
    private bool isFirst = true;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (isFirst)
        {
            if (animator.IsInTransition(0))
            {
                GetComponent<AudioSource>().Play();
                isFirst = false;
            }
        }
    }
}
