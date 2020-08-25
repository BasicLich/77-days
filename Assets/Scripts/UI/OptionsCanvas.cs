using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsCanvas : MonoBehaviour {
    public GameObject skipButton;
    public GameObject remainButton;
    public GameObject foodCheckbox;

    public void ActivateSkipButton(bool active) {
        skipButton.SetActive(active);
    }

    public void ActivateRemainButton(bool active) {
        remainButton.SetActive(active);
    }

    public void ActivateFoodCheckbox(bool active) {
        foodCheckbox.SetActive(active);
    }
}
