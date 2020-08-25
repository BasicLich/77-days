using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Grumpy Pixie", menuName = "Cards/Unique Cards/Grumpy Pixie")]
public class GrumpyPixie : Card {
    public override void SelectCard() {
        cardEffectsSet.ApplyEffects();
        GameManager.instance.map.currLocation.GetComponent<GatheringLocation>().OpenCanvas(false);
        GameManager.instance.map.TeleportToRandomLocation();
        UIManager.instance.optionsCanvas.ActivateSkipButton(false);
        GameManager.instance.StartNight();
    }

    public override void SetEffectsSummary(CardEffectsSummary summary, CardButton cardButton) {
        base.SetEffectsSummary(summary, cardButton);
        summary.AddEffect("teleports you to a random location");
    }
}
