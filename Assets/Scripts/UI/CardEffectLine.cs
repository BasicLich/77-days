using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardEffectLine : MonoBehaviour {
    public TextMeshProUGUI normalText;
    public TextMeshProUGUI quantityText;
    public InventorySlot inventorySlot;

    public void SetEffect(string text) {
        normalText.text = text;
        quantityText.gameObject.SetActive(false);
        inventorySlot.gameObject.SetActive(false);
    }

    public void SetEffect(string text, Item item, CardButton cardButton) {
        quantityText.text = text;
        normalText.gameObject.SetActive(false);
        inventorySlot.SetSlot(item, true);
        // Make clicking on the inventory slots select the card
        inventorySlot.GetComponent<Button>().onClick = cardButton.GetComponent<Button>().onClick;
    }
}
