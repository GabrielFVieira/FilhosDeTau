using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAI : MonoBehaviour {

    public float time;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        time++;

        if (time > 100)
        {
            time = 0;

            int ataque = Random.Range(1, 4);

            if(ataque == 1)
            {
                Debug.Log("Ataque 1 ");
            }
            if (ataque == 2)
            {
                Debug.Log("Ataque 2 ");
            }
            if (ataque == 3)
            {
                Debug.Log("Ataque 3 ");
            }

        }

	}
}
