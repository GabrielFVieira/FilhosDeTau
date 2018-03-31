using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour {

    public Transform itemsParent;
    public GameObject inventoryUI;

    Inventory inventory;

    InventorySlot[] slots;

	// Use this for initialization
	void Start () {
        inventory = Inventory.instance;
        inventory.onIntemChangedCallback += UpdateUI;

        slots = itemsParent.GetComponentsInChildren<InventorySlot>();

        inventoryUI.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
		if(Input.GetButtonDown("Inventory"))
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
