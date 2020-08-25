using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Food", menuName = "Items/Unique Items/Food")]
public class Food : Item {
    public override string GetDescription() {
        string ret = "you eat " + GameManager.instance.player.playerBackground.foodExpense +
                     " food at the end of each night, restoring 1 hp if you eat enough, or " +
                     "suffering the difference to your health if you do not";
        return ret;
    }
}
