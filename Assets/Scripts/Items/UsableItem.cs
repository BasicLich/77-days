using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsableItem : Item {
    public bool consumable;

    public override void TryUse(int quantity) {
        int usable = GetMaxUsable(quantity);
        Use(usable);
    }

    protected void Use(int quantity) {
        if (consumable) {
            GameManager.instance.player.inventory.AddItem(this, -quantity);
        }
        ApplyEffect(quantity);
        GameManager.instance.map.currLocation.RefreshLocation();
    }
    
    protected virtual void ApplyEffect(int quantity) {

    }

    // Returns quantity, if that many is usable, or the greatest quantity usable otherwise
    protected virtual int GetMaxUsable(int useQuantity) {
        if (consumable) {
            return GetMaxDisposable(useQuantity);
        }
        return useQuantity;
    }
}
