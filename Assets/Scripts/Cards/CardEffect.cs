using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CardEffect {
    public Item item;
    public int quantity;
    
    public CardEffect(Item item, int quantity) {
        this.item = item;
        this.quantity = quantity;
    }
}
