using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Checkbox : MonoBehaviour {
    [Header("References")]
    public Image box;
    public Sprite checkedSprite;
    public Sprite uncheckedSprite;

    protected virtual void Start() {
        SetChecked(DetermineChecked());
    }

    protected abstract bool DetermineChecked();

    protected void SetChecked(bool setting) {
        if (setting) {
            box.sprite = checkedSprite;
        } else {
            box.sprite = uncheckedSprite;
        }
    }

    protected abstract void ToggleChecked();

    public void Toggle() {
        if (DetermineChecked()) {
            box.sprite = uncheckedSprite;
        } else {
            box.sprite = checkedSprite;
        }
        ToggleChecked();
    }
}
