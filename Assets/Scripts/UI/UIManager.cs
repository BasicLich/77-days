using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : Singleton<UIManager> {
    [SerializeField] private GameObject _mainMenuCanvas = null;
    public OptionsCanvas optionsCanvas;
    public Tutorials tutorials;
    [SerializeField] private BackgroundProfileButton _backgroundDisplay = null;
    [SerializeField] private TextMeshProUGUI _dayDisplay = null;
    [SerializeField] private TextMeshProUGUI _goldDisplay = null;
    [SerializeField] private Color _debtTextColour = Color.white;
    [SerializeField] private Transform _goldIcon = null;
    [SerializeField] private TextMeshProUGUI _healthNumber = null;
    [SerializeField] private Transform _heartIcon = null;
    [SerializeField] private Image _healthContent = null;
    [SerializeField] private InventoryUI _inventoryUI = null;
    [SerializeField] private TextMeshProUGUI _removeItemGuidance = null;
    [SerializeField] private ItemChangePopup _itemPopupPrefab = null;
    [SerializeField] private GameEndScreen _gameEndScreen = null;

    public static void Rebuild(RectTransform rectTransform) {
        LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
        Canvas.ForceUpdateCanvases();
    }

    public void SetMainMenu(bool active) {
        _mainMenuCanvas.SetActive(active);
    }

    public void UpdateBackgroundDisplay() {
        _backgroundDisplay.SetBackground(GameManager.instance.player.playerBackground);
    }

    public void UpdateDayDisplay(int dayNumber) {
        _dayDisplay.text = "day " + dayNumber.ToString();
    }

    public void UpdateGoldDisplay(int quantity) {
        _goldDisplay.text = quantity.ToString();
        if (quantity >= 0) {
            _goldDisplay.color = Color.white;
        } else {
            _goldDisplay.color = _debtTextColour;
        }
    }

    public void UpdateHealthDisplay(int health) {
        int maxHealth = GameManager.instance.player.GetMaxHealth();
        _healthContent.fillAmount = (float) health / maxHealth;
        _healthNumber.text = health + "/" + maxHealth;
    }

    public void SetNighttimeOptions(bool active) {
        optionsCanvas.ActivateFoodCheckbox(active);
        if (active && !GameManager.instance.map.currLocation.CanRemain()) {
            // Disallow remaining in the current location if necessary
            optionsCanvas.ActivateRemainButton(false);
            return;
        }
        optionsCanvas.ActivateRemainButton(active);
    }

    public void UpdateItem(Item item, int value, bool showPopup) {
        InventorySlot slot = _inventoryUI.UpdateItem(item);
        if (showPopup) {
            ShowItemPopup(value, slot.transform.position);
        }
    }

    public void ShowTextPopup(string text, Vector3 location) {
        ItemChangePopup popup = Instantiate(_itemPopupPrefab);
        popup.Initialize(text, location);
    }

    private void ShowValueChangePopup(int value, Vector3 location) {
        ItemChangePopup popup = Instantiate(_itemPopupPrefab);
        popup.Initialize(value, location);
    }

    public void ShowGoldPopup(int value) {
        ShowValueChangePopup(value, _goldIcon.position);
    }

    public void ShowHealthPopup(int value) {
        ShowValueChangePopup(value, _heartIcon.position);
    }

    public void ShowItemPopup(int value, Vector3 location) {
        ShowValueChangePopup(value, location);
    }

    public void SetRemoveItemText(bool canSell) {
        if (canSell) {
            _removeItemGuidance.text = "right click to sell\nctrl+click to sell/use " + Constants.instance.shiftClickQuantity;
        } else {
            _removeItemGuidance.text = "right click to discard\nctrl+click to discard/use " + Constants.instance.shiftClickQuantity;
        }
        Rebuild(_removeItemGuidance.GetComponent<RectTransform>());
    }

    public void SetLockedSlotsStatus(bool active) {
        _inventoryUI.SetLockedSlotsStatus(active);
        Rebuild(_inventoryUI.slotContainer.GetComponent<RectTransform>());
    }

    public void ShowEndScreen(GameEndType gameEndType) {
        _gameEndScreen.gameObject.SetActive(true);
        StartCoroutine(_gameEndScreen.ShowEndScreen(gameEndType));
    }
}
