using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Area1 : MonoBehaviour {
    public RagDool firstRagdoll;
    public TutorialManager tutorial;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        if (firstRagdoll.attack && !tutorial.partCompleted[1])
        {
            tutorial.partCompleted[1] = true;
            tutorial.StartTutorialDialogue(1);
        }
	}
}
