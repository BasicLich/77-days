using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Town : Location {
    public override void PerformActivity() {
        base.PerformActivity();
        UIManager.instance.tutorials.ShowTownTutorial();
        GameManager.instance.market.OpenMarket(true);
    }

    public override void EndActivity() {
        base.EndActivity();
        GameManager.instance.market.OpenMarket(false);
        UIManager.instance.SetRemoveItemText(false);
    }

    public override void RefreshLocation() {
        GameManager.instance.market.RefreshMarket();
    }
}
