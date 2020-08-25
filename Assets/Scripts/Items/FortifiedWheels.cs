using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FortifiedWheels", menuName = "Items/Unique Items/Fortified Wheels")]
public class FortifiedWheels : Item {
    public override bool CanBeDisposed() {
        // TODO: temporary band-aid: can't sell this item
        // return GameManager.instance.player.inventory.CanLockSlots();
        return disposable;
    }

    public override bool CanAddNewItem() {
        // Can acquire this item even if inventory is full
        return true;
    }

    public override void OnAddItem(int quantity) {
        GameManager.instance.player.inventory.UnlockSlots();
    }

    public override void OnRemoveItem() {
        GameManager.instance.player.inventory.RelockSlots();
    }
}
