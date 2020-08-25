using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundTooltip : BasicTooltip {
    public PlayerBackground background;

    protected override void UpdateText() {
        if (background != null) {
            text.text = background.description;
            base.UpdateText();
        }
    }
}