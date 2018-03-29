using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Item : MonoBehaviour
{
    [SerializeField]
    private string description;
    private bool active;
    public bool inInventory;

    [SerializeField]
    private Text txt;

    private bool drag;
    [SerializeField]
    private GameObject inventory;

    private RectTransform activeArea;
    private RectTransform rect;

    private InvetoryManager invManager;

    public Vector3 startPos;

    public int index;
    public bool colInv;

    public GameObject invContainer;
    public Vector2 startSize;
    public Vector2 invSize;

    private GameObject chestContainer;

    [SerializeField]
    private GameObject chest;

    private float doubleClickTimer;
    private int clicks;
    // Use this for initialization
    void Start()
    {
        chestContainer = this.transform.parent.gameObject;
        startSize = GetComponent<RectTransform>().sizeDelta;
        startPos = GetComponent<RectTransform>().localPosition;
        rect = GetComponent<RectTransform>();
        activeArea = inventory.GetComponent<RectTransform>();
        invManager = GameObject.Find("InventoryManager").GetComponent<InvetoryManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (clicks != 0)
            doubleClickTimer += Time.deltaTime;

        if(clicks >= 2)
        {
            colInv = true;
            drag = false;
            clicks = 0;
            doubleClickTimer = 0;
        }

        if(doubleClickTimer > 0.3f)
        {
            clicks = 0;
            doubleClickTimer = 0;
        }


        if (active && inInventory)
        {
            txt.text = description;
        }

        if (inInventory)
        {
            transform.SetParent(invContainer.transform);
            GetComponent<RectTransform>().sizeDelta = invManager.pos[index].GetComponent<RectTransform>().sizeDelta;
            //GetComponent<RectTransform>().sizeDelta = invSize;
        }

        else
        {
            if (chest.GetComponent<Chest>().showItens)
            {
                transform.SetParent(chestContainer.transform);
                GetComponent<RectTransform>().sizeDelta = startSize;
                GetComponent<RectTransform>().localPosition = startPos;
            }

            else if(chest.GetComponent<Chest>().showItens == false && invManager.inventory.activeSelf)
                Destroy(gameObject);
        }

        if (drag)
            transform.position = Input.mousePosition;

        if (colInv)
        {
            if (drag == false)
            {
                if (invManager.hasSpace > 0)
                    inInventory = true;

                if (invManager.invObj.Contains(this.gameObject) == false && invManager.hasSpace > 0)
                    invManager.AddItem(this.gameObject);

                else
                    invManager.ReAlocate(this.gameObject);
            }

            else if (invManager.hasSpace == 0 && drag == false && inInventory == false)
                rect.localPosition = startPos;
        }

        else if(colInv == false && drag == false)
        {
            if (invManager.invObj.Contains(this.gameObject))
            {
                invManager.DropThis(this.gameObject);
                txt.text = "";
            }
            inInventory = false;
        }
    }

    public void ShowDescription()
    {
        active = true;
    }

    public void HideDescription()
    {
        active = false;
    }

    public void Click()
    {
        if(invManager.inventory.activeSelf)
            clicks += 1;
    }

    public void DragStart()
    {
        if (inInventory)
        {
            drag = true;
            transform.SetAsLastSibling();
        }
    }

    public void DragFinish()
    {
        drag = false;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Inventory")
            colInv = true;
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Inventory")
            colInv = false;
    }
}
