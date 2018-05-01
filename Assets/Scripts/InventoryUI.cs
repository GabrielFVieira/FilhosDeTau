using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour {

    [HideInInspector]
    public bool canOpenInv;

    public Transform itemsParent;
    public GameObject inventoryUI;

    Inventory inventory;

    InventorySlot[] slots;

    private GameManager manager;
    [SerializeField]
    private KeyCode invButton;
	// Use this for initialization
	void Start () {
        if (FindObjectOfType<GameManager>() != null)
        {
            manager = FindObjectOfType<GameManager>();
            invButton = manager.buttons[8];
        }

        inventory = Inventory.instance;
        inventory.onIntemChangedCallback += UpdateUI;

        slots = itemsParent.GetComponentsInChildren<InventorySlot>();

        inventoryUI.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(invButton) && canOpenInv)
        {
            inventoryUI.SetActive(!inventoryUI.activeSelf);
        }

        if(inventoryUI.activeSelf == false)
            inventory.description.text = "";
    }

    void UpdateUI()
    {
        for(int i = 0; i < slots.Length; i ++)
        {
            if(i < inventory.items.Count)
            {
                slots[i].AddItem(inventory.items[i]);
            }

            else
            {
                slots[i].ClearSlot();
            }
        }

        //Debug.Log("UPDATING UI");
    }

    public void CloseInventory()
    {
        inventoryUI.SetActive(false);
    }

    public void ClearDescription()
    {
        inventory.description.text = "";
    }
}
