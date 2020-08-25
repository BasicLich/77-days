using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class InventorySlot : MonoBehaviour, IPointerClickHandler {
    [HideInInspector] public Item item;
    public Image backgroundImage;
    public Image icon;
    public TextMeshProUGUI quantity;
    public ItemTooltip itemTooltip;
    // Marks an item as one not in the inventory, for use in the cards and market
    private bool _displayOnly;

    public void SetSlot(Item item, bool displayOnly = false) {
        this.item = item;
        icon.sprite = item.icon;
        // Set visible
        icon.color = Color.white;
        if (_displayOnly || item.IsUnique()) {
            quantity.text = "";
        } else {
            quantity.text = GameManager.instance.player.inventory.GetQuantity(item).ToString();
        }
        if (item.resourceItem) {
            backgroundImage.enabled = false;
            icon.SetNativeSize();
        } else {
            backgroundImage.enabled = true;
        }
        _displayOnly = displayOnly;
    }
    
    public void Empty() {
        item = null;

        // Set invisible
        Color invis = icon.color;
        invis.a = 0;
        icon.color = invis;
        
        quantity.text = "";
    }

    public bool IsEmpty() {
        return item == null;
    }

    public void UseItem(bool shiftDown) {
        if (item != null && !_displayOnly) {
            int quantity = shiftDown ? Constants.instance.shiftClickQuantity : 1;
            item.TryUse(quantity);
        }
        if (IsEmpty()) {
            itemTooltip.gameObject.SetActive(false);
        }
    }

    public void DisposeItem(bool shiftDown) {
        if (item != null && !_displayOnly) {
            int quantity = shiftDown ? Constants.instance.shiftClickQuantity : 1;
            if (GameManager.instance.market.PlayerCanSell()) {
                GameManager.instance.market.TrySellItem(item, quantity);
            } else {
                // TODO: throw away sfx
                GameManager.instance.player.inventory.AddItem(item, -quantity);
            }
            if (IsEmpty()) {
                itemTooltip.gameObject.SetActive(false);
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData) {
        bool ctrlDown = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);
        if (eventData.button == PointerEventData.InputButton.Left) {
            UseItem(ctrlDown);
        } else if (eventData.button == PointerEventData.InputButton.Right) {
            DisposeItem(ctrlDown);
        }
    }
    
    public void OnMouseover() {
        if (!IsEmpty()) {
            itemTooltip.gameObject.SetActive(true);
            itemTooltip.SetTooltip(item);
        }
    }

    public void StopMouseover() {
        itemTooltip.gameObject.SetActive(false);
    }
}
