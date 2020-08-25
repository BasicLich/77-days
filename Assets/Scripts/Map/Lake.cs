using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lake : GatheringLocation {
    protected override void BeginGather() {
        _remainingGatherChances = gatherChances;
        /*
        // Increase gathering chances if player is Mere
        if (GameManager.instance.player.playerBackground.backgroundType == BackgroundType.MERE) {
            _remainingGatherChances++;
        }
        */
        ProcessGather();
    }

    protected override int GetChoicesCount() {
        int choices = gatherChoices;
        if (GameManager.instance.player.playerBackground.backgroundType == BackgroundType.MERE) {
            choices++;
        }
        return choices;
    }
}
