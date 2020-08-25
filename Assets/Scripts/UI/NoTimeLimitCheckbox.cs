using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoTimeLimitCheckbox : Checkbox {
    protected override bool DetermineChecked() {
        return GameManager.instance.noTimeLimit;
    }
    
    protected override void ToggleChecked() {
        GameManager.instance.noTimeLimit = !GameManager.instance.noTimeLimit;
    }
}