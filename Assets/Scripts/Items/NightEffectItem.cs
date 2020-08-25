using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FailNightEffectBehaviour {
    NO_EFFECT,
    CANNOT_FAIL,
    DISCARD
}

[CreateAssetMenu(fileName = "NewNightEffectItem", menuName = "Items/Night Effect Item")]
public class NightEffectItem : Item {
    [Tooltip("The effects imposed at the end of night.")]
    public CardEffectsSet cardEffectsSet;
    [Tooltip("Determines what happens if the night effect cannot happen, whether it be because the player can't afford it or they don't have the space.")]
    public FailNightEffectBehaviour behaviourType;

    // Tries to perform this item's night effect with the given quantity of this item and returns true if successful
    public void TryPerformNightEffect() {
        if (GameManager.instance.player.CanPayCost(cardEffectsSet.GetCost())) {
            if (GameManager.instance.player.CanApplyEffects(cardEffectsSet)) {
                PerformNightEffect();
            }
        } else {
            OnFailEffect();
        }
    }

    public void PerformNightEffect() {
        cardEffectsSet.ApplyEffects();
    }

    /*
     * Handles behaviour if the night effect fails based on behaviourType.
     * NO_EFFECT makes the item do nothing.
     * CANNOT_FAIL tries to apply the effect anyway.
     * DISCARD makes the item disappear.
     */
    private void OnFailEffect() {
        switch (behaviourType) {
            case (FailNightEffectBehaviour.NO_EFFECT):
                break;
            case (FailNightEffectBehaviour.CANNOT_FAIL):
                PerformNightEffect();
                break;
            case (FailNightEffectBehaviour.DISCARD):
                GameManager.instance.player.inventory.AddItem(this, -1);
                break;
        }

    }
}
