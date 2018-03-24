using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Animator anim;
    private float vel;

    private bool isWalking;
    private float x;
    private float y;

    private bool run;
    private float runVel;
    private float energy;

    private bool isAttacking;
    private float attackTimer;
    [SerializeField]
    private AnimationClip closeAttack;

    private bool isMagicActive;
    private float magicTimer;
    [SerializeField]
    private AnimationClip magicAttack;

    private bool isAiming;
    private float rangedTimer;
    [SerializeField]
    private AnimationClip rangedAttack;
    private float arrowVel;
    [SerializeField]
    private GameObject arrowPrefab;
    public int ammo;
    public int maxAmmo = 10;

    private bool die;

    public GameObject colGO;

    private Dictionary<string, int> dmg = new Dictionary<string, int>();

    private float slowTimer;
    private float maxSlowTime;
    private float initialVel;
    private bool controlSlow;
    public bool controle;
    [SerializeField]
    private GameObject clawHUD;

    private bool roll;
    private float rollEnergyConsum;
    [SerializeField]
    private AnimationClip rollClip;
    private float rollTimer;
    private Vector2 curAxis = new Vector2();

    private GameManager manager;
    // Use this for initialization
    void Start()
    {
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();

        /////////// SET VARIABLES OF SOME ACTIONS ///////////
        ammo = maxAmmo;
        clawHUD.SetActive(false);
        rollEnergyConsum = 10;
        arrowVel = 15f;
        runVel = 1.8f;

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
        dmg.Add("Close", 15); // Close Range Attack DMG
        dmg.Add("Range", 25); // Ranged Attack DMG
        dmg.Add("Magic", 50); // Magic Attack DMG

        //colGO = null; // Variable used to detect the colliding enemy
        controle = false;
    }

    // Update is called once per frame
    void Update()
    {
        energy = GetComponent<EnergyBar>().curEnergy;

        //////////// READ THE INPUTS //////////////////
        x = Input.GetAxis("Horizontal");
        y = Input.GetAxis("Vertical");

        if (Input.GetKeyDown(KeyCode.LeftShift) && isMagicActive == false && isAiming == false && isAttacking == false && die == false && controlSlow == false && energy > 0)
            run = true;

        if (Input.GetKeyUp(KeyCode.LeftShift) || energy <= 0)
        {
            anim.speed = 1;
            run = false;
        }

        if (Input.GetKeyDown(KeyCode.V) && isMagicActive == false && isAiming == false && isAttacking == false && roll == false && run && die == false && isWalking && energy > rollEnergyConsum)
            roll = true;

        if (Input.GetKeyDown(KeyCode.C) && isMagicActive == false && isAiming == false && roll == false && die == false && attackTimer == 0)
        {
            anim.speed = 1;
            isAttacking = true;
            controle = true;
        }

        if (Input.GetKeyDown(KeyCode.X) && isAttacking == false && isAiming == false && roll == false && die == false && magicTimer == 0)
        {
            anim.speed = 1;
            isMagicActive = true;
            controle = true;
        }

        if (Input.GetKeyDown(KeyCode.Z) && isAttacking == false && isMagicActive == false && roll == false && die == false && rangedTimer == 0 && ammo > 0)
        {
            anim.speed = 1;
            isAiming = true;
            controle = true;
        }

        ///////////////////////// SET ANIMATIONS //////////////////////
        if(roll)
            isWalking = true;

        else
            isWalking = (Mathf.Abs(x) + Mathf.Abs(y)) > 0;

        anim.SetBool("isWalking", isWalking);
        anim.SetBool("Roll", roll);
        die = anim.GetBool("Died");

        if (die)
        {
            anim.speed = 1;
            clawHUD.SetActive(false);
        }

        ///////////////// CLOSE ATTACK ////////////////
        if (isAttacking)
        {
            anim.SetBool("isAttacking", isAttacking);

            attackTimer += Time.deltaTime;

            if (attackTimer >= closeAttack.length)  // THIS VALUE IS THE CLOSE ATTACK ANIMATION TIME
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

            if (magicTimer >= magicAttack.length) // THIS VALUE IS THE MAGIC ANIMATION TIME
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


            if (rangedTimer >= rangedAttack.length)  // THIS VALUE IS THE LONG RANGE ANIMATION TIME
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
                ammo -= 1;
                rangedTimer = 0;
            }
        }

        if(roll)
        {
            run = true;
            if(rollTimer == 0)
            {
                curAxis = new Vector2(x, y);
                GetComponent<EnergyBar>().curEnergy -= rollEnergyConsum;
            }

            anim.speed = 2f;

            rollTimer += Time.deltaTime;
            transform.Translate(curAxis.x * vel * 3f * Time.deltaTime, curAxis.y * vel * 3f * Time.deltaTime, 0);

            if (rollTimer >= rollClip.length * 0.5f)
            {
                anim.SetFloat("x", curAxis.x);
                anim.SetFloat("y", curAxis.y);
                roll = false;
                rollTimer = 0;
                anim.speed = 1f;

                if (Input.GetKey(KeyCode.LeftShift))
                    run = true;

                else
                    run = false;
            }

        }

        ///////////////// WALK ///////////////
        if (isWalking && isAttacking == false && isMagicActive == false && isAiming == false && die == false && roll == false)
        {
            if (run)
            {
                Move(runVel);

                GetComponent<EnergyBar>().curEnergy -= 10 * Time.deltaTime;
            }

            else
            {
                Move(1);
            }
        }

        //////////// ATTACK DETECTION WHILE COLLIDING //////////////////
        if (colGO != null)
        {
            if (colGO.transform.position.y > transform.position.y && anim.GetFloat("y") > 0 && anim.GetFloat("x") == 0 || colGO.transform.position.y < transform.position.y && anim.GetFloat("y") < 0 && anim.GetFloat("x") == 0 || colGO.transform.position.x > transform.position.x && anim.GetFloat("x") > 0 && anim.GetFloat("y") == 0 || colGO.transform.position.x < transform.position.x && anim.GetFloat("x") < 0 && anim.GetFloat("y") == 0)
            {
                if (colGO.gameObject.tag == "Enemy" && isAttacking && controle && attackTimer >= 0.4f)
                {
                       colGO.GetComponent<EnemyHealth>().TakeDamage(dmg["Close"]);
                       controle = false;

                }
            }
        }

        if(controlSlow)
        {
            slowTimer += Time.deltaTime;
            anim.speed = 1;
            run = false;
            if (slowTimer > maxSlowTime)
            {
                vel = initialVel;
                controlSlow = false;
                clawHUD.SetActive(false);
                slowTimer = 0;
            }
        }
    }

    public void SpeedDown(float slow, float time)
    {
        if (controlSlow == false)
        {
            clawHUD.SetActive(true);
            controlSlow = true;
            maxSlowTime = time;
            initialVel = vel;
            vel *= slow;
        }
    }

    //////////// DETECT COLLISIONS /////////////////
    private void OnTriggerStay2D(Collider2D col)
    {
        colGO = col.gameObject;
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        colGO = null;
    }

    private void Move(float speed)
    {
        anim.speed = speed;

        anim.SetFloat("x", x);
        anim.SetFloat("y", y);

        ///////////// MOVE THE PLAYER ///////////////////
        transform.Translate(x * vel * speed * Time.deltaTime, y * vel * speed * Time.deltaTime, 0);
    }
}