using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDialogue", menuName = "Dialogue/Day Injection Dialogue")]
public class InjectableDialogue : Dialogue {
    public override string GetDialogue() {
        int daysLeft = GameManager.instance.finalDay - GameManager.instance.day;
        return string.Format(dialogue, Utils.GetFormattedDays(daysLeft));
    }
}
