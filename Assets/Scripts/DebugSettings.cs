using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DebugSettings {
    [Header("Settings")]
    public bool debugOn;
    [Tooltip("Set to true to lift buying restrictions.")]
    public bool canBuyAll;
    [Tooltip("Set to true to allow the selection of any card.")]
    public bool canSelectAll;
    public bool fullInvItems;

    [Header("References")]
    public PlayerBackground fullInvBackground;

    public void DisableDebug() {
        canBuyAll = false;
        canSelectAll = false;
    }

    public void ApplyDebug() {
        if (fullInvItems) {
            fullInvBackground.ApplyBackground(GameManager.instance.player);
        }
    }
}
