using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemHUD : MonoBehaviour {
    [SerializeField]
    private GameObject itemBox;

    [SerializeField]
    private Image itemImg;

    [SerializeField]
    private Text itenAmount;

    private PlayerMovement plMove;
	// Use this for initialization
	void Start () {
        plMove = GetComponent<PlayerMovement>();
	}
	
	// Update is called once per frame
	void Update () {
        itenAmount.text = "X" + plMove.ammo;

        if (plMove.ammo == 0)
            itemBox.SetActive(false);
	}
}
