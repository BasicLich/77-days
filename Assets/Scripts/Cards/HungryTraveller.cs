using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Card that enables Grateful Traveller. Can only be seen once (even if this card is drawn but not selected).
 */
[CreateAssetMenu(fileName = "Hungry Traveller", menuName = "Cards/Unique Cards/Hungry Traveller")]
public class HungryTraveller : Card {
    public override void OnDraw() {
        GameManager.instance.FLAG_HUNGRY_TRAVELLER_SEEN = true;
    }

    public override void SelectCard() {
        base.SelectCard();
        GameManager.instance.FLAG_HUNGRY_TRAVELLER_FED = true;
    }

    public override bool CanDraw() {
        if (!base.CanDraw()) {
            return false;
        }
        return !GameManager.instance.FLAG_HUNGRY_TRAVELLER_SEEN;
    }
}
