using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hut : Location {
    public Item boat;
    public PlayerBackground merpersonBackground;
    private bool visitedHut;

    public override bool DestinationAvailable() {
        if (visitedHut) {
            return false;
        }
        Player player = GameManager.instance.player;
        if (player.inventory.HasItem(boat) || player.playerBackground.backgroundType == BackgroundType.MERE) {
            return true;
        } else {
            return false;
        }
    }
    
    public override void PerformActivity() {
        base.PerformActivity();
        visitedHut = true;
    }

    public override bool CanRemain() {
        return false;
    }
}
