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

    private float attackTimer;

    private bool isAttacking;
    [SerializeField]
    private AnimationClip closeAttack;

    private bool isMagicActive;
    [SerializeField]
    private AnimationClip magicAttack;

    private bool isAiming;
    private bool controlArrow;
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

    [SerializeField]
    private Texture2D texture;
    private Sprite[] ranged;

    public bool pursuit;
    public Transform item;

    [SerializeField]
    private AnimationClip pickUPClip;
    private bool pickUPRunning;
    // Use this for initialization
    void Start()
    {
        /////////// PICK UO ALL THE LONG RANGE ATTACK ANIMATION FRAMES //////////////
        ranged = Resources.LoadAll<Sprite>(string.Format("Player/Sprites/PlayerLRA", texture.name));

        /////////// SET VARIABLES OF SOME ACTIONS ///////////
        //ammo = maxAmmo;
        clawHUD.SetActive(false);
        rollEnergyConsum = 10;
        arrowVel = 15f;
        runVel = 1.8f;
        controlArrow = true;

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

        ammo = Inventory.instance.arrows;

        //////////// READ THE INPUTS //////////////////
        x = Input.GetAxis("Horizontal");
        y = Input.GetAxis("Vertical");

        if (pursuit && item != null && anim.GetBool("PickUp") == false)
            PursuitItem(item);

        if (Input.GetKeyDown(KeyCode.LeftShift) && isMagicActive == false && isAiming == false && isAttacking == false && die == false && controlSlow == false && energy > 0)
            run = true;

        if (Input.GetKeyUp(KeyCode.LeftShift) || energy <= 0)
        {
            anim.speed = 1;
            run = false;
        }

        if (Input.GetKeyDown(KeyCode.V) && isMagicActive == false && isAiming == false && isAttacking == false && roll == false && die == false && isWalking && energy > rollEnergyConsum  && anim.GetBool("PickUp") == false)
            roll = true;

        if (Input.GetKeyDown(KeyCode.C) && isMagicActive == false && isAiming == false && roll == false && die == false && isAttacking == false && anim.GetBool("PickUp") == false)
        {
            anim.speed = 1;
            isAttacking = true;
            controle = true;
        }

        if (Input.GetKeyDown(KeyCode.X) && isAttacking == false && isAiming == false && roll == false && die == false && isMagicActive == false && anim.GetBool("PickUp") == false)
        {
            anim.speed = 1;
            isMagicActive = true;
            controle = true;
        }

        if (Input.GetKeyDown(KeyCode.Z) && isAttacking == false && isMagicActive == false && roll == false && die == false && isAiming == false && ammo > 0 && anim.GetBool("PickUp") == false)
        {
            anim.speed = 1;
            isAiming = true;
            controle = true;
        }

        ///////////////////////// SET ANIMATIONS //////////////////////
        if (anim.GetBool("PickUp") == false)
        {
            if (roll)
                isWalking = true;

            else
                isWalking = (Mathf.Abs(x) + Mathf.Abs(y)) > 0;
        }

        if (pursuit == false)
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
            StartCoroutine(Attacks("isAttacking", isAttacking, closeAttack.length));
        }

        ///////////////// MAGIC ////////////////////
        if (isMagicActive)
        {
            StartCoroutine(Attacks("isMagicActive", isMagicActive, magicAttack.length));
        }

        ////////////////// LONG RANGE //////////////////
        if (isAiming)
        {
            StartCoroutine(Attacks("isAiming", isAiming, rangedAttack.length));
        }

        //////////// PLAYER ROOL /////////////////
        if (roll)
        {
            GetComponent<Collider2D>().enabled = false;
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
                GetComponent<Collider2D>().enabled = true;

                if (Input.GetKey(KeyCode.LeftShift))
                    run = true;

                else
                    run = false;
            }

        }

        ///////////////// WALK ///////////////
        if (isWalking && isAttacking == false && isMagicActive == false && isAiming == false && die == false && roll == false && anim.GetBool("PickUp") == false)
        {
            pursuit = false;
            item = null;

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
                if (anim.GetBool("PickUp") == false && colGO.gameObject.tag == "Enemy" && isAttacking && controle && attackTimer >= (closeAttack.length / 5) * 4)
                {
                       colGO.GetComponent<EnemyHealth>().TakeDamage(dmg["Close"]);
                       controle = false;

                }
            }
        }

        //////////// SLOW-PLAYER CONTROLLER /////////////////

        if (controlSlow)
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

    //////////// DETECT COLLISIONS /////////////////
    private void OnTriggerStay2D(Collider2D col)
    {
        colGO = col.gameObject;
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        colGO = null;
    }

    //////////// SLOW THE PLAYER /////////////////

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

    private void Move(float speed)
    {
        anim.speed = speed;

        anim.SetFloat("x", x);
        anim.SetFloat("y", y);

        ///////////// MOVE THE PLAYER ///////////////////
        transform.Translate(x * vel * speed * Time.deltaTime, y * vel * speed * Time.deltaTime, 0);
    }

    IEnumerator Attacks(string attackName, bool attackType, float attackClipLenght)
    {
        anim.SetBool(attackName, attackType);
        attackTimer += Time.deltaTime;
        bool above = false;

        Quaternion rot = Quaternion.Euler(0, 0, 0);

        for(int i = 0; i < ranged.Length; i++)
        {
            if(GetComponent<SpriteRenderer>().sprite == ranged[i])
            {
                if(i <= 12) // FACING TOP
                {
                    rot = Quaternion.Euler(0, 0, 90);
                    above = true;
                }

                else if (i > 12 && i <= 25) // FACING LEFT
                {
                    rot = Quaternion.Euler(0, 0, 180);
                }

                else if (i > 26 && i <= 38) // FACING BOTTOM
                {
                    rot = Quaternion.Euler(0, 0, -90);
                }

                else
                    rot = Quaternion.Euler(0, 0, 0); // FACING RIGHT
            }
        }

        if (attackName == "isAiming" && attackTimer >= attackClipLenght - 0.2f && controlArrow)
        {
            GameObject arrow = (GameObject)Instantiate(arrowPrefab, transform.position, rot);
            arrow.GetComponent<Arrow>().arrowVel = arrowVel;
            arrow.GetComponent<Arrow>().dmg = dmg["Range"];
            Inventory.instance.arrows -= 1;
            if(above)
                arrow.GetComponent<SpriteRenderer>().sortingOrder = GetComponent<SpriteRenderer>().sortingOrder - 1;

            else
                arrow.GetComponent<SpriteRenderer>().sortingOrder = GetComponent<SpriteRenderer>().sortingOrder + 1;
                
            above = false;
            controlArrow = false;
        }

        yield return new WaitForSeconds(attackClipLenght);

        attackType = false;
        isAttacking = false;
        isMagicActive = false;
        isAiming = false;
        controlArrow = true;
        attackTimer = 0;
        anim.SetBool(attackName, attackType);

        StopAllCoroutines();
    }

    public void PursuitItem(Transform target)
    {
        float xP = 0;
        float yP = 0;

        if (target.position.x > transform.position.x && Mathf.Abs(target.transform.position.x - transform.position.x) > Mathf.Abs(target.transform.position.y - transform.position.y))
        {
            xP = 1;
            yP = 0;
        }

        else if (target.position.x < transform.position.x && Mathf.Abs(target.transform.position.x - transform.position.x) > Mathf.Abs(target.transform.position.y - transform.position.y))
        {
            xP = -1;
            yP = 0;
        }

        else if (target.position.y > transform.position.y && Mathf.Abs(target.transform.position.y - transform.position.y) > Mathf.Abs(target.transform.position.x - transform.position.x))
        {
            yP = 1;
            xP = 0;
        }

        else if (target.position.y < transform.position.y && Mathf.Abs(target.transform.position.y - transform.position.y) > Mathf.Abs(target.transform.position.x - transform.position.x))
        {
            yP = -1;
            xP = 0;
        }

        anim.SetFloat("x", xP);
        anim.SetFloat("y", yP);

        if (item.GetComponent<ItemPickUP>().range > 0.6f)
        {

            anim.SetBool("isWalking", true);
            pickUPRunning = false;
            transform.position = Vector2.MoveTowards(transform.position, target.position, vel * Time.deltaTime);
        }

        else if(item.GetComponent<ItemPickUP>().range <= 0.6f && pickUPRunning == false)
        {
            anim.SetBool("isWalking", false);
            pickUPRunning = true;
            StartCoroutine("PickUPWait");
        }
    }

    IEnumerator PickUPWait()
    {
        anim.SetBool("PickUp", true);

        yield return new WaitForSeconds(pickUPClip.length);

        if (item != null)
            item.GetComponent<ItemPickUP>().picked = true;

        anim.SetBool("PickUp", false);
        //pickUPRunning = false;
        StopCoroutine("PickUPWait");
    }
}