using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BackgroundType {
    NONE,
    HUE,
    MERE,
    RENEGADE
}

[CreateAssetMenu(fileName = "NewBackground", menuName = "Player Background")]
public class PlayerBackground : ScriptableObject {
    public BackgroundType backgroundType;
    public Sprite sprite;
    public CardEffectsSet startingItems;
    public int foodExpense;
    public int maxHealth;
    [TextArea(10, 10), Tooltip("Note start items, food expense, extra abilities here.")]
    public string description;

    public void ApplyBackground(Player player) {
        player.playerBackground = this;
        startingItems.ApplyEffects(false);
        player.SetSprite(sprite);
        player.SetMaxHealth(maxHealth);
        player.SetHealth(maxHealth);
        UIManager.instance.UpdateGoldDisplay(player.inventory.GetGold());
        UIManager.instance.UpdateBackgroundDisplay();
    }

    /*
    public string GetDescription() {
        string ret = "";
        ret += maxHealth + " health\n";
        if (foodExpense != 0) {
            ret += "eats " + foodExpense + " food each night\n";
        } else {
            ret += "does not eat food\n";
        }
        ret += "starts with " + startingItems.GetQuantity(GameManager.instance.goldItem) + " gold";
        ret += "\n" + extraTextDescription;
        return ret;
    }
    */
}
