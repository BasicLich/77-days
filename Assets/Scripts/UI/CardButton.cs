using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardButton : MonoBehaviour {
    private Card _card;
    public Image icon;
    public TextMeshProUGUI cardName;
    public CardEffectsSummary effectsSummary;
    public Button button;
    public GameObject disabledPanel;

    public void SetCard(Card card) {
        _card = card;
        cardName.text = card.cardName;
        icon.sprite = card.icon;
        button.onClick.AddListener(() => card.TrySelect());
        SetAvailability();
        card.SetEffectsSummary(effectsSummary, this);
    }

    public void SetAvailability() {
        if (_card.CanSelect()) {
            disabledPanel.SetActive(false);
        } else {
            disabledPanel.SetActive(true);
        }
    }
}
