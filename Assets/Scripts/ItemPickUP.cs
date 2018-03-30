using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUP : MonoBehaviour {
    public float range;
    private Transform target;

    public ItemScript item;

    public bool picked;

    public bool OnChest;
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
        //Debug.Log("Picking up " + item.name);
        bool wasPickedUp = Inventory.instance.Add(item);

        if (wasPickedUp)
        {
            FindObjectOfType<InventoryUI>().inventoryUI.SetActive(true);
            Destroy(gameObject);
        }

    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1))
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
