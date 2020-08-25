using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Magic Missile", menuName = "Items/Unique Items/Magic Missile")]
public class MagicMissile : UsableItem {
    protected override void ApplyEffect(int quantity) {
        GameManager.instance.EndGame(GameEndType.MAGIC_MISSILE);

    }
}
