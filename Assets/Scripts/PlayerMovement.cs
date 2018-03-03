using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    private Animator anim;
    private float vel;

    private bool isWalking;
    private float x;
    private float y;

    private bool run;

    private bool isAttacking;
    private float attackTimer;

    private bool isMagicActive;
    private float magicTimer;

    private bool isAiming;
    private float rangedTimer;
    // Use this for initialization
    void Start () {
        ////// GET THE ANIMATOR COMPONENT AND SET THE PLAYER VELOCITY ///////
        anim = GetComponent<Animator>();
        vel = 2.3f;

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

        if (Input.GetKeyDown(KeyCode.LeftShift) && isMagicActive == false && isAiming == false && isAttacking == false)
            run = true;

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            anim.speed = 1;
            run = false;
        }

        if (Input.GetKeyDown(KeyCode.C) && isMagicActive == false && isAiming == false)
            isAttacking = true;

        if (Input.GetKeyDown(KeyCode.X) && isAttacking == false && isAiming == false)
            isMagicActive = true;

        if (Input.GetKeyDown(KeyCode.Z) && isAttacking == false && isMagicActive == false)
            isAiming = true;

        ///////////////////////// SET ANIMATIONS //////////////////////
        isWalking = (Mathf.Abs(x) + Mathf.Abs(y)) > 0;
        anim.SetBool("isWalking", isWalking);
        
        ///////////////// CLOSE ATTACK ////////////////
        if (isAttacking)
        {
            anim.SetBool("isAttacking", isAttacking);

            attackTimer += Time.deltaTime;

            if (attackTimer >= 0.5f)  // THIS VALUE IS THE CLOSE ATTACK ANIMATION TIME
            {
                isAttacking = false;
                anim.SetBool("isAttacking", isAttacking);
                attackTimer = 0;
            }
         }

        ///////////////// MAGIC ////////////////////
        if (isMagicActive)
        {
            anim.SetBool("isMagicActive", isMagicActive);

            magicTimer += Time.deltaTime;

            if (magicTimer >= 0.583f) // THIS VALUE IS THE MAGIC ANIMATION TIME
            {
                isMagicActive = false;
                anim.SetBool("isMagicActive", isMagicActive);
                magicTimer = 0;
            }
        }

        ////////////////// LONG RANGE //////////////////
        if (isAiming)
        {
            anim.SetBool("isAiming", isAiming);

            rangedTimer += Time.deltaTime;

            if (rangedTimer >= 1.083f)  // THIS VALUE IS THE LONG RANGE ANIMATION TIME
            {
                isAiming = false;
                anim.SetBool("isAiming", isAiming);
                rangedTimer = 0;
            }
        }

        ///////////////// WALK ///////////////
        if (isWalking && isAttacking == false && isMagicActive == false && isAiming == false)
        {
            if (run)
            {
                ///////////// SPEED UP THE ANIMATION ///////////////
                anim.speed = 1.5f;

                anim.SetFloat("x", x);
                anim.SetFloat("y", y);

                ///////////// MOVE THE PLAYER (ACCELERATED) ///////////////////
                transform.Translate(x * vel * 1.7f *  Time.deltaTime, y * vel * 1.7f * Time.deltaTime, 0);
            }

            else
            {
                anim.speed = 1;

                anim.SetFloat("x", x);
                anim.SetFloat("y", y);

                ///////////// MOVE THE PLAYER ///////////////////
                transform.Translate(x * vel * Time.deltaTime, y * vel * Time.deltaTime, 0);
            }
        }

        
    }
}
