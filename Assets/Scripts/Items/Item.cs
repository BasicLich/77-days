using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Items/Item")]
public class Item : ScriptableObject {
    public string itemName;
    public Sprite icon;
    public int sellValue;
    [Tooltip("Attack power of this item. Setting this above 0 will mark this item as a weapon. Note: currently, this is unused.")]
    public int weaponPower;
    [TextArea] public string description;
    [Tooltip("Effect value used for certain cards with special effects.")]
    public int effectPower;
    [Tooltip("Set to true to mark this as special case of item that doesn't appear in the inventory (i.e. gold and health). Resource items have no border inside cards and have special tooltips.")]
    public bool resourceItem;
    [Tooltip("Set to false to disable disposing of this item (discarding or selling).")]
    public bool disposable = true;
    [Tooltip("Max quantity that player can have for this item. If limit is reached, this item cannot be obtained again. Leave at 0 for no limit. Set to 1 to make this unique. A unique item's quantity is not displayed in the inventory.")]
    public int quantityLimit;

    public virtual void TryUse(int quantity) {

    }

    private bool IsWeapon() {
        return weaponPower > 0;
    }

    public bool HasLimit() {
        return quantityLimit > 0;
    }

    public int GetLimit() {
        if (quantityLimit == 0) {
            Debug.LogWarning("Used GetLimit() on an unlimited quantity item.");
        }
        return quantityLimit;
    }

    public bool IsUnique() {
        return quantityLimit == 1;
    }


    public string BuildTooltipDescription() {
        string ret = "";
        /*
        if (IsWeapon()) {
            ret += weaponPower + " atk";
            if (description != "") {
                ret += "\n";
            }
        }
        */
        if (IsUnique()) {
            ret += "limit one";
            if (GetDescription() != "") {
                ret += "\n\n";
            }
        }
        ret += GetDescription();
        return ret;
    }

    public virtual string GetDescription() {
        return description;
    }

    public virtual bool CanAddNewItem() {
        return !GameManager.instance.player.inventory.IsFull();
    }

    public virtual void OnAddItem(int quantity) {
        
    }

    public virtual void OnRemoveItem() {

    }

    public virtual bool CanBeDisposed() {
        return disposable;
    }
    
    // Returns quantity, if that many is disposable (i.e. usable/sellable/discardable), or the greatest disposable quantity otherwise
    public int GetMaxDisposable(int disposeQuantity) {
        int inventoryQuantity = GameManager.instance.player.inventory.GetQuantity(this);
        if (inventoryQuantity < disposeQuantity) {
            return inventoryQuantity;
        } else {
            return disposeQuantity;
        }
    }

    public virtual bool HasNightEffect() {
        return false;
    }
}
