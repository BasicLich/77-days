using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Elixir", menuName = "Items/Unique Items/Elixir")]
public class Elixir : UsableItem {
    protected override void ApplyEffect(int quantity) {
        GameManager.instance.player.AddMaxHealth(effectPower * quantity);
        GameManager.instance.player.AddHealth(effectPower * quantity);
    }
}
