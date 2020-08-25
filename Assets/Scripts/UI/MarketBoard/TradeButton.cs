using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TradeButton : MonoBehaviour {
    private Shop _shop;
    private Trade _trade;
    public Button button;
    public Transform getItemsList;
    public Transform costItemsList;
    public MarketItemDisplay marketItemDisplayPrefab;
    public GameObject disabledPanel;

    public void SetTrade(Shop shop, Trade trade) {
        _shop = shop;
        _trade = trade;
        foreach (CardEffect effect in trade.get) {
            MarketItemDisplay itemDisplay = Instantiate(marketItemDisplayPrefab, getItemsList);
            itemDisplay.SetEffect(effect, button);
        }
        foreach (CardEffect effect in trade.cost) {
            MarketItemDisplay itemDisplay = Instantiate(marketItemDisplayPrefab, costItemsList);
            itemDisplay.SetEffect(effect, button);
        }
        button.onClick.AddListener(() => trade.OnSelectTrade(shop));
        RefreshAvailability();
    }

    public void RefreshAvailability() {
        if (_trade.CanMakeTrade(_shop)) {
            disabledPanel.SetActive(false);
        } else {
            disabledPanel.SetActive(true);
        }
    }
}
