using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardEffectsSummary : MonoBehaviour {
    public CardEffectLine cardEffectLinePrefab;

    public void AddEffect(string effect) {
        CardEffectLine line = Instantiate(cardEffectLinePrefab, transform);
        line.SetEffect(effect);
    }

    public void AddEffect(int quantity, string effect) {
        if (quantity == 0) {
            Debug.LogWarning("Tried to add an effect with 0 quantity");
            return;
        }
        effect = (quantity > 0 ? "+" : "") + quantity + effect;
        AddEffect(effect);
    }

    public void AddEffect(int quantity, Item item, CardButton cardButton) {
        if (quantity == 0) {
            Debug.LogWarning("Tried to add an effect with 0 quantity");
            return;
        }
        string text = "";
        text += (quantity > 0 ? "+" : "") + quantity;
        CardEffectLine line = Instantiate(cardEffectLinePrefab, transform);
        line.SetEffect(text, item, cardButton);
    }
}
