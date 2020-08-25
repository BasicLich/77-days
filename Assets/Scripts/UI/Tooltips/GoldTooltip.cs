using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldTooltip : BasicTooltip {
    protected override void UpdateText() {
        text.text = "gold\n\n" +
                    "certain fees can put you into debt.\n" +
                    "if you accumulate more than 10 debt,\n" +
                    "you will lose the game";
        base.UpdateText();
    }
}
