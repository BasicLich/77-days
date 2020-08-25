using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTrade", menuName = "Trade")]
public class Trade : ScriptableObject {
    // The items you get from this trade
    public List<CardEffect> get;
    // The items you give up for this trade
    public List<CardEffect> cost;

    public bool CanMakeTrade(Shop shop) {
        if (GameManager.instance.debug.canBuyAll) {
            return true;
        }
        return shop.CanAccess() && GameManager.instance.player.CanApplyEffects(get, cost);
    }


    public void OnSelectTrade(Shop shop) {
        bool ctrlDown = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);
        int trades = ctrlDown ? 10 : 1;
        GameManager.instance.StartCoroutine(TryMakeTrades(shop, trades));
    }

    public void TryMakeTrade(Shop shop) {
        if (shop.CanAccess() && GameManager.instance.gameStatus == GameStatus.PLAYING) {
            if (CanMakeTrade(shop)) {
                MakeTrade();
            } else {
                Player player = GameManager.instance.player;

                // Show failure popup
                if (!shop.CanAccess()) {
                    UIManager.instance.ShowTextPopup(string.Format(Constants.NEED_ITEM, shop.requiredItem.itemName), Input.mousePosition);
                } else if (!player.CanPayCost(cost)) {
                    UIManager.instance.ShowTextPopup(Constants.CANNOT_AFFORD, Input.mousePosition);
                } else if (!player.inventory.EffectsWithinLimit(get, cost)) {
                    Item violatingItem = player.inventory.GetItemViolatingLimit(get, cost);
                    if (violatingItem.GetLimit() == 1) {
                        UIManager.instance.ShowTextPopup(Constants.LIMIT_ONE, Input.mousePosition);
                    } else {
                        UIManager.instance.ShowTextPopup(Constants.LIMITED_ITEM, Input.mousePosition);
                    }
                } else if (!player.inventory.HasSpaceForItems(get, cost)) {
                    UIManager.instance.ShowTextPopup(Constants.NO_INV_SPACE, Input.mousePosition);
                }
            }

        }
    }

    public IEnumerator TryMakeTrades(Shop shop, int quantity) {
        int remaining = quantity;
        while (remaining > 0) {
            TryMakeTrade(shop);
            remaining--;
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void MakeTrade() {
        foreach (CardEffect effect in cost) {
            GameManager.instance.player.inventory.AddItem(effect.item, -effect.quantity);
        }
        foreach (CardEffect effect in get) {
            GameManager.instance.player.inventory.AddItem(effect.item, effect.quantity);
        }
        GameManager.instance.market.RefreshMarket();
    }
}
