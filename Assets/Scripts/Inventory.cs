using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour {

    #region Singleton

    public static Inventory instance;

    void Awake()
    {
        if(instance != null)
        {
            Debug.LogWarning("More than one instance of Inventory found!");
            return;
        }

        instance = this;
    }

    #endregion

    public delegate void OnItemChanged();
    public OnItemChanged onIntemChangedCallback;

    public int space = 15;

    public int arrows;

    public bool removeAll;

    public Text description;

    public List<ItemScript> items = new List<ItemScript>();

    public void Update()
    {
        if(arrows == 0 && items.Count > 0 && !removeAll)
        {
            for(int i = 0; i < items.Count; i++)
            {
                if (items[i].name == "Arrow")
                {
                    Remove(items[i]);
                    i = items.Count;
                }
            }
        }

        if (items.Count == 0)
        {
            removeAll = false;
            arrows = 0;
        }
    }

    public bool Add(ItemScript item)
    {
        if (!item.isDefaultItem)
        {
            if (items.Count >= space)
            {
                Debug.Log("Not enough room.");
                return false;
            }

            if (item.name == "Arrow")
            {
                if (arrows > 0)
                {
                    arrows += 1;
                }

                else
                {
                    arrows += 1;
                    items.Add(item);
                }
            }

            else
                items.Add(item);

            if(onIntemChangedCallback != null)
                onIntemChangedCallback.Invoke();
        }

        return true;
    }

    public void Remove(ItemScript item)
    {
        items.Remove(item);

        if (item.name == "Arrow")
            arrows = 0;

        if (onIntemChangedCallback != null)
            onIntemChangedCallback.Invoke();
    }

    public void RemoveAll()
    {/*
        int max = items.Count;

        for (int i = 0; i < max; i++)
        {
            items.Remove(items[0]);
        }*/

        removeAll = true;

        //arrows = 0;

        if (onIntemChangedCallback != null)
            onIntemChangedCallback.Invoke();
    }

}
