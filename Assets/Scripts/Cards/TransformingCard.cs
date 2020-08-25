using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTransformingCard", menuName = "Cards/Transforming Card")]
public class TransformingCard : Card {
    public Card transformToCard;
    [Tooltip("Item that will cause this card to change if it is in the player's inventory.")]
    public Item hasItem;

    public bool ConditionMet() {
        if (hasItem != null && GameManager.instance.player.inventory.HasItem(hasItem)) {
            return true;
        }
        return false;
    }

    public virtual Card ApplyTransformation() {
        if (ConditionMet()) {
            return transformToCard;
        }
        return this;
    }
}
