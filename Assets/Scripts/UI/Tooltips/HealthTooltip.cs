using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthTooltip : BasicTooltip {
    protected override void UpdateText() {
        text.text = "health\n\nif this reaches 0, you\nwill lose the game";
        base.UpdateText();
    }
}
