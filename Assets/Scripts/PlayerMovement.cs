using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    #region Public Variables
    public int ammo;
    public int maxAmmo = 10;
    public GameObject colGO;
    public bool controle;
    public bool pursuit;
    public Transform item;
    public GameObject aim;
    public GameObject hit;
    public string activeWeapon;
    public GameObject inventory;

    public KeyCode attackButton;
    public KeyCode pursuitButton;
    public KeyCode rollButton;
    public KeyCode runButton;
    public KeyCode magicButton;
    #endregion

    #region Private Variables
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
    private bool die;
    private Dictionary<string, int> dmg = new Dictionary<string, int>();
    private float slowTimer;
    private float maxSlowTime;
    private float initialVel;
    private bool controlSlow;
    [SerializeField]
    private GameObject clawHUD;
    private bool roll;
    private float rollEnergyConsum;
    [SerializeField]
    private AnimationClip rollClip;
    private float rollTimer;
    private Vector2 curAxis = new Vector2();
    [SerializeField]
    private AnimationClip pickUPClip;
    private bool pickUPRunning;
    private Vector3 mousePos;
    #endregion

    // Use this for initialization
    void Start()
    {
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

        mousePos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        bool mouseLook = true;

        #region Read The Inputs
        if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0 && anim.GetBool("isAttacking") == false && anim.GetBool("isAiming") == false && anim.GetBool("isMagicActive") == false && !roll && anim.GetBool("PickUp") == false)
        {
            isWalking = false;
            mouseLook = true;
            x = (mousePos.x * 2) - 1;
            y = (mousePos.y * 2) - 1;
            anim.SetFloat("x", x);
            anim.SetFloat("y", y);
        }

        else
        {
            x = Input.GetAxis("Horizontal");
            y = Input.GetAxis("Vertical");
            mouseLook = false;
        }

        //Read Pursuit Item Input
        if (pursuit && item != null && anim.GetBool("PickUp") == false)
            PursuitItem(item);

        if (!pursuit)
        {
            //Read Run Input True
            if (Input.GetKeyDown(runButton) && !isMagicActive && 
                !isAiming && !isAttacking && !die && !controlSlow && energy > 0)
                run = true;

            //Read Run Input False
            if (Input.GetKeyUp(runButton) || energy <= 0)
            {
                anim.speed = 1;
                run = false;
            }

            //Read Roll Input
            if (Input.GetKeyDown(rollButton) && !isMagicActive && !isAiming &&
                !isAttacking && !roll && !die && isWalking && energy > rollEnergyConsum && anim.GetBool("PickUp") == false)
                roll = true;

            //Read Attack Input
            if (Input.GetKeyDown(attackButton) && !isMagicActive && !isAiming && !roll &&
                !die && !isAttacking && anim.GetBool("PickUp") == false && inventory.activeSelf == false)
            {
                anim.speed = 1;
                controle = true;

                if(activeWeapon == "PokeBall")
                    isAttacking = true;

                else if (activeWeapon == "Arrow" && ammo > 0)
                    isAiming = true;
            }

            //Read Magic Input
            if (Input.GetKeyDown(magicButton) && !isAttacking && !isAiming && !roll &&
                !die && !isMagicActive && anim.GetBool("PickUp") == false)
            {
                anim.speed = 1;
                isMagicActive = true;
                controle = true;
            }
        }
        #endregion

        #region Set Animations
        if (anim.GetBool("PickUp") == false)
        {
            if (roll)
                isWalking = true;

            else if(!roll && !mouseLook)
                isWalking = (Mathf.Abs(x) + Mathf.Abs(y)) > 0;
        }

        if (!pursuit)
            anim.SetBool("isWalking", isWalking);

        anim.SetBool("Roll", roll);
        die = anim.GetBool("Died");

        if (die)
        {
            GetComponent<CapsuleCollider2D>().isTrigger = true;
            anim.speed = 1;
            clawHUD.SetActive(false);
        }
        #endregion

        #region Set Movements Mechanics
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
            //GetComponent<Collider2D>().enabled = false;

            #region Define Roll Direction
            int rolX;
            int rolY;

            if (x > 0)
                rolX = 1;

            else if (x < 0)
                rolX = -1;

            else
                rolX = 0;

            if (y > 0)
                rolY = 1;

            else if (y < 0)
                rolY = -1;

            else
                rolY = 0;

            #endregion

            if (rollTimer == 0)
            {
                curAxis = new Vector2(rolX, rolY);
                GetComponent<EnergyBar>().curEnergy -= rollEnergyConsum;
            }

            anim.speed = 2f;

            rollTimer += Time.deltaTime;
            transform.Translate(curAxis.x * vel * 3f * Time.deltaTime, curAxis.y * vel * 3f * Time.deltaTime, 0);

            if (rollTimer >= rollClip.length * (1/anim.speed))
            {
                anim.SetFloat("x", curAxis.x);
                anim.SetFloat("y", curAxis.y);
                roll = false;
                rollTimer = 0;
                anim.speed = 1f;
                //GetComponent<Collider2D>().enabled = true;
            }
        }

        ///////////////// WALK ///////////////
        if (isWalking && !isAttacking && !isMagicActive && !isAiming && !die && !roll && anim.GetBool("PickUp") == false)
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
                if (anim.GetBool("PickUp") == false && colGO.gameObject.tag == "Enemy" &&
                    isAttacking && controle && attackTimer >= (closeAttack.length / 5) * 4)
                {
                       colGO.GetComponent<EnemyHealth>().TakeDamage(dmg["Close"]);
                       controle = false;
                       colGO = null;
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

        #endregion
    }

    //////////// SLOW THE PLAYER /////////////////
    public void SpeedDown(float slow, float time)
    {
        if (!controlSlow)
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
        if (attackTimer == 0)
        {
            hit.transform.rotation = aim.transform.rotation;

            if (attackName != "isAiming")
                hit.GetComponent<BoxCollider2D>().enabled = true;

            aim.GetComponent<Aim>().canAim = false;
            x = (mousePos.x * 2) - 1;
            y = (mousePos.y * 2) - 1;
            anim.SetFloat("x", x);
            anim.SetFloat("y", y);
        }
        anim.SetBool(attackName, attackType);
        attackTimer += Time.deltaTime;
        bool above = false;

        if (attackName == "isAiming" && attackTimer >= attackClipLenght - 0.2f && controlArrow)
        {
            if (y > 0)
                above = true;

            GameObject arrow = (GameObject)Instantiate(arrowPrefab, transform.position, aim.transform.rotation);

            if (above)
                arrow.GetComponent<SpriteRenderer>().sortingOrder = GetComponent<SpriteRenderer>().sortingOrder - 1;

            else
                arrow.GetComponent<SpriteRenderer>().sortingOrder = GetComponent<SpriteRenderer>().sortingOrder + 1;

            arrow.GetComponent<Arrow>().arrowVel = arrowVel;
            arrow.GetComponent<Arrow>().dmg = dmg["Range"];
            Inventory.instance.arrows -= 1;
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
        aim.GetComponent<Aim>().canAim = true;
        hit.GetComponent<BoxCollider2D>().enabled = false;
        hit.GetComponent<HitCollider>().colliding = false;
        StopAllCoroutines();
    }

    public void PursuitItem(Transform target)
    {
        float xP = 0;
        float yP = 0;

        float offsetX = Mathf.Abs(target.transform.position.x - transform.position.x);
        float offsetY = Mathf.Abs(target.transform.position.y - transform.position.y);

        if (target.position.x > transform.position.x && offsetX > offsetY)
        {
            xP = 1;
            yP = 0;
        }

        else if (target.position.x < transform.position.x && offsetX > offsetY)
        {
            xP = -1;
            yP = 0;
        }

        else if (target.position.y > transform.position.y && offsetY > offsetX)
        {
            yP = 1;
            xP = 0;
        }

        else if (target.position.y < transform.position.y && offsetY > offsetX)
        {
            yP = -1;
            xP = 0;
        }

        anim.SetFloat("x", xP);
        anim.SetFloat("y", yP);

        // ITEM BELOW PLAYER
        if (item.GetComponent<ItemPickUP>().range == 0 && item.GetComponent<ItemPickUP>().clicked == false)
        {
            pickUPRunning = false;
            item.GetComponent<ItemPickUP>().clicked = true;
            anim.SetFloat("x", 0);
            anim.SetFloat("y", 1);
        }

		if (item.GetComponent<ItemPickUP>().range <= 0.6f && item.GetComponent<ItemPickUP>().range > 0 && item.GetComponent<ItemPickUP>().clicked == false)
        {
            pickUPRunning = false;
            item.GetComponent<ItemPickUP>().clicked = true;
        }
		
        if (item.GetComponent<ItemPickUP>().range > 0.6f)
        {

            anim.SetBool("isWalking", true);
            pickUPRunning = false;
            transform.position = Vector2.MoveTowards(transform.position, target.position, vel * Time.deltaTime);
        }

        else if(item.GetComponent<ItemPickUP>().range <= 0.6f && !pickUPRunning)
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
        StopCoroutine("PickUPWait");
    }
}