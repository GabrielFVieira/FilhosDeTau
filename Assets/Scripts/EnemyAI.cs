using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour {
    private Animator anim;
    private float maxDist;
    private float minDist;
    public bool isAttacking;
    private bool isWalking;
    private bool isDead;

    public Transform spawn;
    public Transform target;
    public float speed = 2f;
    private float range;

    private float x;
    private float y;

    public int dmg;
    private GameObject colGO;
    private float timer;
    public bool controle;

    public AnimationClip attackAnim;
    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();
        transform.position = spawn.position;
        target = GameObject.FindGameObjectWithTag("Player").transform;
        maxDist = 7;
        minDist = 1.1f;
        dmg = 10;
        colGO = null;
        speed = 3.5f;
    }
	
	// Update is called once per frame
	void Update () {
        anim.SetBool("isWalking", isWalking);
        anim.SetBool("isAttacking", isAttacking);
        anim.SetFloat("x", x);
        anim.SetFloat("y", y);

        isDead = anim.GetBool("Died");

        if (Vector2.Distance(transform.position, target.position) > maxDist)
            isWalking = false;

        if (range <= minDist && target == spawn)
            isWalking = false;

        else if (range > minDist && target == spawn)
            isWalking = true;

        if(anim.GetBool("Hurt") || anim.GetBool("isFurious"))
        {
            isAttacking = false;
            isWalking = false;
        }

        if (controle)
            timer += Time.deltaTime;

        if (timer >= attackAnim.length)
        {
            if(colGO != null && isAttacking && isDead == false)
                colGO.GetComponent<PlayerHealth>().TakeDamage(dmg);

            isAttacking = false;
            isWalking = false;

            if (timer >= attackAnim.length * Random.Range(1.3f, 1.8f))
            {
                controle = false;
                timer = 0;
            }
        }

        if (GetComponent<EnemyHealth>().curHealth < GetComponent<EnemyHealth>().maxHealth)
        {
            maxDist = 15;
            GetComponent<EnemyHealth>().maxDistInv = 10;
        }

        range = Vector2.Distance(transform.position, target.position);

        if (range < maxDist && anim.GetBool("isFurious") == false && anim.GetBool("Hurt") == false && range != 0)
            isWalking = true;

        if (isWalking && isDead == false)
        {
            if(target.position.x > transform.position.x && Mathf.Abs(target.transform.position.x - transform.position.x) > Mathf.Abs(target.transform.position.y - transform.position.y))
            {
                //Debug.Log("Right");
                GetComponent<SpriteRenderer>().flipX = true; // Remove after put a new animation with all 4 way move
                x = 1;
                y = 0;
            }

            else if (target.position.x < transform.position.x && Mathf.Abs(target.transform.position.x - transform.position.x) > Mathf.Abs(target.transform.position.y - transform.position.y))
            {
                //Debug.Log("Left");
                GetComponent<SpriteRenderer>().flipX = false; // Remove after put a new animation with all 4 way move
                x = -1;
                y = 0;
            }

            else if (target.position.y > transform.position.y && Mathf.Abs(target.transform.position.y - transform.position.y) > Mathf.Abs(target.transform.position.x - transform.position.x))
            {
                //Debug.Log("Up");
                y = 1;
                x = 0;
            }

            else if (target.position.y < transform.position.y && Mathf.Abs(target.transform.position.y - transform.position.y) > Mathf.Abs(target.transform.position.x - transform.position.x))
            {
                //Debug.Log("Down");
                y = -1;
                x = 0;
            }

            if (range > minDist && isAttacking == false)
                transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

            if (range <= minDist && isAttacking == false && timer == 0 && GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>().curHealth > 0)
            {
                isAttacking = true;
                controle = true;
            }

            if (GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>().curHealth <= 0)
            {
                target = spawn;
                isWalking = true;
                minDist = 0;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
            colGO = col.gameObject;
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
            colGO = null;
    }
}
