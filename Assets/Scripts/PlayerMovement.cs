using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    private Animator anim;
    private int move;
    private float vel;
    private float x;
    private float y;
    private bool isWalking;
    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();
        vel = 3;

        //////// SET THE PLAYER FACING DOWN /////////////////
        isWalking = true;
        x = 0;
        y = -1;
        anim.SetBool("isWalking", isWalking);
        anim.SetFloat("x", x);
        anim.SetFloat("y", y);
    }
	
	// Update is called once per frame
	void Update () {
        //////////// READ THE INPUTS //////////////////
        x = Input.GetAxis("Horizontal");
        y = Input.GetAxis("Vertical");

        ////////// SET ANIMATIONS //////////////////////
        isWalking = (Mathf.Abs(x) + Mathf.Abs(y)) > 0;
        anim.SetBool("isWalking", isWalking);

        if(isWalking)
        {
            anim.SetFloat("x", x);
            anim.SetFloat("y", y);

            ///////////// MOVE THE PLAYER ///////////////////
            transform.Translate(x * vel *Time.deltaTime, y * vel * Time.deltaTime, 0);
        }

        
    }
}
