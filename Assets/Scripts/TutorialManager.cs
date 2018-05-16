using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    // Use this for initialization
    void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        inventoryUI = GameObject.FindObjectOfType<InventoryUI>();
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
}
