using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Repair Ship", menuName = "Cards/Unique Cards/Repair Ship")]
public class RepairShip : Card {
    [Header("Repair Ship Values")]
    [Tooltip("Set to true if this card repairs. False means this card dismantles.")]
    public bool repair;
    
    // Override to not disable the final repair card after selection
    public override bool CanSelect() {
        if (base.CanSelect()) {
            return true;
        }
        return GameManager.instance.ShipFixed();
    }
    
    public override void SelectCard() {
        GameManager.instance.AddShipIntegrity(repair);
        cardEffectsSet.ApplyEffects();
        if (GameManager.instance.ShipFixed()) {
            GameManager.instance.EndGame(GameEndType.SHIP_ESCAPE);
        } else {
            GameManager.instance.map.currLocation.GetComponent<GatheringLocation>().ProcessGather();
        }
    }

    public override bool CanDraw() {
        if (!base.CanDraw()) {
            return false;
        }
        int integrity = GameManager.instance.shipIntegrity;
        // Disallow repairing past 100 and dismantling below 0
        if (integrity == 100 && repair) {
            return false;
        }
        if (integrity < GameManager.instance.shipRepairDecrement && !repair) {
            return false;
        }
        return true;
    }
    
    public override void SetEffectsSummary(CardEffectsSummary summary, CardButton cardButton) {
        base.SetEffectsSummary(summary, cardButton);
        int currIntegrity = GameManager.instance.shipIntegrity;
        int newIntegrity = GameManager.instance.CalculateShipIntegrityChange(repair);
        if (repair) {
            summary.AddEffect("increases ship integrity from " + currIntegrity + "% to " + newIntegrity + "%");
        } else {
            summary.AddEffect("decreases ship integrity from " + currIntegrity + "% to " + newIntegrity + "%");
        }
    }
}
