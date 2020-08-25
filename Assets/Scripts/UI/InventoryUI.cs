using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour {
    public Transform slotContainer;
    // References to the slots that display the items
    private InventorySlot[] _inventorySlots;
    // Pairs items with the index of the slot containing it
    private Dictionary<Item, int> _displayedItems = new Dictionary<Item, int>();
    // Index of the least significant empty slot
    private int _earliestEmptySlot;
    private Inventory _inventory;
    private bool _extraSlotsUnlocked;

    private void Awake() {
        // Initialize slot dictionary
        _inventorySlots = slotContainer.GetComponentsInChildren<InventorySlot>();
        foreach (InventorySlot slot in _inventorySlots) {
            slot.Empty();
        }

        // Hide locked inventory slots
        SetLockedSlotsStatus(false);

        _earliestEmptySlot = 0;
        _inventory = GameManager.instance.player.inventory;
    }

    /*
    public int GetSlotNum() {
        return _inventorySlots.Length;
    }
    */

    // Returns the slot associated with given item, or
    public InventorySlot GetSlot(Item item) {
        if (!_displayedItems.ContainsKey(item)) {
                return null;
        }
        return _inventorySlots[_displayedItems[item]];
    }

    public void SetLockedSlotsStatus(bool active) {
        if (active) {
            int lockedSlots = GameManager.instance.player.inventory.lockedSlots;
            int maxSlots = _inventorySlots.Length;
            InventorySlot slot;
            // Set slots active
            for (int i = 0; i < lockedSlots; i++) {
                slot = _inventorySlots[maxSlots - 1 - i];
                slot.Empty();
                slot.gameObject.SetActive(true);
            }
        } else {
            int lockedSlots = GameManager.instance.player.inventory.lockedSlots;
            int maxSlots = _inventorySlots.Length;
            InventorySlot slot;
            // Set slots inactive and move any items in them elsewhere
            for (int i = 0; i < lockedSlots; i++) {
                slot = _inventorySlots[maxSlots - 1 - i];
                if (!slot.IsEmpty()) {
                    // TODO: move icons to earliest free space
                    slot.Empty();
                }
                slot.gameObject.SetActive(false);
            }
        }
        _extraSlotsUnlocked = active;
    }

    // Adds or removes items from the inventory display and return the inventory slot associated with the given item, or null if there is no such slot
    public InventorySlot UpdateItem(Item item) {
        if (!_displayedItems.ContainsKey(item)) {
            if (_earliestEmptySlot >= GameManager.instance.player.inventory.GetUnlockedSlots()) {
                Debug.LogWarning("Tried to add new item when there are no free inventory slots.");
                return null;
            }
            AddNewItem(item);
        }
        int slotIndex = _displayedItems[item];
        InventorySlot associatedSlot = _inventorySlots[slotIndex];
        int itemQuantity = GameManager.instance.player.inventory.GetQuantity(item);
        if (itemQuantity == 0) {
            // Empty slots for items with 0 quantity
            associatedSlot.Empty();
            _displayedItems.Remove(item);
            _earliestEmptySlot = Mathf.Min(slotIndex, _earliestEmptySlot);
        } else {
            // Update slot that contains item
            associatedSlot.SetSlot(item);
        }
        return associatedSlot;
    }

    private void AddNewItem(Item item) {
        // Add new item to display
        _displayedItems.Add(item, _earliestEmptySlot);
        // Update the earliest empty slot index
        for (int i = _earliestEmptySlot + 1; i < _inventorySlots.Length; i++) {
            if (_inventorySlots[i].IsEmpty()) {
                _earliestEmptySlot = i;
                break;
            }
        }
    }
}
