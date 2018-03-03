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
    private float arrowVel;
    [SerializeField]
    private GameObject arrowPrefab;

    private bool die;

    public bool colliding;
    private GameObject colGO;

    private Dictionary<string, int> dmg = new Dictionary<string, int>();

    public bool controle;
    // Use this for initialization
    void Start () {
        arrowVel = 15f;

        ////// GET THE ANIMATOR COMPONENT AND SET THE PLAYER VELOCITY ///////
        anim = GetComponent<Animator>();
        vel = 3f;

        //////// SET THE PLAYER FACING DOWN /////////////////
        isWalking = true;
        x = 0;
        y = -1;
        anim.SetBool("isWalking", isWalking);
        anim.SetFloat("x", x);
        anim.SetFloat("y", y);

        ////////////// SET THE ATTACKS DAMAGE /////////////////
        dmg.Add("Close",10); // Close Range Attack DMG
        dmg.Add("Range",25); // Ranged Attack DMG
        dmg.Add("Magic", 50); // Magic Attack DMG

        colGO = null; // Variable used to detect the colliding enemy
        controle = false;
    }
	
	// Update is called once per frame
	void Update () {
//        Debug.Log("y: " + anim.GetFloat("y") + " x: " + anim.GetFloat("x"));

        //////////// READ THE INPUTS //////////////////
        x = Input.GetAxis("Horizontal");
        y = Input.GetAxis("Vertical");

        if (Input.GetKeyDown(KeyCode.LeftShift) && isMagicActive == false && isAiming == false && isAttacking == false && die == false)
            run = true;

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            anim.speed = 1;
            run = false;
        }

        if (Input.GetKeyDown(KeyCode.E))
            die = true;

        if (Input.GetKeyDown(KeyCode.C) && isMagicActive == false && isAiming == false && die == false && attackTimer == 0)
        {
            isAttacking = true;
            controle = true;
        }

        if (Input.GetKeyDown(KeyCode.X) && isAttacking == false && isAiming == false && die == false && magicTimer == 0)
        {
            isMagicActive = true;
            controle = true;
        }

        if (Input.GetKeyDown(KeyCode.Z) && isAttacking == false && isMagicActive == false && die == false && rangedTimer == 0)
        {
            isAiming = true;
            controle = true;
        }

        ///////////////////////// SET ANIMATIONS //////////////////////
        isWalking = (Mathf.Abs(x) + Mathf.Abs(y)) > 0;
        anim.SetBool("isWalking", isWalking);
        anim.SetBool("Died", die);

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
                Quaternion rot = Quaternion.Euler(0, 0, 0);

                if (anim.GetFloat("y") > 0 && anim.GetFloat("x") == 0)
                    rot = Quaternion.Euler(0, 0, 90);

                else if (anim.GetFloat("y") < 0 && anim.GetFloat("x") == 0)
                    rot = Quaternion.Euler(0, 0, -90);

                else if (anim.GetFloat("x") > 0 && anim.GetFloat("y") == 0)
                    rot = Quaternion.Euler(0, 0, 0);

                else if (anim.GetFloat("x") < 0 && anim.GetFloat("y") == 0)
                    rot = Quaternion.Euler(0, 0, 180);

                GameObject arrow = (GameObject)Instantiate(arrowPrefab, transform.position, rot);

                arrow.GetComponent<Arrow>().arrowVel = arrowVel;
                arrow.GetComponent<Arrow>().dmg = dmg["Range"];

                isAiming = false;
                anim.SetBool("isAiming", isAiming);
                rangedTimer = 0;
            }
        }

        ///////////////// WALK ///////////////
        if (isWalking && isAttacking == false && isMagicActive == false && isAiming == false && die == false)
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

        if (die)
        {
            anim.speed = 1;
        }

        //////////// ATTACK DETECTION WHILE COLLIDING //////////////////
        if (colGO != null)
        {
            if (colGO.transform.position.y > transform.position.y && anim.GetFloat("y") > 0 && anim.GetFloat("x") == 0 || colGO.transform.position.y < transform.position.y && anim.GetFloat("y") < 0 && anim.GetFloat("x") == 0 || colGO.transform.position.x > transform.position.x && anim.GetFloat("x") > 0 && anim.GetFloat("y") == 0 || colGO.transform.position.x < transform.position.x && anim.GetFloat("x") < 0 && anim.GetFloat("y") == 0)
            {
                if (colGO.gameObject.tag == "Enemy")
                {
                    if (isAttacking && controle && attackTimer >= 0.4f)
                    {
                        colGO.GetComponent<EnemyHealth>().TakeDamage(dmg["Close"]);
                        controle = false;
                    }
/*

                    else if (isMagicActive && controle)
                    {
                        colGO.GetComponent<EnemyHealth>().TakeDamage(dmg["Magic"]);
                        controle = false;
                    }*/
                }
            }
        }
    }

    //////////// DETECT COLLISIONS /////////////////
    private void OnTriggerStay2D(Collider2D col)
    {
        colliding = true;
        colGO = col.gameObject;
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        colliding = false;
        colGO = null;
    }
}
