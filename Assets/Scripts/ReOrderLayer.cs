using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReOrderLayer : MonoBehaviour {
    private GameObject player;
    private SpriteRenderer spriteRenderer;
    private Renderer[] renderers;
    public float offset;
	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        spriteRenderer = GetComponent<SpriteRenderer>();
        renderers = GetComponentsInChildren<Renderer>();
    }
	
	// Update is called once per frame
	void Update () {
        spriteRenderer.sortingOrder = Mathf.FloorToInt((transform.position.y - offset) * -1);
        /*
        if (this.gameObject.tag == "Enemy")
        {
            if (player.transform.position.y > transform.position.y)
            {
                spriteRenderer.sortingOrder = player.GetComponent<SpriteRenderer>().sortingOrder + 2;
            }

            else
            {
                spriteRenderer.sortingOrder = player.GetComponent<SpriteRenderer>().sortingOrder - 2;
            }

        }*/
    }
}
