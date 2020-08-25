using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BasicTooltip : MonoBehaviour {
    [SerializeField] protected RectTransform _background = null;
    private Vector3[] _corners = new Vector3[4];
    private Vector3 _topLeftCorner;
    private Vector3 _bottomRightCorner;
    public TextMeshProUGUI text;

    private void Update() {
        FollowCursor();
    }

    protected void FollowCursor() {
        Vector3 pos = Input.mousePosition;
        transform.position = pos;

        // Move tooltip to its bottom right position, thereby putting its top left corner at the mouse
        _background.GetWorldCorners(_corners);
        _bottomRightCorner = _corners[3];
        transform.position = _bottomRightCorner;

        KeepWithinScreen(transform.position);
    }

    // Adjusts position if tooltip is off-screen, given the point to keep to the mouse
    private void KeepWithinScreen(Vector3 centrePos) {
        _background.GetWorldCorners(_corners);
        _topLeftCorner = _corners[1];
        if (_topLeftCorner.x < 0) {
            centrePos.x += -_topLeftCorner.x;
        }
        if (_topLeftCorner.y > Screen.height) {
            centrePos.y -= _topLeftCorner.y - Screen.height;
        }
        _bottomRightCorner = _corners[3];
        if (_bottomRightCorner.x > Screen.width) {
            centrePos.x -= _bottomRightCorner.x - Screen.width;
        }
        if (_bottomRightCorner.y < 0) {
            centrePos.y += -_bottomRightCorner.y;
        }
        transform.position = centrePos;
    }

    private void OnEnable() {
        UpdateText();
    }

    protected virtual void UpdateText() {
        UIManager.Rebuild(_background);
        FollowCursor();
        UIManager.Rebuild(_background);
    }
}
