using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReOrderLayer : MonoBehaviour {
    private GameObject player;
    private SpriteRenderer spriteRenderer;
	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        spriteRenderer = GetComponent<SpriteRenderer>();

    }
	
	// Update is called once per frame
	void Update () {
		if(player.transform.position.y > transform.position.y)
            spriteRenderer.sortingOrder = player.GetComponent<SpriteRenderer>().sortingOrder + 1;

        else
            spriteRenderer.sortingOrder = player.GetComponent<SpriteRenderer>().sortingOrder - 1;
    }
}
