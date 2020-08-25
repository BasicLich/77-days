using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MarketItemDisplay : MonoBehaviour {
    public InventorySlot inventorySlot;
    public TextMeshProUGUI quantityText;
    
    public void SetEffect(CardEffect cardEffect, Button tradeButton) {
        inventorySlot.SetSlot(cardEffect.item, true);
        quantityText.text = "x" + cardEffect.quantity;
        
        // Make clicking on the inventory slots select the trade
        inventorySlot.GetComponent<Button>().onClick = tradeButton.onClick;
    }

}
