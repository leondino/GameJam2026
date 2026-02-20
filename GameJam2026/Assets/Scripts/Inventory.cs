using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Inventory : MonoBehaviour
{
    public int InventorySize { get; set; } = 3;
    public List<ItemData> inventoryItems = new List<ItemData>();
    public ItemData selectedItem;
    private InventoryBarManager inventoryBarManager;
    private int selectedItemIndex = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Fill the inventory with null values to represent empty slots, so that we can easily check for empty slots later
        for (int i = 0; i < InventorySize; i++)
        {
            inventoryItems.Add(null);
        }
        selectedItem = inventoryItems[selectedItemIndex];

        inventoryBarManager = GameManager.Instance.inventoryBarManager;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddItemToInventory(ItemData itemToAdd)
    {
        foreach (ItemData item in inventoryItems) 
        {
            if (item == null) 
            {
                int index = inventoryItems.IndexOf(item);
                inventoryItems[index] = itemToAdd;
                inventoryBarManager.UpdateInventorySlot(index, itemToAdd);
                selectedItem = inventoryItems[selectedItemIndex];
                Debug.Log($"Added {itemToAdd.itemName} to inventory slot {index}");
                return;
            }
        }
        Debug.Log("Inventory is full! Cannot add item.");
    }

    private void SelectNextItem()
    {
        if (selectedItemIndex < InventorySize - 1)
            selectedItemIndex++;
        else
            selectedItemIndex = 0;
        selectedItem = inventoryItems[selectedItemIndex];
        inventoryBarManager.HighlightSelectedSlot(selectedItemIndex);
        Debug.Log($"Selected item: {(selectedItem != null ? selectedItem.itemName : "Empty Slot")}");
    }

    private void SelectPreviousItem()
    {
        if (selectedItemIndex > 0)
            selectedItemIndex--;
        else
            selectedItemIndex = InventorySize-1;
        selectedItem = inventoryItems[selectedItemIndex];
        inventoryBarManager.HighlightSelectedSlot(selectedItemIndex);
        Debug.Log($"Selected item: {(selectedItem != null ? selectedItem.itemName : "Empty Slot")}");
    }

    public void OnSelectNextItem(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            SelectNextItem();
        }
    }

    public void OnSelectPreviousItem(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            SelectPreviousItem();
        }
    }

    public void OnSelectWithScroll(InputAction.CallbackContext context)
    {
        float scrollValue = context.ReadValue<float>();
        if (scrollValue > 0)
        {
            SelectNextItem();
        }
        else if (scrollValue < 0)
        {
            SelectPreviousItem();
        }
    }
}
