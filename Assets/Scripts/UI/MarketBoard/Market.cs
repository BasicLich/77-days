using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Market : MonoBehaviour {
    public List<Shop> shops;
    public GameObject workshop;
    public GameObject blackMarket;
    private bool _marketOpen;
    public RectTransform scrollingContainer;
    public ScrollRect scrollRect;

    public void OpenMarket(bool open) {
        _marketOpen = open;
        RefreshMarket();
    }

    public void ScrollToTop() {
        scrollRect.normalizedPosition = new Vector2(0, 1);
    }

    public void RefreshMarket() {
        foreach(Shop shop in shops) {
            shop.UpdateShop();
        }
        UIManager.instance.SetRemoveItemText(PlayerCanSell());

        // TODO: ineffective. change argument?
        // UIManager.Rebuild(scrollingContainer);
    }

    public bool PlayerCanSell() {
        return _marketOpen && GameManager.instance.player.inventory.HasItem(GameManager.instance.vendingLicence);
    }

    public void TrySellItem(Item item, int quantity) {
        if (PlayerCanSell() && item.CanBeDisposed()) {
            int sellable = item.GetMaxDisposable(quantity);
            if (sellable > 0) {
                // TODO: sell sound effect
            }
            GameManager.instance.player.inventory.AddItem(item, -sellable);
            GameManager.instance.player.inventory.AddGold(sellable * item.sellValue);
        }
    }
}
