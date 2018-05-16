using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhythmBar : MonoBehaviour {
    public Transform cursor;
    public float intensity;
    public float cursorSpeed;
    private float x;
    public float[] dmgMultipliers;
    public bool freeze;
    private float timer;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        x = cursor.transform.localPosition.x;
        GetComponent<Animator>().SetBool("Broken", freeze);

        if (!freeze)
        {
            MoveCursor();
            SetIntensity();
        }

        else
        {
            timer += Time.deltaTime;

            if(timer > 0.25f)
            {
                timer = 0;
                freeze = false;
            }
        }
    }

    void MoveCursor()
    {
        if (x < 1 && cursorSpeed > 0 || x > -1 && cursorSpeed < 0)
            cursor.transform.Translate(cursorSpeed * Time.deltaTime, 0, 0);

        if (x >= 1 && cursorSpeed > 0 || x <= -1 && cursorSpeed < 0)
            cursorSpeed *= -1;
    }

    void SetIntensity()
    {
        if (x <= -0.8f || x >= 0.8f)
        {
            intensity = dmgMultipliers[0];
        }

        else if (x > -0.8f && x <= -0.4f || x < 0.8f && x >= 0.4f)
        {
            intensity = dmgMultipliers[1];
        }

        else if (x > -0.4f && x <= -0.15f || x < 0.4f && x >= 0.15f)
        {
            intensity = dmgMultipliers[2];
        }

        else
            intensity = dmgMultipliers[3];
    }
}
