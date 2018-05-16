using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TutorialManager : MonoBehaviour {
    private GameObject player;
    private InventoryUI inventoryUI;

    [Header("Tutorial Lock Variable Controllers")]
    public bool canWalk;
    public bool canRun;
    public bool canAttack;
    public bool canRoll;
    public bool canUseMagic;
    public bool canPursuit;
    public bool canChangeWeapon;
    public bool canOpenInv;

    [Space(10)]

    public bool startBossFight;
    public GameObject startBossFightCol;
    public Transform bossArenaMiddle;
    public GameObject bossArenaCol;
    public GameObject normalCol;
    public GameObject wallCol;
    public GameObject topGround;

    public int levelPart;
    public bool[] partCompleted;
    public Transform[] areaMiddle;
    public float range;
    public bool walkBack;
    // Use this for initialization
    void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        inventoryUI = GameObject.FindObjectOfType<InventoryUI>();
	}

    private void Update()
    {
        if(partCompleted[levelPart] == false)
        {
            FollowObjective(areaMiddle[levelPart - 1]);
            levelPart -= 1;
            walkBack = true;
        }

        if (walkBack)
        {
            range = Vector2.Distance(player.transform.position, areaMiddle[levelPart].position);

            if(range == 0)
            {
                canWalk = true;
                canRun = true;
                canAttack = true;
                canRoll = true;
                canUseMagic = true;
                canPursuit = true;
                canChangeWeapon = true;

                walkBack = false;
            }

        }

        if (startBossFight)
        {
            if (player.GetComponent<PlayerMovement>().objectiveDist < 10)
            {
                normalCol.SetActive(false);
                wallCol.GetComponent<CompositeCollider2D>().isTrigger = true;
                topGround.GetComponent<TilemapRenderer>().sortingLayerName = "Objects";
                topGround.GetComponent<TilemapRenderer>().sortingOrder = -100;
            }
                if (player.GetComponent<PlayerMovement>().objectiveDist == 0)
            {
                canWalk = true;
                canRun = true;
                canAttack = true;
                canRoll = true;
                canUseMagic = true;
                canPursuit = true;
                canChangeWeapon = true;
                bossArenaCol.SetActive(true);
            }

        }
    }

    // Update is called once per frame
    void LateUpdate () {
        player.GetComponent<PlayerMovement>().canWalk = canWalk;
        player.GetComponent<PlayerMovement>().canRun = canRun;
        player.GetComponent<PlayerMovement>().canAttack = canAttack;
        player.GetComponent<PlayerMovement>().canRoll = canRoll;
        player.GetComponent<PlayerMovement>().canUseMagic = canUseMagic;
        player.GetComponent<PlayerMovement>().canPursuit = canPursuit;
        player.GetComponent<ItemHUD>().canChangeWeapon = canChangeWeapon;
        //inventoryUI.canOpenInv = canOpenInv;
    }

    public void StartBossFight()
    {
        player.GetComponent<PlayerMovement>().FollowObjective(bossArenaMiddle);
        startBossFightCol.SetActive(false);
        startBossFight = true;
        canWalk = false;
        canRun = false;
        canAttack = false;
        canRoll = false;
        canUseMagic = false;
        canPursuit = false;
        canChangeWeapon = false;
    }

    public void FollowObjective(Transform t)
    {
        player.GetComponent<PlayerMovement>().FollowObjective(t);
        canWalk = false;
        canRun = false;
        canAttack = false;
        canRoll = false;
        canUseMagic = false;
        canPursuit = false;
        canChangeWeapon = false;
    }
}
