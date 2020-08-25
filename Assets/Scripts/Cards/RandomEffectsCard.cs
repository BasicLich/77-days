using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewRandomEffectsCard", menuName = "Cards/Random Effects Card")]
public class RandomEffectsCard : Card {
    [Tooltip("List of potential sets of card effects.")]
    public List<CardEffectsSet> cardEffectsSets;

    public override void OnPeek() {
        cardEffectsSet = cardEffectsSets[Random.Range(0, cardEffectsSets.Count)];
    }
}
