using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Cauldron", menuName = "Items/Unique Items/Cauldron")]
public class Cauldron : UsableItem {
    public CardEffectsSet totalEffect;
    public CardEffectsSet rewardEffects;
    public CardEffectsSet costEffects;
    [Range(0, 1)] public float successChance;

    public override void TryUse(int quantity) {
        if (GameManager.instance.player.CanApplyEffects(totalEffect)) {
            int usable = GetMaxUsable(quantity);
            Use(usable);
        }
    }
    
    protected override void ApplyEffect(int quantity) {
        int successes = 0;
        for (int i = 0; i < quantity; i++) {
            if (Random.value <= successChance) {
                successes++;
            }
        }
        rewardEffects.ApplyEffects(true, successes);
        costEffects.ApplyEffects(true, quantity);
    }
}
