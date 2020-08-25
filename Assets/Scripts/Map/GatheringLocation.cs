using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatheringLocation : Location {
    [Header("Base Gathering Values")]
    [Tooltip("Number of gathering attempts player is given.")]
    public int gatherChances;
    [Tooltip("Number of choices to pick from.")]
    public int gatherChoices;

    protected int _remainingGatherChances;

    [Space]

    public DeckHolder deckHolder;
    private GatheringLocationUI gatheringLocationUI;

    protected override void Start() {
        base.Start();
        gatheringLocationUI = locationCanvas.GetComponent<GatheringLocationUI>();
    }

    public override void PerformActivity() {
        UIManager.instance.tutorials.ShowGatheringTutorial();
        base.PerformActivity();
        BeginGather();
    }

    protected virtual void BeginGather() {
        _remainingGatherChances = gatherChances;
        ProcessGather();
    }

    public void ProcessGather() {
        if (_remainingGatherChances > 0) {
            gatheringLocationUI.Cleanup();
            PresentChoices();
            UIManager.instance.optionsCanvas.ActivateSkipButton(true);
            _remainingGatherChances--;
        } else {
            EndActivity();
            UIManager.instance.optionsCanvas.ActivateSkipButton(false);
        }
    }

    protected virtual int GetChoicesCount() {
        return gatherChoices;
    }

    private void PresentChoices() {
        List<Card> cards = DrawCards();
        gatheringLocationUI.SetCards(cards);
    }

    private List<Card> DrawCards() {
        List<Card> cards = new List<Card>();
        // Copy the deck and throw out duplicates
        Deck tempDeck = deckHolder.deck.GetCopy();
        int choices = GetChoicesCount();
        for (int i = 0; i < choices; i++) {
            cards.Add(tempDeck.DrawCard(false));
        }
        return cards;
    }

    public override void RefreshLocation() {
        gatheringLocationUI.RefreshCardButtons();
    }
}
