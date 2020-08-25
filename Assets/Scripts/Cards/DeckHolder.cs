using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDeck", menuName = "Deck", order = 0)]
public class DeckHolder : ScriptableObject {
    public Deck deck;

    // Recount deck
    private void Awake() {
        deck.SetDeckSize();
    }
}
