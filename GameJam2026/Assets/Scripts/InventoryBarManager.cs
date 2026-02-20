using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class InventoryBarManager : MonoBehaviour
{
    private const int SLOT_WIDTH = 46;
    private const int SLOT_OFFSET = 2;
    private const int BACKGROUND_OFFSET = 3;
    private int inventorySlots = 3;
    [SerializeField]
    private float highlightSizeFactor = 1.05f;
    [SerializeField]
    private Color highlightColor = Color.white;
    private Color baseColor;

    private RectTransform inventoryBarTransform;
    [SerializeField]
    private GameObject inventorySlotPrefab;
    List<GameObject> inventorySlotsList = new List<GameObject>();
    private GameObject highlightedSlot;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inventoryBarTransform = GetComponent<RectTransform>();
        UpdateInventoryBarSize();
        baseColor = inventorySlotPrefab.GetComponent<UnityEngine.UI.Image>().color;
        HighlightSelectedSlot(0);
    }

    /// <summary>
    /// Updates the specified inventory slot to display the provided item's icon or clears the slot if no item is
    /// provided.
    /// </summary>
    /// <remarks>If the specified item is not null, the slot displays the item's icon. If the item is null,
    /// the slot is cleared and the icon is hidden. Ensure that the slotIndex is within the bounds of the inventory
    /// slots list to avoid runtime exceptions.</remarks>
    /// <param name="slotIndex">The zero-based index of the inventory slot to update. Must be a valid index within the inventory slots list.</param>
    /// <param name="item">The item data to display in the inventory slot. If null, the slot will be cleared.</param>
    public void UpdateInventorySlot(int slotIndex, ItemData item)
    {
        var itemImage = inventorySlotsList[slotIndex].transform.GetChild(0).GetComponent<UnityEngine.UI.Image>();
        if (item != null)
        {
            itemImage.enabled = true;
            itemImage.sprite = item.icon;
        }
        else itemImage.enabled = false;
    }

    /// <summary>
    /// Updates the inventory bar to reflect the current inventory size of the player.
    /// </summary>
    /// <remarks>This method adjusts the visual layout of the inventory bar by resizing it and adding
    /// inventory slot elements as needed. It ensures that the number of visible slots matches the player's inventory
    /// capacity. The inventory slot prefab must be correctly assigned for proper appearance. Call this method after
    /// changes to the player's inventory size to keep the UI in sync.</remarks>
    public void UpdateInventoryBarSize()
    {
        inventorySlots = GameManager.Instance.player.GetComponent<Inventory>().InventorySize;
        inventoryBarTransform.sizeDelta = new Vector2(SLOT_WIDTH * inventorySlots + BACKGROUND_OFFSET, inventoryBarTransform.sizeDelta.y);
        while (inventorySlots > inventorySlotsList.Count)
        {
            GameObject newSLot = Instantiate(inventorySlotPrefab, transform, false);
            inventorySlotsList.Add(newSLot);
            UpdateInventorySlot(inventorySlotsList.Count - 1, null);
        }
        foreach (GameObject slot in inventorySlotsList)
        {
            slot.GetComponent<RectTransform>().anchoredPosition = new Vector2(SLOT_WIDTH * inventorySlotsList.IndexOf(slot) + SLOT_OFFSET, 0);
        }
    }

    /// <summary>
    /// Highlights the inventory slot at the specified index to indicate selection, updating its visual appearance
    /// accordingly.
    /// </summary>
    /// <remarks>If another slot is already highlighted, its appearance is reset before highlighting the new
    /// slot. Ensure that the provided index is valid to prevent runtime exceptions.</remarks>
    /// <param name="slotIndex">The zero-based index of the inventory slot to highlight. Must be within the bounds of the inventory slots list.</param>
    public void HighlightSelectedSlot(int slotIndex)
    {
        if (highlightedSlot != null)
        {
            highlightedSlot.GetComponent<UnityEngine.UI.Image>().color = baseColor;
            highlightedSlot.GetComponent<RectTransform>().sizeDelta /= highlightSizeFactor;
        }
        highlightedSlot = inventorySlotsList[slotIndex];
        highlightedSlot.GetComponent<UnityEngine.UI.Image>().color = highlightColor;
        highlightedSlot.GetComponent<RectTransform>().sizeDelta *= highlightSizeFactor;
    }
}
