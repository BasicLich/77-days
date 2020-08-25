using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DestinationCanvas : MonoBehaviour {
    public Image flagIcon;
    public TextMeshProUGUI destinationName;
    public Image foodIcon;
    public TextMeshProUGUI costText;
    public Color disabledColour;

    public void SetCost(int cost) {
        costText.text = cost.ToString();
    }

    public void SetEnabled(bool enabled) {
        if (enabled) {
            SetColour(Color.white);
        } else {
            SetColour(disabledColour);
        }
    }

    private void SetColour(Color colour) {
        flagIcon.color = colour;
        destinationName.color = colour;
        costText.color = colour;
        foodIcon.color = colour;
    }
}
