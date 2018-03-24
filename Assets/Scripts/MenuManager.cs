using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour {
    private GameManager manager;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (manager == null)
            manager = GameObject.Find("MenuManager").GetComponent<GameManager>();
	}

    public void LoadSave()
    {
        manager.LoadGame();
    }

    public void NewSave()
    {
        manager.NewGame();
    }
}
