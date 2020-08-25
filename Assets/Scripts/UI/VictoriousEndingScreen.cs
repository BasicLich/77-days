using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class VictoriousEndingScreen : MonoBehaviour {
    [TextArea(10, 10)] public string initialEndingText;

    public TextMeshProUGUI detailsText;
    private void OnEnable() {
        UpdateText();
    }

    private void UpdateText() {
        detailsText.text = initialEndingText + Utils.GetFormattedDays(GameManager.instance.day);
    }
}
