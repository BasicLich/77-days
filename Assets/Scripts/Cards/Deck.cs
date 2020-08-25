using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Deck {
    public List<CardCopies> composition;
    private bool _deckCounted;
    private int _deckSize;

    public Deck (List<CardCopies> composition, int deckSize) {
        this.composition = composition;
        _deckSize = deckSize;
    }

    public Deck GetCopy() {
        _deckSize = GetDeckSize();
        return new Deck(new List<CardCopies>(composition), _deckSize);
    }

    // Returns a random card from this deck. Set allowDuplicates to false to throw out all cards of the same type after drawing.
    public Card DrawCard(bool allowDuplicates = true) {
        if (!_deckCounted) {
            SetDeckSize();
        }
        CardCopies cardCopies = GetValidCard();
        if (!allowDuplicates) {
            ThrowOutCardCopies(cardCopies);
        }
        return cardCopies.card;
    }

    private void ThrowOutCardCopies(CardCopies cardCopies) {
        _deckSize -= cardCopies.copies;
        composition.Remove(cardCopies);
    }

    public void SetDeckSize() {
        _deckSize = GetDeckSize();
        _deckCounted = true;
    }

    private int GetDeckSize() {
        int count = 0;
        foreach (CardCopies cardCopies in composition) {
            count += cardCopies.copies;
        }
        return count;
    }

    private CardCopies GetValidCard() {
        CardCopies cardCopies = GetCard();
        int attempts = 1;
        int maxAttempts = Constants.instance.loopLimit;
        while (!cardCopies.card.CanDraw() && attempts < maxAttempts) {
            cardCopies = GetCard();
            attempts++;
            // Don't throw card out -- if it's a random effect card, its effects may be different on a redraw
            /*
            if (!cardCopies.card.CanDraw()) {
                ThrowOutCardCopies(cardCopies);
            }
            */
        }
        if (attempts == maxAttempts) {
            Debug.LogWarning("Max attempts reached while trying to draw a card.");
        }
        cardCopies.card.OnDraw();
        return cardCopies;
    }

    private CardCopies GetCard() {
        int rand = Random.Range(0, _deckSize);
        CardCopies cardCopies = GetCardCopiesFromIndex(rand);
        if (cardCopies.card is TransformingCard) {
            cardCopies = new CardCopies(((TransformingCard) cardCopies.card).ApplyTransformation(), cardCopies.copies);
        }
        cardCopies.card.OnPeek();
        return cardCopies;
    }

    private CardCopies GetCardCopiesFromIndex(int i) {
        int currIndex = 0;
        foreach (CardCopies cardCopies in composition) {
            currIndex += cardCopies.copies;
            if (i < currIndex) {
                return cardCopies;
            }
        }
        Debug.LogWarning("Failed to GetCardFromIndex: bad index " + i);
        return null;
    }
}
