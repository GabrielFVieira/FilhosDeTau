using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Area3 : MonoBehaviour {
    public ArrowTarget[] targets;
    public TutorialManager tutorial;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(targets[0].hit && targets[1].hit && targets[2].hit && targets[3].hit && targets[4].hit && targets[5].hit && targets[6].hit && !tutorial.partCompleted[3])
        {
            tutorial.partCompleted[3] = true;
            tutorial.StartTutorialDialogue(6);
            tutorial.gameObject.GetComponent<DialogueManager>().waitTime = 1.5f;
        }
    }
}
