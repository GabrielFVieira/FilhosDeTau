using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class InvetoryManager : MonoBehaviour {
    public List<GameObject> invObj = new List<GameObject>();
    //public List<int> invObjAmount = new List<int>();
    public RectTransform[] pos;
    private bool[] posEnabled = new bool[15];
    public int hasSpace;

    [SerializeField]
    private Text txt;
    private bool dropAll;

    public GameObject inventory;
    // Use this for initialization
    void Start () {
        MakeBoolsTrue();
        hasSpace = posEnabled.Length;
        inventory.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.I))
            OpenCloseInv();

        if (dropAll)
        {
            foreach (GameObject go in invObj)
            {
                go.GetComponent<Item>().colInv = false;
                go.GetComponent<RectTransform>().localPosition = go.GetComponent<Item>().startPos;
                go.GetComponent<Item>().inInventory = false;
            }
            hasSpace = posEnabled.Length;
            MakeBoolsTrue();
            invObj.Clear();
            dropAll = false;
        }
	}

    public void AddItem(GameObject obj)
    {
        for (int x = 0; x < posEnabled.Length; x++)
        {
            if(posEnabled[x])
            {
                obj.GetComponent<RectTransform>().localPosition = new Vector3(pos[x].localPosition.x, pos[x].localPosition.y, pos[x].localPosition.z);
                obj.GetComponent<Item>().index = x;
                posEnabled[x] = false;
                hasSpace -= 1;
                x = posEnabled.Length;
            }
        }
        invObj.Add(obj);
    }

    public void ReAlocate(GameObject obj)
    {
        obj.GetComponent<RectTransform>().localPosition = new Vector3(pos[obj.GetComponent<Item>().index].localPosition.x, pos[obj.GetComponent<Item>().index].localPosition.y, pos[obj.GetComponent<Item>().index].localPosition.z);
    }

    public void DropAll()
    {
        dropAll = true;
        txt.text = "";
    }

    public void DropThis(GameObject obj)
    {
        posEnabled[obj.GetComponent<Item>().index] = true;
        invObj.Remove(obj.gameObject);
        hasSpace += 1;
        txt.text = "";
    }

    private void MakeBoolsTrue()
    {
        for (int y = 0; y < posEnabled.Length; y++)
        {
            posEnabled[y] = true;
        }
    }

    public void Use()
    {
        if (GetComponent<AudioSource>().isPlaying == false)
        {
            GetComponent<AudioSource>().Play();
        }
    }

    public void OpenCloseInv()
    {
        inventory.SetActive(!inventory.activeSelf);
    }
}
