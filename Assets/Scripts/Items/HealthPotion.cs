using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Health Potion", menuName = "Items/Unique Items/Health Potion")]
public class HealthPotion : UsableItem {
    protected override void ApplyEffect(int quantity) {
        GameManager.instance.player.AddHealth(effectPower * quantity);
    }
    
    protected override int GetMaxUsable(int useQuantity) {
        int inventoryQuantity = base.GetMaxUsable(useQuantity);
        int missingHealth = GameManager.instance.player.GetMissingHealth();
        int usablePotions = (int) Mathf.Ceil((float) missingHealth / effectPower);
        return Mathf.Min(inventoryQuantity, usablePotions);
    }
}
