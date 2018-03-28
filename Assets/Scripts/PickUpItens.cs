using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpItens : MonoBehaviour {
    private PlayerMovement plMove;
    private Animator anim;
    private bool pickUp;
    private bool col;

    [SerializeField]
    private AnimationClip pickUpItemClip;

    [SerializeField]
    private Sprite facingUp;

    private GameObject colGO;

    [SerializeField]
    private GameObject pressE;
    // Use this for initialization
    void Start () {
        plMove = GetComponent<PlayerMovement>();
        anim = GetComponent<Animator>();
        pressE.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
        anim.SetBool("PickUp", pickUp);

        if (col && GetComponent<SpriteRenderer>().sprite == facingUp && anim.GetBool("isWalking") == false)
            pressE.SetActive(true);

        else
            pressE.SetActive(false);

            if (Input.GetKeyDown(KeyCode.E) && anim.GetBool("isWalking") == false && anim.GetBool("isAttacking") == false && anim.GetBool("isAiming") == false && anim.GetBool("isMagicActive") == false && pickUp == false)
        {
            if(col && GetComponent<SpriteRenderer>().sprite == facingUp)
                StartCoroutine("PickUpItem");
        }
	}

    IEnumerator PickUpItem()
    {
        pickUp = true;

        yield return new WaitForSeconds(pickUpItemClip.length);

        //Pick up the item here

        col = false;
        pickUp = false;
        colGO = null;

        StopAllCoroutines();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Chest")
        {
            col = true;
            colGO = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Chest")
        {
            col = false;
            colGO = null;
        }
    }
}
