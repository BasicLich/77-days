using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatheringLocationUI : MonoBehaviour {
    public Transform cardDisplay;
    public CardButton cardButtonPrefab;
    private List<CardButton> currCardButtons = new List<CardButton>();

    public void SetCards(List<Card> cards) {
        // TODO: frosty optimization: reuse existing cards
        foreach (Card card in cards) {
            CardButton cardButton = Instantiate(cardButtonPrefab, cardDisplay);
            cardButton.SetCard(card);
            currCardButtons.Add(cardButton);
        }
    }

    public void RefreshCardButtons() {
        foreach (CardButton cardButton in currCardButtons) {
            cardButton.SetAvailability();
        }
    }

    public void Cleanup() {
        for (int i = currCardButtons.Count - 1; i >= 0; i--) {
            Destroy(currCardButtons[i].gameObject);
        }
        currCardButtons.Clear();
    }
}
