using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddArrow : MonoBehaviour {
    public PlayerMovement player;
    public int amount;
    // Use this for initialization
    void Start () {
		
	}

    public void Change()
    {
        player.ammo += amount;
        Destroy(gameObject);
    }
}
