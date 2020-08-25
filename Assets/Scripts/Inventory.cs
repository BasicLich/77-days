using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum ItemType {
    _gold,
    FOOD
}

public class Inventory : MonoBehaviour {
    private int _gold;
    private Dictionary<Item, int> _inventory = new Dictionary<Item, int>();
    private HashSet<Item> _nightEffectItems = new HashSet<Item>();
    public int maxSlots;
    [Tooltip("Number of slots that exist but are initially hidden.")]
    public int lockedSlots;
    private int _currentSlots;

    private void Awake() {
        // Initialize inventory
        // foreach (ItemType itemType in System.Enum.GetValues(typeof(ItemType))) {
        //     _inventory.Add(itemType, 0);
        // }
        // UIManager.instance.Update_goldDisplay(_inventory[ItemType._gold]);
        // UIManager.instance.UpdateFoodDisplay(_inventory[ItemType.FOOD]);
    }

    private void Start() {
        _currentSlots = maxSlots - lockedSlots;
    }

    public bool IsFull() {
        return _inventory.Count >= _currentSlots;
    }
    
    public int GetEmptySlots() {
        return _currentSlots - _inventory.Count;
    }

    // Return true iff inventory has enough free space to handle the trade specified from the given get and cost.
    // Note that cost has positive values. Also, CardEffect items should be unique within the lists.
    public bool HasSpaceForItems(List<CardEffect> get, List<CardEffect> cost) {
        int freeSpace = GetEmptySlots();
        // Count items that are already in the inventory as free space
        foreach (CardEffect cardEffect in get) {
            if (cardEffect.item.resourceItem || HasItem(cardEffect.item) || cardEffect.item == GameManager.instance.inventoryUpgradeItem) {
                freeSpace++;
            }
        }
        if (freeSpace >= get.Count) {
            return true;
        }

        // Check if paying the cost will free enough space. Note this assumes the cost contains none of the items in get.
        foreach (CardEffect cardEffect in cost) {
            if (!cardEffect.item.resourceItem && GetQuantity(cardEffect.item) <= cardEffect.quantity) {
                freeSpace++;
            }
        }

        return freeSpace >= get.Count;
    }

    // Return true iff inventory has enough free space to handle the given cardEffectsSet from a card.
    public bool HasSpaceForItems(CardEffectsSet cardEffectsSet) {
        List<CardEffect> get = cardEffectsSet.GetReward();
        List<CardEffect> cost = cardEffectsSet.GetCost();
        return HasSpaceForItems(get, cost);
    }

    // Checks that the card effects set won't cause an item quantity limit to be surpassed
    public bool EffectsWithinLimit(List<CardEffect> get, List<CardEffect> cost) {
        return GetItemViolatingLimit(get, cost) == null;
    }

    // Returns violating item, or null if there is none
    public Item GetItemViolatingLimit(List<CardEffect> get, List<CardEffect> cost) {
        foreach (CardEffect getEffect in get) {
            Item item = getEffect.item;
            if (item.HasLimit()) {
                int prospectiveQuantity = GetQuantity(item) + getEffect.quantity;
                foreach (CardEffect costEffect in cost) {
                    if (costEffect.item == item) {
                        prospectiveQuantity -= costEffect.quantity;
                    }
                }
                if (prospectiveQuantity > item.GetLimit()) {
                    return item;
                }
            }
        }
        return null;
    }

    public int GetGold() {
        return _gold;
    }

    // Changes gold by specified amount
    public void AddGold(int quantity, bool showPopup = true) {
        _gold += quantity;
        UIManager.instance.UpdateGoldDisplay(_gold);
        if (showPopup) {
            UIManager.instance.ShowGoldPopup(quantity);
        }
        GameManager.instance.OnInventoryChanged();
    }

    public bool HasItem(Item item) {
        return _inventory.ContainsKey(item);
    }

    public int GetQuantity(Item item) {
        // Handle resource items
        if (item.resourceItem) {
            if (item == GameManager.instance.goldItem) {
                return _gold;
            } else if (item == GameManager.instance.healthItem) {
                return GameManager.instance.player.GetHealth();
            } else if (item == GameManager.instance.maxHealthItem) {
                return GameManager.instance.player.GetMaxHealth();
            } else if (item == GameManager.instance.dayItem) {
                return GameManager.instance.day;
            }
        }
        
        if (HasItem(item)) {
            return _inventory[item];
        } else {
            return 0;
        }
    }

    public void AddItem(Item item, int quantity, bool showPopup = true) {
        // Handle resource items
        if (item.resourceItem) {
            if (item == GameManager.instance.goldItem) {
                AddGold(quantity, showPopup);
            } else if (item == GameManager.instance.healthItem) {
                GameManager.instance.player.AddHealth(quantity, showPopup);
            } else if (item == GameManager.instance.maxHealthItem) {
                GameManager.instance.player.AddMaxHealth(quantity);
            } else if (item == GameManager.instance.dayItem) {
                // TODO // prevent time travelling/subtracting days -- used to make Work trade add days instead of subtract it
                GameManager.instance.AddDays(Mathf.Abs(quantity));
            }
        } else {
            // Add item as key to inv dict if there it isn't there
            if (!HasItem(item)) {
                if (item.CanAddNewItem()) {
                    AddNewItem(item);
                } else {
                    Debug.Log("Inventory is too full to add " + item.itemName);
                    return;
                }
            }
            if (quantity > 0) {
                item.OnAddItem(quantity);
            } else if (quantity < 0) {
                if (!item.CanBeDisposed()) {
                    return;
                }
                // Clamp item removals to remove no more than what player has
                quantity = -item.GetMaxDisposable(Mathf.Abs(quantity));
            }
            _inventory[item] += quantity;
            // Cap item if necessary
            if (item.HasLimit()) {
                _inventory[item] = Mathf.Min(_inventory[item], item.GetLimit());
            }
            // Handle item being removed
            if (_inventory[item] <= 0) {
                item.OnRemoveItem();
                // Remove item from dictionary if there are none left
                RemoveItem(item);
            }
            // Update inventory and handle popup
            UIManager.instance.UpdateItem(item, quantity, showPopup);
        }
        GameManager.instance.OnInventoryChanged();
    }

    private void AddNewItem(Item item) {
        _inventory.Add(item, 0);
        if (item is NightEffectItem) {
            _nightEffectItems.Add(item);
        }
    }
    
    private void RemoveItem(Item item) {
        _inventory.Remove(item);
        if (item is NightEffectItem) {
            _nightEffectItems.Remove(item);
        }
    }

    public int GetUnlockedSlots() {
        return _currentSlots;
    }

    public void UnlockSlots() {
        _currentSlots += lockedSlots;
        UIManager.instance.SetLockedSlotsStatus(true);
    }

    public void RelockSlots() {
        _currentSlots -= lockedSlots;
        UIManager.instance.SetLockedSlotsStatus(false);
    }

    public bool CanLockSlots() {
        // Note: assumes the inventory expanding item is unique, which it is
        return GetEmptySlots() >= lockedSlots - 1;
    }

    public Dictionary<NightEffectItem, int> GetNightEffectItems() {
        Dictionary<NightEffectItem, int> ret = new Dictionary<NightEffectItem, int>();
        foreach (NightEffectItem item in _nightEffectItems) {
            ret.Add(item, _inventory[item]);
        }
        return ret;
    }
}
