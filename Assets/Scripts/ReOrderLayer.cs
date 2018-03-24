using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReOrderLayer : MonoBehaviour {
    private GameObject player;
    private SpriteRenderer spriteRenderer;
    private Renderer[] renderers;
	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
	
	// Update is called once per frame
	void Update () {
        renderers = GetComponentsInChildren<Renderer>();
        foreach (var r in renderers)
        {
            r.sortingOrder = spriteRenderer.sortingOrder + 1;
        }

        if (player.transform.position.y > transform.position.y)
        {
            foreach (var r in renderers)
            {
                r.sortingOrder = spriteRenderer.sortingOrder + 1;
            }
            
            spriteRenderer.sortingOrder = player.GetComponent<SpriteRenderer>().sortingOrder + 1;
        }

        else
        {
            spriteRenderer.sortingOrder = player.GetComponent<SpriteRenderer>().sortingOrder - 2;
        }
    }
}
