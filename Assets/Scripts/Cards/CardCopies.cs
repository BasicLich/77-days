using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CardCopies {
    public Card card;
    public int copies;

    public CardCopies (Card card, int copies) {
        this.card = card;
        this.copies = copies;
    }
}
