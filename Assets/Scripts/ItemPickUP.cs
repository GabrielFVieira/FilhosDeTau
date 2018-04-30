using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUP : MonoBehaviour {
    public float range;
    private Transform target;

    public ItemScript item;

    public bool picked;

    public bool OnChest;

    public bool clicked;
    // Use this for initialization
    void Start () {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
	}
	
	// Update is called once per frame
	void Update () {
        range = Vector2.Distance(transform.position, target.position);

        if(target.GetComponent<PlayerMovement>().pursuit && range < 0.6f && target.GetComponent<PlayerMovement>().item == GetComponent<Transform>() && picked)
        {
            target.GetComponent<PlayerMovement>().pursuit = false;
            target.GetComponent<PlayerMovement>().item = null;
            PickUP();
        }
    }

    void PickUP()
    {
        if(item.name == "Arrow")
        {
            for(int i = 0; i < Inventory.instance.items.Count; i++)
            {
                if(Inventory.instance.items[i].name == "Arrow")
                {
                    Inventory.instance.arrows += 1;
                    i = Inventory.instance.items.Count;
                    Destroy(gameObject);
                    return;
                }
            }

        }

        //Debug.Log("Picking up " + item.name);
        bool wasPickedUp = Inventory.instance.Add(item);

        if (wasPickedUp)
        {
            if(item.name != "Arrow")
                FindObjectOfType<InventoryUI>().inventoryUI.SetActive(true);

            Destroy(gameObject);
        }

        else
            FindObjectOfType<InventoryUI>().inventoryUI.SetActive(true);

    }

    private void OnMouseOver()
    {
        if (Input.GetKeyDown(target.gameObject.GetComponent<PlayerMovement>().pursuitButton))
        {
            if (OnChest)
                PickUP();

            else if (OnChest == false && target.GetComponent<Animator>().GetBool("PickUp") == false)
            {
                target.GetComponent<PlayerMovement>().pursuit = true;
                target.GetComponent<PlayerMovement>().item = GetComponent<Transform>();
            }
        }
    }
}
