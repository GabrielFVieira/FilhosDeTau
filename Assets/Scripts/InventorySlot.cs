using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour {

    public Image icon;
    public Button removeButton;
    ItemScript item;

    public void AddItem(ItemScript newItem)
    {
        item = newItem;

        icon.sprite = item.icon;
        icon.preserveAspect = true;
        icon.enabled = true;
        removeButton.interactable = true;
    }

    public void ClearSlot()
    {
        item = null;

        icon.sprite = null;
        icon.enabled = false;

        removeButton.interactable = false;
    }

    public void OnRemoveButton()
    {
        Inventory.instance.Remove(item);
    }

    public void UseItem()
    {
        if(item != null)
        {
            item.Use();
            Inventory.instance.description.text = "";
        }
    }

    public void ShowDescription()
    {
        if (item != null)
            Inventory.instance.description.text = item.description;

        else
            Inventory.instance.description.text = "";
    }
}
