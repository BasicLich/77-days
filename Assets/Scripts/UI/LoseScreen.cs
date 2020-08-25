using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LoseScreen : MonoBehaviour {
    public TextMeshProUGUI details;

    public void SetDetails(string text) {
        details.text = text;
    }
}
