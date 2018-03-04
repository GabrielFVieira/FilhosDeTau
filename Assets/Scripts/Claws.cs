using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Claws : MonoBehaviour {
    private bool controle;
    private int dmg;
	// Use this for initialization
	void Start () {
        controle = true;
        dmg = 5;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && controle)
        {
            collision.GetComponent<PlayerHealth>().TakeDamage(dmg);
            controle = false;
        }

    }
}
