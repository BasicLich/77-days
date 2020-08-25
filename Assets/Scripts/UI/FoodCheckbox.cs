using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FoodCheckbox : Checkbox {
    protected override bool DetermineChecked() {
        return GameManager.instance.eatAtNight;
    }
    
    protected override void ToggleChecked() {
        GameManager.instance.eatAtNight = !GameManager.instance.eatAtNight;
    }
}
