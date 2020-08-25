using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour {
    public string shopName;
    [Tooltip("The item the player needs to have in their inventory in order to buy from this shop, if there is one.")]
    public Item requiredItem;
    public List<Trade> trades;
    public TradeButton tradeButtonPrefab;
    public RectTransform tradeButtonsList;
    private List<TradeButton> tradeButtons = new List<TradeButton>();

    private void Start() {
        foreach (Trade trade in trades) {
            TradeButton tradeButton = Instantiate(tradeButtonPrefab, tradeButtonsList);
            tradeButton.SetTrade(this, trade);
            tradeButtons.Add(tradeButton);
        }
        UIManager.Rebuild(tradeButtonsList);
    }

    public bool CanAccess() {
        if (requiredItem == null) {
            return true;
        }
        return GameManager.instance.player.inventory.HasItem(requiredItem);
    }

    public void UpdateShop() {
        if (CanAccess()) {
            gameObject.SetActive(true);
        }
        foreach (TradeButton button in tradeButtons) {
            button.RefreshAvailability();
        }
    }
}
