using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundProfileButton : MonoBehaviour {
    public PlayerBackground background;
    public BackgroundTooltip tooltip;
    
    [Header("References")]
    public Button button;
    public Image icon;

    private void Start() {
        SetBackground(background);
        button.onClick.AddListener(() => ChooseBackground());
    }

    public void SetBackground(PlayerBackground background) {
        icon.sprite = background.sprite;
        tooltip.background = background;
    }

    public void ChooseBackground() {
        if (GameManager.instance.gameStatus == GameStatus.MENU) {
            background.ApplyBackground(GameManager.instance.player);
            GameManager.instance.StartGame();
        }
    }
}
