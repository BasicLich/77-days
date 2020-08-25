using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class CardEffectsSet {
    public List<CardEffect> cardEffects;

    public CardEffectsSet(List<CardEffect> cardEffects) {
        this.cardEffects = cardEffects;
    }
    
    /*
    public CardEffectsSet(List<CardEffect> get, List<CardEffect> cost) {
        this.cardEffects = get;
        foreach (CardEffect effect in cost) {
            cardEffects.Add(new CardEffect(effect.item, -effect.quantity));
        }
    }
    */
    
    // Returns the CardEffects with positive quantity
    public List<CardEffect> GetReward() {
        return cardEffects.Where(effect => effect.quantity > 0).ToList();
    }

    // Returns the CardEffects with negative quantity. If setPositive is true, the effects will be made positive.
    public List<CardEffect> GetCost(bool setPositive = true) {
        List<CardEffect> cost = new List<CardEffect>(cardEffects.Where(effect => effect.quantity < 0).ToList());
        if (setPositive) {
            // Make deep copy of cost to not affect original quantities
            for (int i = 0; i < cost.Count; i++) {
                cost[i] = new CardEffect(cost[i].item, -cost[i].quantity);
            }
        }
        return cost;
    }

    public void ApplyEffects(bool showPopup = true, int multiplier = 1) {
        foreach (CardEffect effect in cardEffects) {
            GameManager.instance.player.inventory.AddItem(effect.item, effect.quantity * multiplier, showPopup);
        }
    }

    /*
    public int GetQuantity(Item item) {
        int quantity = 0;
        foreach (CardEffect effect in cardEffects) {
            if (effect.item == item) {
                quantity += effect.quantity;
            }
        }
        return quantity;
    }
    */
}
