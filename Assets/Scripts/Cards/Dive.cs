using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * See the Fish card.
 */
[CreateAssetMenu(fileName = "Dive", menuName = "Cards/Unique Cards/Dive")]
public class Dive : RandomEffectsCard {
    public Item spear;
    public Item boat;

    public override void SelectCard() {
        foreach (CardEffect effect in cardEffectsSet.cardEffects) {
            if (effect.item == GameManager.instance.foodItem) {
                GameManager.instance.player.inventory.AddItem(effect.item, ApplyFoodBonuses(effect.quantity));
            } else {
                GameManager.instance.player.inventory.AddItem(effect.item, effect.quantity);
            }
        }
        GameManager.instance.map.currLocation.GetComponent<GatheringLocation>().ProcessGather();
    }

    private int ApplyFoodBonuses(int baseQuantity) {
        int foodQuantity = baseQuantity;
        if (GameManager.instance.player.inventory.HasItem(spear)) {
            foodQuantity += spear.effectPower;
        }
        if (GameManager.instance.player.inventory.HasItem(boat)) {
            foodQuantity += boat.effectPower;
        }
        return foodQuantity;
    }
    
    public override void SetEffectsSummary(CardEffectsSummary summary, CardButton cardButton) {
        foreach (CardEffect effect in cardEffectsSet.cardEffects) {
            if (effect.quantity != 0) {
                if (effect.item == GameManager.instance.foodItem) {
                    summary.AddEffect(ApplyFoodBonuses(effect.quantity), effect.item, cardButton);
                } else {
                    summary.AddEffect(effect.quantity, effect.item, cardButton);
                }
            }
        }
    }
}
