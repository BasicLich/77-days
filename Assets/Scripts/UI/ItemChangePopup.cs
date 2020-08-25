using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemChangePopup : MonoBehaviour {
    [Header("References")]
    [SerializeField] private TextMeshProUGUI _text = null;

    [Header("Popup Properties")]
    [Tooltip("Duration for which UI's alpha is unchanged.")]
    [SerializeField] private float _opaqueTime = 0;
    [Tooltip("Duration of fade animation.")]
    [SerializeField] private float _fadeTime = 0;
    [Tooltip("Speed of text moving up.")]
    [SerializeField] private float _ascensionSpeed = 0;
    /*
    [Tooltip("Text colour for positive values.")]
    [SerializeField] private Color32 _positiveColour = Color.white;
    [Tooltip("Text colour for negative values.")]
    [SerializeField] private Color32 _negativeColour = Color.white;
    */

    private bool _floatUp = true;

    public void Initialize(string text, Vector3 location) {
        _text.text = text;
        _text.transform.position = location;
        StartCoroutine(Fade());
    }

    public void Initialize(int value, Vector3 location) {
        if (value == 0) {
            Destroy(gameObject);
            return;
        }
        bool positive = value > 0;
        _floatUp = positive;
        if (positive) {
            _text.text = "+" + value.ToString();
        } else {
            _text.text = value.ToString();
        }
        _text.transform.position = location;
        StartCoroutine(Fade());
    }

    private IEnumerator Fade() {
        float totalLifetime = _opaqueTime + _fadeTime;
        float elapsedTime = 0;
        int sign = _floatUp ? 1 : -1;
        while (elapsedTime < totalLifetime) {
            _text.transform.position += new Vector3(0, _ascensionSpeed * Time.deltaTime * sign, 0);
            elapsedTime += Time.deltaTime;
            if (elapsedTime > _opaqueTime) {
                float interpolant = Mathf.InverseLerp(_opaqueTime, totalLifetime, elapsedTime);
                Color32 newColour = _text.color;
                newColour.a = (byte) Mathf.Lerp(255, 0, interpolant);
                _text.color = newColour;
            }
            yield return null;
        }
        Destroy(gameObject);
    }
}
