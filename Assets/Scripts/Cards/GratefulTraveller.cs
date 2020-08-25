using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Grateful Traveller", menuName = "Cards/Unique Cards/Grateful Traveller")]
public class GratefulTraveller : Card {
    public override void SelectCard() {
        base.SelectCard();
        GameManager.instance.FLAG_GRATEFUL_TRAVELLER_SELECTED = true;
    }

    public override bool CanDraw() {
        if (!base.CanDraw()) {
            return false;
        }
        return GameManager.instance.FLAG_HUNGRY_TRAVELLER_FED && !GameManager.instance.FLAG_GRATEFUL_TRAVELLER_SELECTED;
    }
}

