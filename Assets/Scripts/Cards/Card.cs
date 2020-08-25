using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCard", menuName = "Cards/Card Base", order = 1)]
public class Card : ScriptableObject {
    [Header("Card Info")]
    public string cardName;
    public Sprite icon;
    public CardEffectsSet cardEffectsSet;
    [Tooltip("BackgroundType required to draw this card.")]
    public BackgroundType requiredBackground;
    [Tooltip("Item that prevents this item from being drawn.")]
    public Item purgeItem;
    
    // Called when the deck tries to draw this card but before validating it
    public virtual void OnPeek() {

    }
    
    // Called when this card is put into the gathering choices
    public virtual void OnDraw() {

    }
    
    
    // Returns whether or not this card can be selected by the player
    public virtual bool CanSelect() {
        if (GameManager.instance.debug.canSelectAll) {
            return true;
        }
        return GameManager.instance.player.CanApplyEffects(cardEffectsSet);
    }

    public bool TrySelect() {
        if (CanSelect()) {
            SelectCard();
            return true;
        }
        return false;
    }

    public virtual void SelectCard() {
        cardEffectsSet.ApplyEffects();
        GameManager.instance.map.currLocation.GetComponent<GatheringLocation>().ProcessGather();
    }

    // Returns whether or not this card is available to be drawn
    public virtual bool CanDraw() {
        List<CardEffect> get = cardEffectsSet.GetReward();
        foreach (CardEffect effect in get) {
            Item item = effect.item;
            int inventoryQuantity = GameManager.instance.player.inventory.GetQuantity(item);
            // Disallow cards that have reached the item limit
            if (inventoryQuantity > 0 && item.HasLimit()) {
                if (inventoryQuantity >= item.GetLimit()) {
                    return false;
                }
            }
        }
        if (purgeItem != null && GameManager.instance.player.inventory.HasItem(purgeItem)) {
            return false;
        }
        if (requiredBackground != BackgroundType.NONE && requiredBackground != GameManager.instance.player.playerBackground.backgroundType) {
            return false;
        }
        return true;
    }

    public virtual void SetEffectsSummary(CardEffectsSummary summary, CardButton cardButton) {
        foreach (CardEffect effect in cardEffectsSet.cardEffects) {
            if (effect.quantity != 0) {
                summary.AddEffect(effect.quantity, effect.item, cardButton);
            }
        }
    }
}
